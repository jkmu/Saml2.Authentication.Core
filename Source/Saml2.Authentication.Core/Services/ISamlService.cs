using System;
using Microsoft.AspNetCore.Http;

namespace Saml2.Authentication.Core.Services
{
    public interface ISamlService
    {
        string GetAuthnRequest(string authnRequestId, string relayState, string assertionConsumerServiceUrl);

        Saml2Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId);

        Saml2Assertion HandleHttpArtifactResponse(HttpRequest request);

        bool HandleLogoutResponse(Uri uri, string originalRequestId);

        string GetLogoutRequest(string logoutRequestId, string sessionIndex, string subject, string relayState);
    }
}