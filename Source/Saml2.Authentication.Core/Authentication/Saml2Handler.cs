using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Options;
using Saml2.Authentication.Core.Services;

namespace Saml2.Authentication.Core.Authentication
{
    public class Saml2Handler : AuthenticationHandler<Saml2Options>, IAuthenticationRequestHandler
    {
        private readonly ISamlService _samlService;

        public Saml2Handler(
            IOptionsMonitor<Saml2Options> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISamlService samlService)
            : base(options, logger, encoder, clock)
        {
            _samlService = samlService;
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
            //Response.Redirect(Options.AssertionConsumerServiceUrl);
            Response.Redirect(_samlService.GetSingleSignOnRequestUrl());
            return Task.CompletedTask;
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
