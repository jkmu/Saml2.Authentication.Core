using System;
using System.Security.Cryptography;
using dk.nita.saml20;
using Microsoft.AspNetCore.Http;

namespace Saml2.Authentication.Core.Bindings
{
    public interface IHttpRedirectBinding
    {
        string BuildAuthnRequestUrl(Saml2AuthnRequest request, AsymmetricAlgorithm signingKey, string hashingAlgorithm,
            string relayState);

        string BuildLogoutRequestUrl(Saml2LogoutRequest saml2LogoutRequest, AsymmetricAlgorithm signingKey,
            string hashingAlgorithm, string relayState);

        bool IsValid(HttpRequest request);

        bool IsLogoutRequest(HttpRequest request);

        string GetLogoutResponseMessage(Uri uri, AsymmetricAlgorithm key);

        Saml2Response GetResponse(HttpRequest request);

        Saml2LogoutResponse GetLogoutReponse(Uri uri, AsymmetricAlgorithm key);

        string BuildLogoutResponseUrl(dk.nita.saml20.Saml2LogoutResponse logoutResponse, AsymmetricAlgorithm signingKey,
            string hashingAlgorithm, string relayState);

        string GetCompressedRelayState(HttpRequest request);
    }
}