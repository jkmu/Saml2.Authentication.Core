using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Bindings;
using Saml2.Authentication.Core.Factories;
using Saml2.Authentication.Core.Options;
using Saml2.Authentication.Core.Services;

namespace Saml2.Authentication.Core.Authentication
{
    public class Saml2Handler : AuthenticationHandler<Saml2Options>, IAuthenticationRequestHandler, IAuthenticationSignOutHandler
    {
        private readonly ISamlService _samlService;
        private readonly IHttpRedirectBinding _httpRedirectBinding;
        private readonly ISaml2ClaimFactory _claimFactory;

        public Saml2Handler(
            IOptionsMonitor<Saml2Options> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISamlService samlService,
            IHttpRedirectBinding httpRedirectBinding,
            ISaml2ClaimFactory claimFactory)
            : base(options, logger, encoder, clock)
        {
            _samlService = samlService;
            _httpRedirectBinding = httpRedirectBinding;
            _claimFactory = claimFactory;
        }

        public async Task<bool> HandleRequestAsync()
        {
            if (await HandleSignIn())
            {
                return true;
            }

            return await HandleSignOut();
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Fail("Not supported"));
        }


        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authnRequestId = GetUniqueAuthnRequestId();

            var cookieOptions = Options.AuthnRequestIdCookie.Build(Context, Clock.UtcNow);
            Response.Cookies.Append(Options.AuthnRequestIdCookie.Name, Options.StringDataFormat.Protect(authnRequestId),
                cookieOptions);

            var relayState = Options.StateDataFormat.Protect(properties);
            Response.Redirect(_samlService.GetSingleSignOnRequestUrl(authnRequestId, relayState));
            return Task.CompletedTask;
        }

        private Task<bool> HandleSignOut()
        {
            return Task.FromResult(false);
        }

        private async Task<bool> HandleSignIn()
        {
            if (Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase))
            {
                if (!_httpRedirectBinding.IsValid(Context.Request))
                {
                    return false;
                }

                var relayState = _httpRedirectBinding.GetSamlResponse(Context.Request);
                var authenticationProperties = Options.StateDataFormat.Unprotect(relayState);// check for null

                var authnRequestIdCookie = Request.Cookies.Keys.FirstOrDefault(key => key == Options.AuthnRequestIdCookie.Name);
                if (string.IsNullOrEmpty(authnRequestIdCookie))
                {
                    throw new ArgumentNullException(nameof(authnRequestIdCookie));
                }

                var initialAuthnRequestId = Options.StringDataFormat.Unprotect(authnRequestIdCookie);
                var cookieOptions = Options.AuthnRequestIdCookie.Build(Context, Clock.UtcNow);
                Response.Cookies.Delete(Options.AuthnRequestIdCookie.Name, cookieOptions); // delete all starting with prefix

                var base64EncodedSamlResponse = _httpRedirectBinding.GetSamlResponse(Context.Request);
                var result = _samlService.HandleHttpRedirectResponse(base64EncodedSamlResponse, initialAuthnRequestId);

                var claims = _claimFactory.Create(result);
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                await Context.SignInAsync(Options.SignInScheme, principal, authenticationProperties);
                return true;
            }

            return false;
        }

        private static string GetUniqueAuthnRequestId(int length = 32)
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
    }
}
