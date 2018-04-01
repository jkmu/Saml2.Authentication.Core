using System.Security.Claims;
using System.Threading.Tasks;
using DemoWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DemoWebApp.Services
{
    public class DemoWebAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DemoWebAppClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor, IHttpContextAccessor httpContextAccessor) : base(userManager,
            optionsAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var service =
                (SignInManager<ApplicationUser>) _httpContextAccessor.HttpContext.RequestServices.GetService(
                    typeof(SignInManager<ApplicationUser>));
            var info = await service.GetExternalLoginInfoAsync();

            var claimsIdentity = await base.GenerateClaimsAsync(user);
            claimsIdentity.AddClaims(info.Principal.Claims); //Add external claims to cookie. The SessionIndex and Subject are required for SLO
            return claimsIdentity;
        }
    }
}
