using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using dk.nita.saml20.Utils;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Extensions;

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
        public Stream ResolveArtifact(string artifact, string artifactResolveEndpoint, string serviceProviderId,
            X509Certificate2 cert)
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
                doc.RemoveChild(doc.FirstChild);

            XmlSignatureUtils.SignDocument(doc, resolve.ID, cert);

            var artifactResolveString = doc.OuterXml;
            return GetResponse(artifactResolveEndpoint, artifactResolveString);
        }

        ///// <summary>
        ///// Creates an artifact and redirects the user to the IdP
        ///// </summary>
        ///// <param name="destination">The destination of the request.</param>
        ///// <param name="serviceProviderId"></param>
        ///// <param name="serviceProviderLogoutEndpointIndex"></param>
        ///// <param name="request">The authentication request.</param>
        //public void RedirectFromLogin(string destination, string serviceProviderId, short serviceProviderLogoutEndpointIndex, Saml2AuthnRequest request)
        //{
        //    var doc = request.GetXml();
        //    XmlSignatureUtils.SignDocument(doc, request.Request.ID);
        //    ArtifactRedirect(destination, serviceProviderId, serviceProviderLogoutEndpointIndex, doc);
        //}

        ///// <summary>
        ///// Creates an artifact for the LogoutRequest and redirects the user to the IdP.
        ///// </summary>
        ///// <param name="destination">The destination of the request.</param>
        ///// <param name="serviceProviderId"></param>
        ///// <param name="serviceProviderLogoutEndpointIndex"></param>
        ///// <param name="request">The logout request.</param>
        //public void RedirectFromLogout(string destination, string serviceProviderId, short serviceProviderLogoutEndpointIndex, Saml2LogoutRequest request)
        //{
        //    var doc = request.GetXml();
        //    XmlSignatureUtils.SignDocument(doc, request.Request.ID);
        //    ArtifactRedirect(destination, serviceProviderId, serviceProviderLogoutEndpointIndex, doc);
        //}

        ///// <summary>
        ///// Creates an artifact for the LogoutRequest and redirects the user to the IdP.
        ///// </summary>
        ///// <param name="destination">The destination of the request.</param>
        ///// <param name="serviceProviderId"></param>
        ///// <param name="serviceProviderLogoutEndpointIndex"></param>
        ///// <param name="request">The logout request.</param>
        ///// <param name="relayState">The query string relay state value to add to the communication</param>
        //public void RedirectFromLogout(string destination, string serviceProviderId, short serviceProviderLogoutEndpointIndex, Saml2LogoutRequest request, string relayState)
        //{
        //    var doc = request.GetXml();
        //    XmlSignatureUtils.SignDocument(doc, request.Request.ID);
        //    ArtifactRedirect(destination, serviceProviderId, serviceProviderLogoutEndpointIndex, doc, relayState);
        //}

        ///// <summary>
        ///// Creates an artifact for the LogoutResponse and redirects the user to the IdP.
        ///// </summary>
        ///// <param name="destination">The destination of the response.</param>
        ///// <param name="serviceProviderId"></param>
        ///// <param name="serviceProviderLogoutEndpointIndex"></param>
        ///// <param name="response">The logout response.</param>
        //public void RedirectFromLogout(string destination, string serviceProviderId, short serviceProviderLogoutEndpointIndex, Saml2LogoutResponse response)
        //{
        //    var doc = response.GetXml();
        //    XmlSignatureUtils.SignDocument(doc, response.Response.ID);
        //    ArtifactRedirect(destination, serviceProviderId, serviceProviderLogoutEndpointIndex, doc);
        //}

        ///// <summary>
        ///// Handles all artifact creations and redirects.
        ///// </summary>
        ///// <param name="destination">The destination.</param>
        ///// <param name="serviceProviderId"></param>
        ///// <param name="localEndpointIndex">Index of the local endpoint.</param>
        ///// <param name="signedSamlMessage">The signed saml message.</param>
        ///// <param name="relayState">The query string relay state value to add to the communication</param>
        //private void ArtifactRedirect(string destination, string serviceProviderId, Int16 localEndpointIndex, XmlDocument signedSamlMessage, string relayState)
        //{
        //    var sourceId = serviceProviderId;
        //    var sourceIdHash = ArtifactUtil.GenerateSourceIdHash(sourceId);
        //    var messageHandle = ArtifactUtil.GenerateMessageHandle();

        //    var artifact = ArtifactUtil.CreateArtifact(HttpArtifactBindingConstants.ArtifactTypeCode, localEndpointIndex, sourceIdHash, messageHandle);

        //    Context.Cache.Insert(artifact, signedSamlMessage, null, DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration);

        //    var destinationUrl = destination + "?" + HttpArtifactBindingConstants.ArtifactQueryStringName + "=" + artifact.UrlEncode();
        //    if (!string.IsNullOrEmpty(relayState))
        //    {
        //        destinationUrl += "&relayState=" + relayState;
        //    }

        //    Context.Response.Redirect(destinationUrl);
        //}

        ///// <summary>
        ///// Handles all artifact creations and redirects. Convenience wrapper which re-uses the existing relay state
        ///// </summary>
        ///// <param name="destination">The destination.</param>
        ///// <param name="serviceProviderId"></param>
        ///// <param name="localEndpointIndex">Index of the local endpoint.</param>
        ///// <param name="signedSamlMessage">The signed saml message.</param>
        //private void ArtifactRedirect(string destination, string serviceProviderId, short localEndpointIndex, XmlDocument signedSamlMessage)
        //{
        //    ArtifactRedirect(destination, serviceProviderId, localEndpointIndex, signedSamlMessage, Context.Request.Query["relayState"]);
        //}

        ///// <summary>
        ///// Handles responses to an artifact resolve message.
        ///// </summary>
        ///// <param name="artifactResolve">The artifact resolve message.</param>
        //public void RespondToArtifactResolve(ArtifactResolve artifactResolve, string serviceProviderId)
        //{
        //    var samlDoc = (XmlDocument)Context.Cache.Get(artifactResolve.Artifact);

        //    var response = new Saml2ArtifactResponse
        //    {
        //        Issuer = serviceProviderId,
        //        StatusCode = Saml2Constants.StatusCodes.Success,
        //        InResponseTo = artifactResolve.ID,
        //        SamlElement = samlDoc.DocumentElement
        //    };

        //    var responseDoc = response.GetXml();

        //    if (responseDoc.FirstChild is XmlDeclaration)
        //        responseDoc.RemoveChild(responseDoc.FirstChild);

        //    XmlSignatureUtils.SignDocument(responseDoc, response.ID);

        //    SendResponseMessage(responseDoc.OuterXml);
        //}
    }
}