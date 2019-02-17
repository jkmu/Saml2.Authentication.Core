using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Saml2.Authentication.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetBaseUrl(this HttpRequest request)
        {
            return request.Scheme + "://" + request.Host.Value;
        }
    }
}