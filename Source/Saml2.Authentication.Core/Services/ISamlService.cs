namespace Saml2.Authentication.Core.Services
{
    public interface ISamlService
    {
        string GetSingleSignOnRequestUrl(string authnRequestId, string relayState);

        Saml20Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId);
    }
}