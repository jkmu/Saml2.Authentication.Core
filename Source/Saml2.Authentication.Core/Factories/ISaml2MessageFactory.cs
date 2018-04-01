using dk.nita.saml20;

namespace Saml2.Authentication.Core.Factories
{
    internal interface ISaml2MessageFactory
    {
        Saml20AuthnRequest CreateAuthnRequest(string authnRequestId, string assertionConsumerServiceUrl);

        Saml20LogoutRequest CreateLogoutRequest(string logoutRequestId, string sessionIndex, string subject);
    }
}