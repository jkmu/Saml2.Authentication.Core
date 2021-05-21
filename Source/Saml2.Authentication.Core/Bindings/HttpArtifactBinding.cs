namespace Saml2.Authentication.Core.Bindings
{
    using System;
    using System.IO;
    using System.Xml;
    using dk.nita.saml20.Utils;
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using Providers;

    /// <summary>
    ///     Implementation of the artifact over HTTP SOAP binding.
    /// </summary>
    internal class HttpArtifactBinding : HttpSoapBinding, IHttpArtifactBinding
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IConfigurationProvider _configurationProvider;

        public HttpArtifactBinding(
            IHttpContextAccessor httpContextAccessor,
            IConfigurationProvider configurationProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _configurationProvider = configurationProvider;
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
        /// <param name="providerName"></param>
        /// <returns>A stream containing the artifact response from the IdP</returns>
        public Stream ResolveArtifact(string providerName)
        {
            var artifactResolveEndpoint = _configurationProvider.GetIdentityProviderConfiguration(providerName).ArtifactResolveService;
            if (artifactResolveEndpoint == null)
            {
                throw new InvalidOperationException("Received artifact from unknown IDP.");
            }

            var serviceProviderId = _configurationProvider.ServiceProviderConfiguration.EntityId;
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

            var cert = _configurationProvider.ServiceProviderSigningCertificate();
            XmlSignatureUtils.SignDocument(doc, resolve.ID, cert);

            var artifactResolveString = doc.OuterXml;
            return GetResponse(artifactResolveEndpoint, artifactResolveString);
        }

        private string GetArtifact() => Request?.Query["SAMLart"];
    }
}