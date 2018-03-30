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
            IdentityProviderConfiguration identityProviderConfiguration,
            ServiceProviderConfiguration serviceProviderConfiguration)
        {
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _saml2MessageFactory = saml2MessageFactory;
            _certificateProvider = certificateProvider;
            _samlProvider = samlProvider;
            _saml2Validator = saml2Validator;
            _identityProviderConfiguration = identityProviderConfiguration;
            _serviceProviderConfiguration = serviceProviderConfiguration;
        }

        public string GetAuthnRequest(string authnRequestId, string relayState)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var saml20AuthnRequest = _saml2MessageFactory.CreateAuthnRequest(authnRequestId);

            // check protocol binding

            return _httpRedirectBinding.BuildAuthnRequestUrl(saml20AuthnRequest,
                signingCertificate.ServiceProvider.PrivateKey,
                _identityProviderConfiguration.HashingAlgorithm, relayState);
        }

        public string GetLogoutRequest(string logoutRequestId, string relayState, string sessionIndex)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var logoutRequest = _saml2MessageFactory.CreateLogoutRequest(logoutRequestId, sessionIndex);
            return _httpRedirectBinding.BuildLogoutRequestUrl(logoutRequest,
                signingCertificate.ServiceProvider.PrivateKey, _identityProviderConfiguration.HashingAlgorithm,
                relayState);
        }

        public bool HandleLogoutResponse(Uri uri, string originalRequestId)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var logoutMessage = _httpRedirectBinding.GetLogoutResponseMessage(uri, signingCertificate.ServiceProvider.PrivateKey);
            var logoutRequest = _samlProvider.GetLogoutResponse(logoutMessage);
            if (!_saml2Validator.CheckReplayAttack(logoutRequest.InResponseTo, originalRequestId))
            {
                return false;
            }
            return logoutRequest.Status.StatusCode.Value == Saml20Constants.StatusCodes.Success;
        }

        public Saml20Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId)
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


        public Saml20Assertion HandleHttpArtifactResponse(HttpRequest request)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var artifact = _httpArtifactBinding.GetArtifact(request);
            var stream = _httpArtifactBinding.ResolveArtifact(artifact,
                _identityProviderConfiguration.ArtifactResolveEndpoint, _serviceProviderConfiguration.Id,
                signingCertificate.ServiceProvider);

            var artifactResponseElement = _samlProvider.GetArtifactResponse(stream);
            return GetValidatedAssertion(artifactResponseElement);
        }

        private Saml20Assertion GetValidatedAssertion(XmlElement samlResponseElement)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            var assertionElement = _samlProvider.GetAssertion(samlResponseElement, signingCertificate.ServiceProvider.PrivateKey);
            return _saml2Validator.GetValidatedAssertion(assertionElement,
                signingCertificate.ServiceProvider.PrivateKey,
                _identityProviderConfiguration.OmitAssertionSignatureCheck);
        }
    }
}
