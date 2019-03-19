using DemoWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoWebApp.Services
{
    public class DemoWebAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DemoWebAppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            IHttpContextAccessor httpContextAccessor)
            : base(userManager, optionsAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext Context => _httpContextAccessor.HttpContext;

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var signInManager =
                (SignInManager<ApplicationUser>)Context.RequestServices.GetService(
                    typeof(SignInManager<ApplicationUser>));

            var claims = new List<Claim>();
            var authenticationSchemes = await signInManager.GetExternalAuthenticationSchemesAsync();
            foreach (var scheme in authenticationSchemes)
            {
                var authenticateResult = await Context.AuthenticateAsync(scheme.Name);
                if (!authenticateResult.Succeeded)
                {
                    continue;
                }

                var sessionIndex = authenticateResult.Principal.Claims.First(c => c.Type == Saml2ClaimTypes.SessionIndex);
                var saml2Subject = authenticateResult.Principal.Claims.First(c => c.Type == Saml2ClaimTypes.Subject);
                claims.Add(sessionIndex);
                claims.Add(saml2Subject);
            }

            var claimsIdentity = await base.GenerateClaimsAsync(user);
            claimsIdentity.AddClaims(claims); //Add external claims to cookie. The SessionIndex and Subject are required for SLO
            return claimsIdentity;
        }
    }
}
