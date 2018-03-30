using System;
using Microsoft.AspNetCore.Http;

namespace Saml2.Authentication.Core.Services
{
    public interface ISamlService
    {
        string GetAuthnRequest(string authnRequestId, string relayState);

        Saml20Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId);

        Saml20Assertion HandleHttpArtifactResponse(HttpRequest request);

        bool HandleLogoutResponse(Uri uri, string originalRequestId);

        string GetLogoutRequest(string logoutRequestId, string relayState, string sessionIndex);
    }
}