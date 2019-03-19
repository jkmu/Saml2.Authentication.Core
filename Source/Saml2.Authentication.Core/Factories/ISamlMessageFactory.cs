namespace Saml2.Authentication.Core.Factories
{
    public interface ISamlMessageFactory
    {
        Saml2AuthnRequest CreateAuthnRequest(string providerName, string authnRequestId, string assertionConsumerServiceUrl);

        Saml2LogoutRequest CreateLogoutRequest(string providerName, string logoutRequestId, string sessionIndex, string subject);

        Saml2LogoutResponse CreateLogoutResponse(string providerName, string statusCode, string inResponseTo);
    }
}