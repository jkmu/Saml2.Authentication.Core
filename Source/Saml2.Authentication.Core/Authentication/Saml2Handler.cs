namespace Saml2.Authentication.Core.Authentication
{
    using System;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Bindings;
    using Configuration;
    using Extensions;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Providers;
    using Services;
    using Session;

    public class Saml2Handler : AuthenticationHandler<Saml2Options>, IAuthenticationRequestHandler, IAuthenticationSignOutHandler
    {
        private const string AuthnRequestIdKey = "AuthnRequestId";
        private const string LogoutRequestIdKey = "LogoutRequestId";

        private readonly ISessionStore _sessionStore;

        private readonly ISamlService _samlService;
        private readonly IConfigurationProvider _configurationProvider;

        private readonly IHttpArtifactBinding _httpArtifactBinding;
        private readonly IHttpRedirectBinding _httpRedirectBinding;
        private readonly ILogger _logger;

        public Saml2Handler(
            IOptionsMonitor<Saml2Options> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpRedirectBinding httpRedirectBinding,
            IHttpArtifactBinding httpArtifactBinding,
            ISessionStore sessionStore,
            ISamlService samlService,
            IConfigurationProvider configurationProvider)
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger(typeof(Saml2Handler));
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _sessionStore = sessionStore;
            _samlService = samlService;
            _configurationProvider = configurationProvider;
        }

        private ServiceProviderConfiguration ServiceProviderConfiguration => _configurationProvider.ServiceProviderConfiguration;

        public async Task<bool> HandleRequestAsync()
        {
            if (await HandleSignIn())
            {
                return true;
            }

            if (await HandleSignOut())
            {
                return true;
            }

            return await HandleHttpArtifact(Options.IdentityProviderName);
        }

        public async Task SignOutAsync(AuthenticationProperties properties)
        {
            _logger.LogDebug($"Entering {nameof(SignOutAsync)}", properties);

            var logoutRequestId = CreateUniqueId();
            properties ??= new AuthenticationProperties();

            properties.Items.Add(LogoutRequestIdKey, logoutRequestId);
            properties.Items.Add(nameof(Options.SignOutScheme), Options.SignOutScheme);
            await _sessionStore.SaveAsync<AuthenticationProperties>(properties);

            await _samlService.InitiateSloAsync(Options.IdentityProviderName, logoutRequestId);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await Context.AuthenticateAsync(Options.SignInScheme);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            _logger.LogDebug($"Entering {nameof(HandleChallengeAsync)}", properties);

            properties ??= new AuthenticationProperties();

            var authnRequestId = CreateUniqueId();
            properties.Items.Add(AuthnRequestIdKey, authnRequestId);
            properties.Items.Add(nameof(Options.SignInScheme), Options.SignInScheme);

            await _sessionStore.SaveAsync<AuthenticationProperties>(properties);

            await _samlService.InitiateSsoAsync(Options.IdentityProviderName, authnRequestId);
        }

        private static string CreateUniqueId() => Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

        private async Task<bool> HandleSignOut()
        {
            if (Request.Path.Value != null && (!Request.Path.Value.EndsWith(ServiceProviderConfiguration.SingleLogoutServiceUrl, StringComparison.OrdinalIgnoreCase)
                                               || !Request.Path.Value.EndsWith(ServiceProviderConfiguration.SingleLogoutResponseServiceUrl, StringComparison.OrdinalIgnoreCase)
                                               || !_httpRedirectBinding.IsValid()))
            {
                return false;
            }

            _logger.LogDebug($"Entering {nameof(HandleSignOut)}");

            // idp initiated logout. TODO: BUG:Context.User and cookies are not populated
            if (_httpRedirectBinding.IsLogoutRequest())
            {
                var logoutResponseUrl = await _samlService.ReceiveIdpInitiatedLogoutRequest(Options.IdentityProviderName);
                await Context.SignOutAsync(Options.SignOutScheme, new AuthenticationProperties());

                Context.Response.Redirect(logoutResponseUrl);
                return true;
            }

            // sp initiated logout
            var properties = await _sessionStore.LoadAsync<AuthenticationProperties>() ?? new AuthenticationProperties();
            properties.Items.TryGetValue(LogoutRequestIdKey, out var initialLogoutRequestId);
            properties.Items.TryGetValue(nameof(Options.SignOutScheme), out var signOutScheme);

            if (!await _samlService.ReceiveSpInitiatedLogoutResponse(Options.IdentityProviderName, initialLogoutRequestId))
            {
                return false;
            }

            await Context.SignOutAsync(signOutScheme, properties);

            await _sessionStore.RemoveAsync<AuthenticationProperties>();

            var redirectUrl = GetRedirectUrl(properties);

            _logger.LogDebug($"Method={nameof(HandleSignOut)}. Received and handled sp initiated logout response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl);
            return true;
        }

        private async Task<bool> HandleSignIn()
        {
            if (Request.Path.Value != null && (!Request.Path.Value.EndsWith(ServiceProviderConfiguration.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase) || !_httpRedirectBinding.IsValid()))
            {
                return false;
            }

            _logger.LogDebug($"Entering {nameof(HandleSignIn)}");

            var properties = await _sessionStore.LoadAsync<AuthenticationProperties>() ?? new AuthenticationProperties();
            properties.Items.TryGetValue(AuthnRequestIdKey, out var initialAuthnRequestId);
            properties.Items.TryGetValue(nameof(Options.SignInScheme), out var signInScheme);

            var assertion = await _samlService.ReceiveHttpRedirectAuthnResponseAsync(initialAuthnRequestId);
            await _samlService.SignInAsync(signInScheme, assertion, properties);

            await _sessionStore.RemoveAsync<AuthenticationProperties>();
            var redirectUrl = GetRedirectUrl(properties);

            _logger.LogDebug($"Method={nameof(HandleSignIn)}. Received and handled SSO redirect response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl);
            return true;
        }

        private async Task<bool> HandleHttpArtifact(string providerName)
        {
            if (Request.Path.Value != null && (!Request.Path.Value.EndsWith(ServiceProviderConfiguration.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase)
                                               || !_httpArtifactBinding.IsValid()))
            {
                return false;
            }

            _logger.LogDebug($"Entering {nameof(HandleHttpArtifact)}");

            var properties = await _sessionStore.LoadAsync<AuthenticationProperties>() ?? new AuthenticationProperties();

            properties.Items.TryGetValue(AuthnRequestIdKey, out var initialAuthnRequestId);

            var assertion = await _samlService.ReceiveHttpArtifactAuthnResponseAsync(providerName, initialAuthnRequestId);
            await _samlService.SignInAsync(Options.SignInScheme, assertion, properties);

            await _sessionStore.RemoveAsync<AuthenticationProperties>();

            var redirectUrl = GetRedirectUrl(properties);

            _logger.LogDebug($"Method={nameof(HandleHttpArtifact)}. Received and handled SSO artifact response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl);
            return true;
        }

        private string GetRedirectUrl(AuthenticationProperties authenticationProperties) => authenticationProperties
            .RedirectUri.IsNotNullOrEmpty()
            ? authenticationProperties.RedirectUri
            : Options.DefaultRedirectUrl;
    }
}