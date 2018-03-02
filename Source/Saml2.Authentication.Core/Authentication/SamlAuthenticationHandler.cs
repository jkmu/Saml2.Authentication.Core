using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Saml2.Authentication.Core.Authentication
{
    public class SamlAuthenticationHandler : AuthenticationHandler<SamlAuthenticationOptions>, IAuthenticationRequestHandler
    {
        public SamlAuthenticationHandler(
            IOptionsMonitor<SamlAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        public async Task<bool> HandleRequestAsync()
        {
            if (await HandleSignIn())
            {
                return true;
            }

            return await HandleSignout();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.Fail("Not supported"));
        }


        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Redirect(Options.AssertionConsumerServiceUrl);
            return Task.FromResult(0);
        }

        protected override Task InitializeHandlerAsync()
        {
            return base.InitializeHandlerAsync();
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }

        private Task<bool> HandleSignout()
        {
            return Task.FromResult(false);
        }

        private async Task<bool> HandleSignIn()
        {
            if (Request.Path.Value.EndsWith(Options.AssertionConsumerServiceUrl, StringComparison.OrdinalIgnoreCase))
            {
                const string redirectUrl = "/Account/ExternalLoginCallback";
                var properties = new AuthenticationProperties
                {
                    RedirectUri = redirectUrl,
                    Items = { new KeyValuePair<string, string>("LoginProvider", Scheme.Name) }
                };

                var claims = new List<Claim>
                {
                    new Claim("subject", Guid.NewGuid().ToString()),
                    new Claim("name", "J K M"),
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email, $"{Guid.NewGuid().ToString()}@gmail.com")
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);

                var principal = new ClaimsPrincipal(identity);

                await Context.SignInAsync(Options.SignInScheme, principal, properties);

                Response.Redirect(redirectUrl);
                return true;
            }

            return false;
        }
    }
}
