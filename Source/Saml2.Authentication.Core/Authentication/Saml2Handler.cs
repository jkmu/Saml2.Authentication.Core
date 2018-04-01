using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using dk.nita.saml20;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
    public class Saml2Handler : AuthenticationHandler<Saml2Options>, IAuthenticationRequestHandler, IAuthenticationSignOutHandler
    {
        private readonly ISamlService _samlService;
        private readonly IHttpRedirectBinding _httpRedirectBinding;
        private readonly IHttpArtifactBinding _httpArtifactBinding;
        private readonly ISaml2ClaimFactory _claimFactory;

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
            _samlService = samlService;
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _claimFactory = claimFactory;
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

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            var logoutRequestId = CreateUniqueId();
            var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Response.Cookies.Append(Options.RequestIdCookie.Name, Options.StringDataFormat.Protect(logoutRequestId),
                cookieOptions);

            var relayState = Options.StateDataFormat.Protect(properties);
            var sessionIndex = Context.User.GetSessionIndex();
            var subject = Context.User.GetSubject();
            var logoutRequestUrl = _samlService.GetLogoutRequest(logoutRequestId, sessionIndex, subject, relayState);
            Response.Redirect(logoutRequestUrl);
            return Task.CompletedTask;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Fail("Not supported"));
        }


        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authnRequestId = CreateUniqueId();

            Response.DeleteAllRequestIdCookies(Request, new CookieOptions());

            var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
            Response.Cookies.Append(Options.RequestIdCookie.Name, Options.StringDataFormat.Protect(authnRequestId),
                cookieOptions);

            var relayState = Options.StateDataFormat.Protect(properties);
            var requestUrl = _samlService.GetAuthnRequest(authnRequestId, relayState,
                $"{Request.GetBaseUrl()}/{Options.AssertionConsumerServiceUrl}");
            Response.Redirect(requestUrl);
            return Task.CompletedTask;
        }

        private async Task<bool> HandleSignOut()
        {
            if (Request.Path.Value.EndsWith(Options.SingleLogoutServiceUrl, StringComparison.OrdinalIgnoreCase))
            {
                if (!_httpRedirectBinding.IsValid(Context.Request))
                {
                    return false;
                }

                var url = Context.Request.GetEncodedUrl();
                var response = _httpRedirectBinding.GetResponse(Context.Request);
                var authenticationProperties = Options.StateDataFormat.Unprotect(response.RelayState) ?? new AuthenticationProperties();

                var initialLogoutRequestId = GetRequestId();
                if (!_samlService.HandleLogoutResponse(new Uri(url), initialLogoutRequestId)) //TODO add support for idp initiated logout
                {
                    return false;
                }

                await Context.SignOutAsync(Options.SignOutScheme, authenticationProperties);

                var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
                Response.DeleteAllRequestIdCookies(Context.Request, cookieOptions);

                var redirectUrl = GetRedirectUrl(authenticationProperties);
                Response.Redirect(redirectUrl, true);
                return true;
            }
            return false;
        }

        private async Task<bool> HandleSignIn()
        {
            if (Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase))
            {
                if (!_httpRedirectBinding.IsValid(Context.Request))
                {
                    return false;
                }

                var result = _httpRedirectBinding.GetResponse(Context.Request);
                var initialAuthnRequestId = GetRequestId();
                var base64EncodedSamlResponse = result.Response;
                var assertion = _samlService.HandleHttpRedirectResponse(base64EncodedSamlResponse, initialAuthnRequestId);
                if (assertion == null)
                {
                    throw new Saml20Exception("Assertion canot be empty");
                }

                var authenticationProperties = Options.StateDataFormat.Unprotect(result.RelayState) ?? new AuthenticationProperties();
                var claims = _claimFactory.Create(assertion);
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                await Context.SignInAsync(Options.SignInScheme, principal, authenticationProperties);

                var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
                Response.DeleteAllRequestIdCookies(Context.Request, cookieOptions);

                var redirectUrl = GetRedirectUrl(authenticationProperties);
                Response.Redirect(redirectUrl);
                return true;
            }

            return false;
        }

        private async Task<bool> HandleHttpArtifact()
        {
            if (Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase))
            {
                if (!_httpArtifactBinding.IsValid(Context.Request))
                {
                    return false;
                }

                var relayState = _httpArtifactBinding.GetRelayState(Context.Request);
                var authenticationProperties = Options.StateDataFormat.Unprotect(relayState) ?? new AuthenticationProperties();

                var initialAuthnRequestId = GetRequestId(); //TODO validate inResponseTo

                var result = _samlService.HandleHttpArtifactResponse(Context.Request);
                var claims = _claimFactory.Create(result);
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                await Context.SignInAsync(Options.SignInScheme, principal, authenticationProperties);

                var cookieOptions = Options.RequestIdCookie.Build(Context, Clock.UtcNow);
                Response.DeleteAllRequestIdCookies(Context.Request, cookieOptions);

                var redirectUrl = GetRedirectUrl(authenticationProperties);
                Response.Redirect(redirectUrl);
                return true;
            }
            return false;
        }

        private static string CreateUniqueId(int length = 32)
        {
            var bytes = new byte[length];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(bytes);
                var hex = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes)
                {
                    hex.AppendFormat("{0:x2}", b);
                }

                return hex.ToString();
            }
        }

        private string GetRequestId()
        {
            var requestIdCookie = Request.GetRequestIdCookie();
            if (string.IsNullOrEmpty(requestIdCookie))
            {
                throw new ArgumentNullException(nameof(requestIdCookie));
            }

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
