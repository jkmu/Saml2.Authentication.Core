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
        private readonly ISamlMessageFactory _samlMessageFactory;
        private readonly ICertificateProvider _certificateProvider;
        private readonly ISamlProvider _samlProvider;
        private readonly IdentityProviderOptions _identityProviderOptions;
        private readonly ServiceProviderOptions _serviceProviderOptions;

        public SamlService(
            IHttpRedirectBinding httpRedirectBinding,
            IHttpArtifactBinding httpArtifactBinding,
            ISamlMessageFactory samlMessageFactory,
            ICertificateProvider certificateProvider,
            ISamlProvider samlProvider,
            IdentityProviderOptions identityProviderOptions,
            ServiceProviderOptions serviceProviderOptions)
        {
            _httpRedirectBinding = httpRedirectBinding;
            _httpArtifactBinding = httpArtifactBinding;
            _samlMessageFactory = samlMessageFactory;
            _certificateProvider = certificateProvider;
            _samlProvider = samlProvider;
            _identityProviderOptions = identityProviderOptions;
            _serviceProviderOptions = serviceProviderOptions;
        }

        public string GetSingleSignOnRequestUrl()
        {
            var signingCertificate = _certificateProvider.GetCertificate();
            if (!signingCertificate.ServiceProvider.HasPrivateKey)
            {
                throw new InvalidOperationException("Certificate does not have a private key.");
            }

            var saml20AuthnRequest = _samlMessageFactory.CreateAuthnRequest();
            var request = saml20AuthnRequest.GetXml().OuterXml;
            // check protocol binding


            var query = _httpRedirectBinding.BuildQuery(request, signingCertificate.ServiceProvider.PrivateKey, _identityProviderOptions.HashingAlgorithm);
            return $"{saml20AuthnRequest.Destination}?{query}";
        }

        public Saml20Assertion HandleHttpRedirectResponse(HttpRequest request, string originalSamlRequestId)
        {
            if (!_httpRedirectBinding.IsValid(request))
            {
                return null;
            }

            var defaultEncoding = Encoding.UTF8;
            var base64EncodedSamlResponse = _httpRedirectBinding.GetSamlResponse(request);

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
                _identityProviderOptions.ArtifactResolveEndpoint, _serviceProviderOptions.Id,
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
            if (!_identityProviderOptions.OmitAssertionSignatureCheck)
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
