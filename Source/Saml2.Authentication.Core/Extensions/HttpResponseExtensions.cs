namespace Saml2.Authentication.Core.Extensions
{
    using System.Linq;
    using Authentication;
    using Microsoft.AspNetCore.Http;

    public static class HttpResponseExtensions
    {
        public static void DeleteAllSessionCookies(this HttpResponse response, HttpRequest request, CookieOptions options)
        {
            var cookies = request.Cookies;
            foreach (var cookie in cookies.Where(c => c.Key.StartsWith(Saml2Defaults.SessionKeyPrefix)))
            {
                response.Cookies.Delete(cookie.Key, options);
            }
        }
    }
}