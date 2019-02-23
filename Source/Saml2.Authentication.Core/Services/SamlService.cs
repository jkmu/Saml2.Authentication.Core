using System;
using System.Text;
using System.Xml;
using dk.nita.saml20;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Bindings;
using Saml2.Authentication.Core.Factories;
using Saml2.Authentication.Core.Options;
using Saml2.Authentication.Core.Providers;
using Saml2.Authentication.Core.Validation;
using Saml2LogoutResponse = Saml2.Authentication.Core.Bindings.Saml2LogoutResponse;

namespace Saml2.Authentication.Core.Services
{
    internal class SamlService : ISamlService
    {
        private readonly ICertificateProvider _certificateProvider;
        private readonly IHttpArtifactBinding _httpArtifactBinding;
        private readonly IHttpRedirectBinding _httpRedirectBinding;
        private readonly IdentityProviderConfiguration _identityProviderConfiguration;
        private readonly Saml2Configuration _saml2Configuration;
        private readonly ISaml2MessageFactory _saml2MessageFactory;
        private readonly ISaml2Validator _saml2Validator;
        private readonly ISamlProvider _samlProvider;
        private readonly ServiceProviderConfiguration _serviceProviderConfiguration;

        public SamlService(
            IHttpRedirectBinding httpRedirectBinding,
            IHttpArtifactBinding httpArtifactBinding,
            ISaml2MessageFactory saml2MessageFactory,
            ICertificateProvider certificateProvider,
            ISamlProvider samlProvider,
            ISaml2Validator saml2Validator,
            Saml2Configuration saml2Configuration)
        {
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _saml2MessageFactory = saml2MessageFactory;
            _certificateProvider = certificateProvider;
            _samlProvider = samlProvider;
            _saml2Validator = saml2Validator;
            _saml2Configuration = saml2Configuration;
            _identityProviderConfiguration = saml2Configuration.IdentityProviderConfiguration;
            _serviceProviderConfiguration = saml2Configuration.ServiceProviderConfiguration;
        }

        public string GetAuthnRequest(string authnRequestId, string relayState, string assertionConsumerServiceUrl)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var saml20AuthnRequest =
                _saml2MessageFactory.CreateAuthnRequest(authnRequestId, assertionConsumerServiceUrl);
            // check protocol binding if supporting more than HTTP-REDIRECT
            return _httpRedirectBinding.BuildAuthnRequestUrl(saml20AuthnRequest,
                signingCertificate.ServiceProvider.PrivateKey,
                _identityProviderConfiguration.HashingAlgorithm, relayState);
        }

        public string GetLogoutRequest(string logoutRequestId, string sessionIndex, string subject, string relayState)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var logoutRequest = _saml2MessageFactory.CreateLogoutRequest(logoutRequestId, sessionIndex, subject);
            return _httpRedirectBinding.BuildLogoutRequestUrl(logoutRequest,
                signingCertificate.ServiceProvider.PrivateKey, _identityProviderConfiguration.HashingAlgorithm,
                relayState);
        }

        public bool IsLogoutResponseValid(Uri uri, string originalRequestId)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var logoutMessage =
                _httpRedirectBinding.GetLogoutResponseMessage(uri, signingCertificate.IdentityProvider.PublicKey.Key);
            var logoutRequest = _samlProvider.GetLogoutResponse(logoutMessage);

            _saml2Validator.CheckReplayAttack(logoutRequest.InResponseTo, originalRequestId);

            return logoutRequest.Status.StatusCode.Value == Saml2Constants.StatusCodes.Success;
        }

        public Saml2LogoutResponse GetLogoutReponse(Uri uri)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var logoutResponse =
                _httpRedirectBinding.GetLogoutReponse(uri, signingCertificate.IdentityProvider.PublicKey.Key);
            if (!_saml2Validator.ValidateLogoutRequestIssuer(logoutResponse.OriginalLogoutRequest.Issuer.Value,
                _identityProviderConfiguration.EntityId))
            {
                logoutResponse.StatusCode = Saml2Constants.StatusCodes.RequestDenied;
            }

            return logoutResponse;
        }

        public string GetLogoutResponseUrl(Saml2LogoutResponse logoutResponse, string relayState)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var response = _saml2MessageFactory.CreateLogoutResponse(logoutResponse.StatusCode,
                logoutResponse.OriginalLogoutRequest.ID);
            return _httpRedirectBinding.BuildLogoutResponseUrl(response,
                signingCertificate.ServiceProvider.PrivateKey, _identityProviderConfiguration.HashingAlgorithm,
                relayState);
        }

        public Saml2Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId)
        {
            var samlResponseDocument = _samlProvider.GetDecodedSamlResponse(base64EncodedSamlResponse, Encoding.UTF8);
            var samlResponseElement = samlResponseDocument.DocumentElement;

            _saml2Validator.CheckReplayAttack(samlResponseElement, originalSamlRequestId);

            return !_saml2Validator.CheckStatus(samlResponseDocument)
                ? null
                : GetValidatedAssertion(samlResponseElement);
        }


        public Saml2Assertion HandleHttpArtifactResponse(HttpRequest request, string originalSamlRequestId)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var artifact = _httpArtifactBinding.GetArtifact(request);
            var stream = _httpArtifactBinding.ResolveArtifact(artifact,
                _identityProviderConfiguration.ArtifactResolveService, _serviceProviderConfiguration.EntityId,
                signingCertificate.ServiceProvider);

            var artifactResponseElement = _samlProvider.GetArtifactResponse(stream);
            _saml2Validator.CheckReplayAttack(artifactResponseElement, originalSamlRequestId);
            return GetValidatedAssertion(artifactResponseElement);
        }

        private Saml2Assertion GetValidatedAssertion(XmlElement samlResponseElement)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var assertionElement =
                _samlProvider.GetAssertion(samlResponseElement, signingCertificate.ServiceProvider.PrivateKey);
            return _saml2Validator.GetValidatedAssertion(assertionElement,
                signingCertificate.IdentityProvider.PublicKey.Key, _serviceProviderConfiguration.EntityId,
                _saml2Configuration.OmitAssertionSignatureCheck);
        }
    }
}