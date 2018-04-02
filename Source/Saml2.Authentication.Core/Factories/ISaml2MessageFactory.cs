using dk.nita.saml20;

namespace Saml2.Authentication.Core.Factories
{
    public interface ISaml2MessageFactory
    {
        Saml2AuthnRequest CreateAuthnRequest(string authnRequestId, string assertionConsumerServiceUrl);

        Saml2LogoutRequest CreateLogoutRequest(string logoutRequestId, string sessionIndex, string subject);
        Saml2LogoutResponse CreateLogoutResponse(string statusCode, string inResponseTo);
    }
}