using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Authentication;
using System.Linq;

namespace Saml2.Authentication.Core.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void DeleteAllSessionCookies(this HttpResponse response, HttpRequest request, CookieOptions options)
        {
            var cookies = request.Cookies;
            foreach (var coookie in cookies.Where(c => c.Key.StartsWith(Saml2Defaults.SessionKeyPrefix)))
                response.Cookies.Delete(coookie.Key, options);
        }
    }
}