namespace Saml2.Authentication.Core.Providers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;

    public interface ISaml2AuthenticationProvider
    {
        Task InitiateSsoAsync(string requestId, string relayState = null);

        Task<Saml2Assertion> ReceiveHttpRedirectAuthnResponseAsync(string initialRequestId);

        Task SignInAsync(
            string signinScheme,
            Saml2Assertion assertion,
            AuthenticationProperties authenticationProperties);

        Task<Saml2Assertion> ReceiveHttpArtifactAuthnResponseAsync(string initialRequestId);

        Task InitiateSloAsync(string requestId, string relayState = null);

        Task<string> ReceiveIdpInitiatedLogoutRequest();

        Task<bool> ReceiveSpInitiatedLogoutResponse(string logoutRequestId);
    }
}