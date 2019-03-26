namespace Saml2.Authentication.Core.Extensions
{
    using Microsoft.AspNetCore.Http;

    public static class HttpRequestExtensions
    {
        public static string GetBaseUrl(this HttpRequest request)
        {
            return request.Scheme + "://" + request.Host.Value;
        }
    }
}