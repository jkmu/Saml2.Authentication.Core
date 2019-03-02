namespace Saml2.Authentication.Core.Bindings
{
    using System;
    using System.IO;
    using System.Xml;

    using dk.nita.saml20.Utils;

    using Extensions;

    using Microsoft.AspNetCore.Http;

    using Options;

    using Providers;

    /// <summary>
    ///     Implementation of the artifact over HTTP SOAP binding.
    /// </summary>
    internal class HttpArtifactBinding : HttpSoapBinding, IHttpArtifactBinding
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ICertificateProvider _certificateProvider;

        private readonly ServiceProviderConfiguration _serviceProviderConfiguration;

        private readonly IdentityProviderConfiguration _identityProviderConfiguration;

        public HttpArtifactBinding(
            IHttpContextAccessor httpContextAccessor,
            ICertificateProvider certificateProvider,
            Saml2Configuration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _certificateProvider = certificateProvider;
            _identityProviderConfiguration = configuration.IdentityProviderConfiguration;
            _serviceProviderConfiguration = configuration.ServiceProviderConfiguration;
        }

        private HttpRequest Request => _httpContextAccessor.HttpContext.Request;

        public bool IsValid()
        {
            return Request?.Method == HttpMethods.Get &&
                   !string.IsNullOrEmpty(Request?.Query["SAMLart"]);
        }

        public string GetRelayState()
        {
            var encodedRelayState = Request?.Query["RelayState"].ToString();
            return encodedRelayState.DeflateDecompress();
        }

        /// <summary>
        ///     Resolves an artifact.
        /// </summary>
        /// <returns>A stream containing the artifact response from the IdP</returns>
        public Stream ResolveArtifact()
        {
            var artifactResolveEndpoint = _identityProviderConfiguration.ArtifactResolveService;
            if (artifactResolveEndpoint == null)
            {
                throw new InvalidOperationException("Received artifact from unknown IDP.");
            }

            var serviceProviderId = _serviceProviderConfiguration.EntityId;
            var artifact = GetArtifact();
            var resolve = new Saml2ArtifactResolve
            {
                Issuer = serviceProviderId,
                Artifact = artifact
            };

            var doc = resolve.GetXml();
            if (doc.FirstChild is XmlDeclaration)
            {
                doc.RemoveChild(doc.FirstChild);
            }

            var signingCertificate = _certificateProvider.GetCertificate();

            var cert = signingCertificate.ServiceProvider;
            XmlSignatureUtils.SignDocument(doc, resolve.ID, cert);

            var artifactResolveString = doc.OuterXml;
            return GetResponse(artifactResolveEndpoint, artifactResolveString);
        }

        private string GetArtifact() => Request?.Query["SAMLart"];
    }
}