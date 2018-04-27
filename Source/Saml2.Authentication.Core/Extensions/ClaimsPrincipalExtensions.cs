using System.Linq;
using System.Security.Claims;
using Saml2.Authentication.Core.Authentication;

namespace Saml2.Authentication.Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSessionIndex(this ClaimsPrincipal principal)
        {
            return GetClaimValueByType(principal, Saml2ClaimTypes.SessionIndex);
        }

        public static string GetSubject(this ClaimsPrincipal principal)
        {
            return GetClaimValueByType(principal, Saml2ClaimTypes.Subject);
        }

        public static string GetClaimValueByType(this ClaimsPrincipal principal, string type)
        {
            var claims = principal.Claims;
            var claim = claims.FirstOrDefault(cl => cl.Type == type);
            return claim != null ? claim.Value : string.Empty;
        }
    }
}