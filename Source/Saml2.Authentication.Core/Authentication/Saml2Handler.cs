﻿namespace Saml2.Authentication.Core.Authentication
{
    using System;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Bindings;

    using Extensions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Options;

    using Providers;

    using Session;

    public class Saml2Handler : AuthenticationHandler<Saml2Options>, IAuthenticationRequestHandler, IAuthenticationSignOutHandler
    {
        private const string AuthnRequestIdKey = "AuthnRequestId";
        private const string LogoutRequestIdKey = "LogoutRequestId";

        private readonly ISessionStore _sessionStore;

        private readonly ISaml2AuthenticationProvider _authenticationProvider;

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
            ISaml2AuthenticationProvider authenticationProvider)
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger(typeof(Saml2Handler));
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _sessionStore = sessionStore;
            _authenticationProvider = authenticationProvider;
        }

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

            return await HandleHttpArtifact();
        }

        public async Task SignOutAsync(AuthenticationProperties properties)
        {
            _logger.LogDebug($"Entering {nameof(SignOutAsync)}", properties);

            var logoutRequestId = CreateUniqueId();
            properties = properties ?? new AuthenticationProperties();

            properties.Items.Add(LogoutRequestIdKey, logoutRequestId);
            await _sessionStore.SaveAsync<AuthenticationProperties>(properties);

            await _authenticationProvider.InitiateSloAsync(logoutRequestId);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Fail("Not supported"));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            _logger.LogDebug($"Entering {nameof(HandleChallengeAsync)}", properties);

            properties = properties ?? new AuthenticationProperties();

            var authnRequestId = CreateUniqueId();
            properties.Items.Add(AuthnRequestIdKey, authnRequestId);

            await _sessionStore.SaveAsync<AuthenticationProperties>(properties);

            await _authenticationProvider.InitiateSsoAsync(authnRequestId);
        }

        private static string CreateUniqueId() => Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

        private async Task<bool> HandleSignOut()
        {
            if (!Request.Path.Value.EndsWith(Options.SingleLogoutServiceUrl, StringComparison.OrdinalIgnoreCase)
                || !_httpRedirectBinding.IsValid())
            {
                return false;
            }

            _logger.LogDebug($"Entering {nameof(HandleSignOut)}");

            // idp initiated logout. TODO: BUG:Context.User and cookies are not populated
            if (_httpRedirectBinding.IsLogoutRequest())
            {
                var logoutResponseUrl = await _authenticationProvider.ReceiveIdpInitiatedLogoutRequest();
                await Context.SignOutAsync(Options.SignOutScheme, new AuthenticationProperties());

                Context.Response.Redirect(logoutResponseUrl, true);
                return true;
            }

            // sp initiated logout
            var properties = await _sessionStore.LoadAsync<AuthenticationProperties>() ?? new AuthenticationProperties();
            properties.Items.TryGetValue(LogoutRequestIdKey, out var initialLogoutRequestId);

            if (!await _authenticationProvider.ReceiveSpInitiatedLogoutResponse(initialLogoutRequestId))
            {
                return false;
            }

            await Context.SignOutAsync(Options.SignOutScheme, properties);

            await _sessionStore.RemoveAsync<AuthenticationProperties>();

            var redirectUrl = GetRedirectUrl(properties);

            _logger.LogDebug($"Method={nameof(HandleSignOut)}. Received and handled sp initiated logout response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl, true);
            return true;
        }

        private async Task<bool> HandleSignIn()
        {
            if (!Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase)
                || !_httpRedirectBinding.IsValid())
            {
                return false;
            }

            _logger.LogDebug($"Entering {nameof(HandleSignIn)}");

            var properties = await _sessionStore.LoadAsync<AuthenticationProperties>() ?? new AuthenticationProperties();
            properties.Items.TryGetValue(AuthnRequestIdKey, out var initialAuthnRequestId);

            var assertion = await _authenticationProvider.ReceiveHttpRedirectAuthnResponseAsync(initialAuthnRequestId);
            await _authenticationProvider.SignInAsync(Options.SignInScheme, assertion, properties);

            await _sessionStore.RemoveAsync<AuthenticationProperties>();
            var redirectUrl = GetRedirectUrl(properties);

            _logger.LogDebug($"Method={nameof(HandleSignIn)}. Received and handled SSO redirect response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl, true);
            return true;
        }

        private async Task<bool> HandleHttpArtifact()
        {
            if (!Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase)
                || !_httpArtifactBinding.IsValid())
            {
                return false;
            }

            _logger.LogDebug($"Entering {nameof(HandleHttpArtifact)}");

            var properties = await _sessionStore.LoadAsync<AuthenticationProperties>() ?? new AuthenticationProperties();

            properties.Items.TryGetValue(AuthnRequestIdKey, out var initialAuthnRequestId);

            var assertion = await _authenticationProvider.ReceiveHttpArtifactAuthnResponseAsync(initialAuthnRequestId);
            await _authenticationProvider.SignInAsync(Options.SignInScheme, assertion, properties);

            await _sessionStore.RemoveAsync<AuthenticationProperties>();

            var redirectUrl = GetRedirectUrl(properties);

            _logger.LogDebug($"Method={nameof(HandleHttpArtifact)}. Received and handled SSO artifact response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl, true);
            return true;
        }

        private string GetRedirectUrl(AuthenticationProperties authenticationProperties) => authenticationProperties
            .RedirectUri.IsNotNullOrEmpty()
            ? authenticationProperties.RedirectUri
            : Options.DefaultRedirectUrl;
    }
}