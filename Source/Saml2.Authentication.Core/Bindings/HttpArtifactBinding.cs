using dk.nita.saml20.Utils;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Extensions;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace Saml2.Authentication.Core.Bindings
{
    /// <summary>
    ///     Implementation of the artifact over HTTP SOAP binding.
    /// </summary>
    internal class HttpArtifactBinding : HttpSoapBinding, IHttpArtifactBinding
    {
        public bool IsValid(HttpRequest request)
        {
            return request?.Method == HttpMethods.Get &&
                   !string.IsNullOrEmpty(request?.Query["SAMLart"]);
        }

        public string GetArtifact(HttpRequest request)
        {
            return request?.Query["SAMLart"];
        }

        public string GetRelayState(HttpRequest request)
        {
            var encodedRelayState = request?.Query["RelayState"].ToString();
            return encodedRelayState.DeflateDecompress();
        }

        /// <summary>
        ///     Resolves an artifact.
        /// </summary>
        /// <returns>A stream containing the artifact response from the IdP</returns>
        public Stream ResolveArtifact(string artifact, string artifactResolveEndpoint, string serviceProviderId, X509Certificate2 cert)
        {
            if (artifactResolveEndpoint == null)
                throw new InvalidOperationException("Received artifact from unknown IDP.");

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

            XmlSignatureUtils.SignDocument(doc, resolve.ID, cert);

            var artifactResolveString = doc.OuterXml;
            return GetResponse(artifactResolveEndpoint, artifactResolveString);
        }
    }
}