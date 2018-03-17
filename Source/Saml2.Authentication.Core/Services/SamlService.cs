using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Protocol;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Bindings;
using Saml2.Authentication.Core.Factories;
using Saml2.Authentication.Core.Options;
using Saml2.Authentication.Core.Providers;
namespace Saml2.Authentication.Core.Services
{
    public class SamlService : ISamlService
    {
        private readonly IHttpRedirectBinding _httpRedirectBinding;
        private readonly IHttpArtifactBinding _httpArtifactBinding;
        private readonly ISaml2MessageFactory _saml2MessageFactory;
        private readonly ICertificateProvider _certificateProvider;
        private readonly ISamlProvider _samlProvider;
        private readonly IdentityProviderConfiguration _identityProviderConfiguration;
        private readonly ServiceProviderConfiguration _serviceProviderConfiguration;

        public SamlService(
            IHttpRedirectBinding httpRedirectBinding,
            IHttpArtifactBinding httpArtifactBinding,
            ISaml2MessageFactory saml2MessageFactory,
            ICertificateProvider certificateProvider,
            ISamlProvider samlProvider,
            IdentityProviderConfiguration identityProviderConfiguration,
            ServiceProviderConfiguration serviceProviderConfiguration)
        {
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _saml2MessageFactory = saml2MessageFactory;
            _certificateProvider = certificateProvider;
            _samlProvider = samlProvider;
            _identityProviderConfiguration = identityProviderConfiguration;
            _serviceProviderConfiguration = serviceProviderConfiguration;
        }

        public string GetSingleSignOnRequestUrl(string authnRequestId, string relayState)
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            if (!signingCertificate.ServiceProvider.HasPrivateKey)
            {
                throw new InvalidOperationException("Certificate does not have a private key.");
            }

            var saml20AuthnRequest = _saml2MessageFactory.CreateAuthnRequest(authnRequestId);
            var request = saml20AuthnRequest.GetXml().OuterXml;
            // check protocol binding


            var query = _httpRedirectBinding.BuildQuery(request, signingCertificate.ServiceProvider.PrivateKey,
                _identityProviderConfiguration.HashingAlgorithm, relayState);
            return $"{saml20AuthnRequest.Destination}?{query}";
        }

        public Saml20Assertion HandleHttpRedirectResponse(string base64EncodedSamlResponse, string originalSamlRequestId)
        {
            var defaultEncoding = Encoding.UTF8;
            var samlResponseDocument = _samlProvider.GetDecodedSamlResponse(base64EncodedSamlResponse, defaultEncoding);
            var samlResponseElement = samlResponseDocument.DocumentElement;
            if (!_samlProvider.CheckReplayAttack(samlResponseElement, originalSamlRequestId))
            {
                return null;
            }

            return !_samlProvider.CheckStatus(samlResponseDocument) ? null : GetValidatedAssertion(samlResponseElement);
        }


        public Saml20Assertion HandleHttpArtifactResponse(HttpRequest request)
        {
            if (!_httpArtifactBinding.IsValid(request))
            {
                return null;
            }

            var artifact = _httpArtifactBinding.GetArtifact(request);

            var signingCertificate = _certificateProvider.GetCertificate();
            var stream = _httpArtifactBinding.ResolveArtifact(artifact,
                _identityProviderConfiguration.ArtifactResolveEndpoint, _serviceProviderConfiguration.Id,
                signingCertificate.ServiceProvider);

            var parser = new HttpArtifactBindingParser(stream);
            if (!parser.IsArtifactResponse())
            {
                return null;
            }

            var status = parser.ArtifactResponse.Status;
            if (status.StatusCode.Value != Saml20Constants.StatusCodes.Success)
            {
                throw new Exception($"Illegal status: {status.StatusCode} for ArtifactResponse");
            }

            if (parser.ArtifactResponse.Any.LocalName != Response.ELEMENT_NAME)
            {
                return null;
            }

            var artifactResponseElement = parser.ArtifactResponse.Any;
            return GetValidatedAssertion(artifactResponseElement);
        }

        private Saml20Assertion GetValidatedAssertion(XmlElement samlResponseElement)
        {
            var signingCertificate = _certificateProvider.GetCertificate();

            var assertionElement = _samlProvider.GetAssertion(samlResponseElement, signingCertificate.ServiceProvider.PrivateKey);
            var assertion = new Saml20Assertion(assertionElement, null, false);
            if (!_identityProviderConfiguration.OmitAssertionSignatureCheck)
            {
                if (!assertion.CheckSignature(new List<AsymmetricAlgorithm> { signingCertificate.ServiceProvider.PrivateKey }))
                {
                    throw new Saml20Exception("Invalid signature in assertion");
                }
            }

            if (assertion.IsExpired())
            {
                throw new Saml20Exception("Assertion is expired");
            }

            return assertion;
        }
    }
}
