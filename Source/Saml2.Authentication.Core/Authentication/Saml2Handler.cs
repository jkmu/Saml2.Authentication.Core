using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using dk.nita.saml20;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Bindings;
using Saml2.Authentication.Core.Extensions;
using Saml2.Authentication.Core.Factories;
using Saml2.Authentication.Core.Options;
using Saml2.Authentication.Core.Services;

namespace Saml2.Authentication.Core.Authentication
{
    public class Saml2Handler : AuthenticationHandler<Saml2Options>, IAuthenticationRequestHandler,
        IAuthenticationSignOutHandler
    {
        private readonly ISaml2ClaimFactory _claimFactory;
        private readonly IHttpArtifactBinding _httpArtifactBinding;
        private readonly IHttpRedirectBinding _httpRedirectBinding;
        private readonly ILogger _logger;
        private readonly ISamlService _samlService;

        public Saml2Handler(
            IOptionsMonitor<Saml2Options> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISamlService samlService,
            IHttpRedirectBinding httpRedirectBinding,
            IHttpArtifactBinding httpArtifactBinding,
            ISaml2ClaimFactory claimFactory)
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger(typeof(Saml2Handler));
            _samlService = samlService;
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _claimFactory = claimFactory;
        }

        public async Task<bool> HandleRequestAsync()
        {
            if (await HandleSignIn())
                return true;

            if (await HandleSignOut())
                return true;
            return await HandleHttpArtifact();
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            _logger.LogDebug($"Entering {nameof(SignOutAsync)}", properties);

            var logoutRequestId = CreateUniqueId();
            var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Response.Cookies.Append(Options.RequestIdCookie.Name, Options.StringDataFormat.Protect(logoutRequestId),
                cookieOptions);

            var relayState = Options.StateDataFormat.Protect(properties);
            var sessionIndex = Context.User.GetSessionIndex();
            var subject = Context.User.GetSubject();

            var logoutRequestUrl = _samlService.GetLogoutRequest(logoutRequestId, sessionIndex, subject, relayState);

            _logger.LogDebug(
                $"Method={nameof(SignOutAsync)}. Redirecting to saml identity provider for SLO. Url={logoutRequestUrl}");

            Context.Response.Redirect(logoutRequestUrl, true);
            return Task.CompletedTask;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Fail("Not supported"));
        }


        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            _logger.LogDebug($"Entering {nameof(HandleChallengeAsync)}", properties);

            var authnRequestId = CreateUniqueId();

            var deleteCookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Response.DeleteAllRequestIdCookies(Context.Request, deleteCookieOptions);

            var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Response.Cookies.Append(Options.RequestIdCookie.Name, Options.StringDataFormat.Protect(authnRequestId),
                cookieOptions);

            var relayState = Options.StateDataFormat.Protect(properties);
            var requestUrl = _samlService.GetAuthnRequest(authnRequestId, relayState,
                $"{Request.GetBaseUrl()}/{Options.AssertionConsumerServiceUrl}");

            _logger.LogDebug(
                $"Method={nameof(HandleChallengeAsync)}. Redirecting to saml identity provider for SSO. Url={requestUrl}");
            Context.Response.Redirect(requestUrl, true);
            return Task.CompletedTask;
        }

        private async Task<bool> HandleSignOut()
        {
            if (!Request.Path.Value.EndsWith(Options.SingleLogoutServiceUrl, StringComparison.OrdinalIgnoreCase))
                return false;

            _logger.LogDebug($"Entering {nameof(HandleSignOut)}");

            if (!_httpRedirectBinding.IsValid(Context.Request))
                return false;

            var uri = new Uri(Context.Request.GetEncodedUrl());
            if (_httpRedirectBinding.IsLogoutRequest(Context.Request)
            ) //idp initiated logout. TODO: BUG:Context.User and cookies are not populated
            {
                var logoutReponse = _samlService.GetLogoutReponse(uri);
                if (logoutReponse.StatusCode != Saml2Constants.StatusCodes.Success ||
                    Context.User.Identity.IsAuthenticated)
                    return false;

                var relayState = _httpRedirectBinding.GetCompressedRelayState(Context.Request);
                var url = _samlService.GetLogoutResponseUrl(logoutReponse, relayState);
                await Context.SignOutAsync(Options.SignOutScheme, new AuthenticationProperties());

                Context.Response.Redirect(url, true);
                return true;
            }

            //sp initiated logout
            var response = _httpRedirectBinding.GetResponse(Context.Request);
            var authenticationProperties =
                Options.StateDataFormat.Unprotect(response.RelayState) ?? new AuthenticationProperties();

            var initialLogoutRequestId = GetRequestId();
            if (!_samlService.IsLogoutResponseValid(uri, initialLogoutRequestId))
                return false;

            await Context.SignOutAsync(Options.SignOutScheme, authenticationProperties);

            var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Context.Response.DeleteAllRequestIdCookies(Context.Request, cookieOptions);

            var redirectUrl = GetRedirectUrl(authenticationProperties);

            _logger.LogDebug(
                $"Method={nameof(HandleSignOut)}. Received and handled sp initiated logout response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl, true);
            return true;
        }

        private async Task<bool> HandleSignIn()
        {
            if (!Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase))
                return false;

            _logger.LogDebug($"Entering {nameof(HandleSignIn)}");

            if (!_httpRedirectBinding.IsValid(Context.Request))
                return false;

            var initialAuthnRequestId = GetRequestId();
            var result = _httpRedirectBinding.GetResponse(Context.Request);
            var base64EncodedSamlResponse = result.Response;
            var assertion = _samlService.HandleHttpRedirectResponse(base64EncodedSamlResponse, initialAuthnRequestId);

            var authenticationProperties =
                Options.StateDataFormat.Unprotect(result.RelayState) ?? new AuthenticationProperties();
            await SignIn(assertion, authenticationProperties);

            var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Response.DeleteAllRequestIdCookies(Context.Request, cookieOptions);

            var redirectUrl = GetRedirectUrl(authenticationProperties);

            _logger.LogDebug(
                $"Method={nameof(HandleSignIn)}. Received and handled SSO redirect response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl, true);
            return true;
        }

        private async Task<bool> HandleHttpArtifact()
        {
            if (!Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase))
                return false;

            _logger.LogDebug($"Entering {nameof(HandleHttpArtifact)}");

            if (!_httpArtifactBinding.IsValid(Context.Request))
                return false;

            var initialAuthnRequestId = GetRequestId(); //TODO validate inResponseTo

            var assertion = _samlService.HandleHttpArtifactResponse(Context.Request);
            var relayState = _httpArtifactBinding.GetRelayState(Context.Request);
            var authenticationProperties =
                Options.StateDataFormat.Unprotect(relayState) ?? new AuthenticationProperties();
            await SignIn(assertion, authenticationProperties);

            var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Response.DeleteAllRequestIdCookies(Context.Request, cookieOptions);

            var redirectUrl = GetRedirectUrl(authenticationProperties);

            _logger.LogDebug(
                $"Method={nameof(HandleHttpArtifact)}. Received and handled SSO artifact response. Redirecting to {redirectUrl}");

            Context.Response.Redirect(redirectUrl, true);
            return true;
        }

        private async Task SignIn(Saml2Assertion assertion, AuthenticationProperties authenticationProperties)
        {
            var claims = _claimFactory.Create(assertion);
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            await Context.SignInAsync(Options.SignInScheme, principal, authenticationProperties);
        }

        private static string CreateUniqueId(int length = 32)
        {
            var bytes = new byte[length];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(bytes);
                var hex = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes)
                    hex.AppendFormat("{0:x2}", b);

                return hex.ToString();
            }
        }

        private string GetRequestId()
        {
            var requestIdCookie = Request.GetRequestIdCookie();
            if (string.IsNullOrEmpty(requestIdCookie))
                throw new ArgumentNullException(nameof(requestIdCookie));

            return Options.StringDataFormat.Unprotect(requestIdCookie);
        }

        private string GetRedirectUrl(AuthenticationProperties authenticationProperties)
        {
            return authenticationProperties.RedirectUri.IsNotNullOrEmpty()
                ? authenticationProperties.RedirectUri
                : Options.DefaultRedirectUrl;
        }
    }
}