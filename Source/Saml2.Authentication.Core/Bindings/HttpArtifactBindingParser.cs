using System;
using System.IO;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Bindings;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core.Bindings
{
    /// <summary>
    ///     Parses the response messages related to the artifact binding.
    /// </summary>
    public class HttpArtifactBindingParser : HttpSoapBindingParser
    {
        private ArtifactResolve _artifactResolve;
        private ArtifactResponse _artifactResponse;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpArtifactBindingParser" /> class.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public HttpArtifactBindingParser(Stream inputStream) : base(inputStream)
        {
        }

        /// <summary>
        ///     Gets the artifact response message.
        /// </summary>
        /// <value>The artifact response.</value>
        public ArtifactResponse ArtifactResponse
        {
            get
            {
                if (!IsArtifactResponse())
                {
                    throw new InvalidOperationException("The Saml message is not an ArtifactResponse");
                }

                LoadArtifactResponse();
                return _artifactResponse;
            }
        }

        /// <summary>
        ///     Gets the artifact resolve message.
        /// </summary>
        /// <value>The artifact resolve.</value>
        public ArtifactResolve ArtifactResolve
        {
            get
            {
                if (!IsArtifactResolve())
                {
                    throw new InvalidOperationException("The Saml message is not an ArtifactResolve");
                }

                LoadArtifactResolve();
                return _artifactResolve;
            }
        }

        /// <summary>
        ///     Gets the issuer of the current message.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get
            {
                if (IsArtifactResolve())
                {
                    return ArtifactResolve.Issuer.Value;
                }

                if (IsArtifactResponse())
                {
                    return ArtifactResponse.Issuer.Value;
                }

                if (IsLogoutReqest())
                {
                    return LogoutRequest.Issuer.Value;
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///     Determines whether the current message is an artifact resolve
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the current message is an artifact resolve; otherwise, <c>false</c>.
        /// </returns>
        public bool IsArtifactResolve()
        {
            return SamlMessage.LocalName == HttpArtifactBindingConstants.ArtifactResolve;
        }

        /// <summary>
        ///     Determines whether the current message is an artifact response.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the current message is an artifact response; otherwise, <c>false</c>.
        /// </returns>
        public bool IsArtifactResponse()
        {
            return SamlMessage.LocalName == HttpArtifactBindingConstants.ArtifactResponse;
        }

        /// <summary>
        ///     Loads the current message as an artifact resolve.
        /// </summary>
        private void LoadArtifactResolve()
        {
            if (_artifactResolve == null)
            {
                _artifactResolve = Serialization.Deserialize<ArtifactResolve>(new XmlNodeReader(SamlMessage));
            }
        }

        /// <summary>
        ///     Loads the current message as an artifact response.
        /// </summary>
        private void LoadArtifactResponse()
        {
            if (_artifactResponse == null)
            {
                _artifactResponse = Serialization.Deserialize<ArtifactResponse>(new XmlNodeReader(SamlMessage));
                _artifactResponse.Any = (XmlElement)SamlMessage.GetElementsByTagName(Response.ELEMENT_NAME, Saml2Constants.PROTOCOL)[0];
            }

        }
    }
}