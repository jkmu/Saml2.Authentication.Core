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

namespace Saml2.Authentication.Core.Services
{
    public class SamlService : ISamlService
    {
        private readonly IHttpRedirectBinding _httpRedirectBinding;
        private readonly IHttpArtifactBinding _httpArtifactBinding;
        private readonly ISaml2MessageFactory _saml2MessageFactory;
        private readonly ICertificateProvider _certificateProvider;
        private readonly ISamlProvider _samlProvider;
        private readonly ISaml2Validator _saml2Validator;
        private readonly IdentityProviderConfiguration _identityProviderConfiguration;
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
            _identityProviderConfiguration = saml2Configuration.IdentityProviderConfiguration;
            _serviceProviderConfiguration = saml2Configuration.ServiceProviderConfiguration;
        }

        public string GetAuthnRequest(string authnRequestId, string relayState, string assertionConsumerServiceUrl)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var saml20AuthnRequest = _saml2MessageFactory.CreateAuthnRequest(authnRequestId, assertionConsumerServiceUrl);

            // check protocol binding

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

        public bool HandleLogoutResponse(Uri uri, string originalRequestId)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var logoutMessage = _httpRedirectBinding.GetLogoutResponseMessage(uri, signingCertificate.IdentityProvider.PublicKey.Key);
            var logoutRequest = _samlProvider.GetLogoutResponse(logoutMessage);
            if (!_saml2Validator.CheckReplayAttack(logoutRequest.InResponseTo, originalRequestId))
            {
                return false;
            }
            return logoutRequest.Status.StatusCode.Value == Saml2Constants.StatusCodes.Success;
        }

        public Saml2Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId)
        {
            var defaultEncoding = Encoding.UTF8;
            var samlResponseDocument = _samlProvider.GetDecodedSamlResponse(base64EncodedSamlResponse, defaultEncoding);
            var samlResponseElement = samlResponseDocument.DocumentElement;
            if (!_saml2Validator.CheckReplayAttack(samlResponseElement, originalSamlRequestId))
            {
                return null;
            }

            return !_saml2Validator.CheckStatus(samlResponseDocument) ? null : GetValidatedAssertion(samlResponseElement);
        }


        public Saml2Assertion HandleHttpArtifactResponse(HttpRequest request)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var artifact = _httpArtifactBinding.GetArtifact(request);
            var stream = _httpArtifactBinding.ResolveArtifact(artifact,
                _identityProviderConfiguration.ArtifactResolveService, _serviceProviderConfiguration.Id,
                signingCertificate.ServiceProvider);

            var artifactResponseElement = _samlProvider.GetArtifactResponse(stream);
            return GetValidatedAssertion(artifactResponseElement);
        }

        private Saml2Assertion GetValidatedAssertion(XmlElement samlResponseElement)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var assertionElement = _samlProvider.GetAssertion(samlResponseElement, signingCertificate.ServiceProvider.PrivateKey);
            return _saml2Validator.GetValidatedAssertion(assertionElement,
                signingCertificate.IdentityProvider.PublicKey.Key, _serviceProviderConfiguration.Id,
                _identityProviderConfiguration.OmitAssertionSignatureCheck);
        }
    }
}
