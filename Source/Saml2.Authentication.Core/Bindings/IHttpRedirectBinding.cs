using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Saml2.Authentication.Core.Bindings
{
    public interface IHttpRedirectBinding
    {
        string BuildQuery(string request, AsymmetricAlgorithm signingKey, string hashingAlgorithm);

        bool IsValid(HttpRequest request);

        string GetSamlResponse(HttpRequest request);
    }
}