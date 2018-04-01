using System.Linq;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Authentication;

namespace Saml2.Authentication.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetRequestIdCookie(this HttpRequest request)
        {
            var cookies = request.Cookies;
            return cookies[cookies.Keys.FirstOrDefault(key => key.StartsWith(Saml2Defaults.RequestIdCookiePrefix))];
        }

        public static string GetBaseUrl(this HttpRequest request)
        {
            return request.Scheme + "://" + request.Host.Value;
        }
    }
}
