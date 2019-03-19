namespace Saml2.Authentication.Core.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;

    public interface ISamlService
    {
        Task InitiateSsoAsync(string providerName, string requestId, string relayState = null);

        Task<Saml2Assertion> ReceiveHttpRedirectAuthnResponseAsync(string initialRequestId);

        Task SignInAsync(
            string signinScheme,
            Saml2Assertion assertion,
            AuthenticationProperties authenticationProperties);

        Task<Saml2Assertion> ReceiveHttpArtifactAuthnResponseAsync(string providerName, string initialRequestId);

        Task InitiateSloAsync(string providerName, string requestId, string relayState = null);

        Task<string> ReceiveIdpInitiatedLogoutRequest(string providerName);

        Task<bool> ReceiveSpInitiatedLogoutResponse(string providerName, string logoutRequestId);
    }
}