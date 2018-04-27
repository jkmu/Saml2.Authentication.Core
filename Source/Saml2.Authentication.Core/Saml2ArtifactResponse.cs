using System;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core
{
    /// <summary>
    ///     Encapsulates the ArtificatResponse schema class
    /// </summary>
    public class Saml2ArtifactResponse
    {
        private readonly ArtifactResponse _artifactResponse;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2ArtifactResponse" /> class.
        /// </summary>
        public Saml2ArtifactResponse()
        {
            _artifactResponse = new ArtifactResponse
            {
                Version = Saml2Constants.Version,
                ID = "id" + Guid.NewGuid().ToString("N"),
                Issuer = new NameID(),
                IssueInstant = DateTime.Now,
                Status = new Status {StatusCode = new StatusCode()}
            };
        }

        /// <summary>
        ///     Gets or sets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get => _artifactResponse.Issuer.Value;
            set => _artifactResponse.Issuer.Value = value;
        }

        /// <summary>
        ///     Gets or sets InResponseTo.
        /// </summary>
        /// <value>The in response to.</value>
        public string InResponseTo
        {
            get => _artifactResponse.InResponseTo;
            set => _artifactResponse.InResponseTo = value;
        }

        /// <summary>
        ///     Gets or sets the SAML element.
        /// </summary>
        /// <value>The SAML element.</value>
        public XmlElement SamlElement
        {
            get => _artifactResponse.Any;
            set => _artifactResponse.Any = value;
        }

        /// <summary>
        ///     Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public string ID => _artifactResponse.ID;

        /// <summary>
        ///     Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public string StatusCode
        {
            get => _artifactResponse.Status.StatusCode.Value;
            set => _artifactResponse.Status.StatusCode.Value = value;
        }

        /// <summary>
        ///     Returns the ArtifactResponse as an XML document.
        /// </summary>
        public XmlDocument GetXml()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            doc.LoadXml(Serialization.SerializeToXmlString(_artifactResponse));
            return doc;
        }
    }
}