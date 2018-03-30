using System.Linq;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Authentication;

namespace Saml2.Authentication.Core.Extensions
{
  public static  class HttpResponseExtensions
    {
        public static void DeleteAllRequestIdCookies(this HttpResponse response, HttpRequest request, CookieOptions options)
        {
            var cookies = request.Cookies;
            foreach (var coookie in cookies.Where(c => c.Key.StartsWith(Saml2Defaults.RequestIdCookiePrefix)))
            {
                response.Cookies.Delete(coookie.Key, options);
            }
        }
    }
}
