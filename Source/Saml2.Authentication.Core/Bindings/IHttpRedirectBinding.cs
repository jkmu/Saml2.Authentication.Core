namespace Saml2.Authentication.Core.Bindings
{
    public interface IHttpRedirectBinding
    {
        string BuildAuthnRequestUrl(Saml2AuthnRequest request, string relayState);

        string BuildLogoutRequestUrl(Saml2LogoutRequest saml2LogoutRequest, string relayState);

        bool IsValid();

        bool IsLogoutRequest();

        string GetLogoutResponseMessage();

        Saml2Response GetResponse();

        Saml2LogoutResponse GetLogoutReponse();

        string BuildLogoutResponseUrl(Core.Saml2LogoutResponse logoutResponse, string relayState);

        string GetCompressedRelayState();
    }
}