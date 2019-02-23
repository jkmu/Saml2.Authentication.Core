using System;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Bindings;

namespace Saml2.Authentication.Core.Services
{
    public interface ISamlService
    {
        string GetAuthnRequest(string authnRequestId, string relayState, string assertionConsumerServiceUrl);

        Saml2Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId);

        Saml2Assertion HandleHttpArtifactResponse(HttpRequest request, string originalSamlRequestId);

        bool IsLogoutResponseValid(Uri uri, string originalRequestId);

        string GetLogoutRequest(string logoutRequestId, string sessionIndex, string subject, string relayState);

        Saml2LogoutResponse GetLogoutReponse(Uri uri);

        string GetLogoutResponseUrl(Saml2LogoutResponse logoutResponse, string relayState);
    }
}