namespace Saml2.Authentication.Core.Bindings
{
    public interface IHttpRedirectBinding
    {
        string BuildAuthnRequestUrl(string providerName, Saml2AuthnRequest request, string relayState);

        string BuildLogoutRequestUrl(string providerName, Saml2LogoutRequest saml2LogoutRequest, string relayState);

        bool IsValid();

        bool IsLogoutRequest();

        string GetLogoutResponseMessage(string providerName);

        Saml2Response GetResponse();

        Saml2LogoutResponse GetLogoutResponse(string providerName);

        string BuildLogoutResponseUrl(string providerName, Core.Saml2LogoutResponse logoutResponse, string relayState);

        string GetCompressedRelayState();
    }
}