using System;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core
{
    /// <summary>
    /// Encapsulates the ArtifactResolve schema class.
    /// </summary>
    public class Saml20ArtifactResolve
    {
        private readonly ArtifactResolve _artifactResolve;

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml20ArtifactResolve"/> class.
        /// </summary>
        public Saml20ArtifactResolve()
        {
            _artifactResolve = new ArtifactResolve
            {
                Version = Saml20Constants.Version,
                ID = "id" + Guid.NewGuid().ToString("N"),
                Issuer = new NameID(),
                IssueInstant = DateTime.Now
            };
        }

        /// <summary>
        /// Gets the underlying schema instance.
        /// </summary>
        /// <value>The resolve.</value>
        public ArtifactResolve Resolve => _artifactResolve;

        /// <summary>
        /// Gets the ID of the SAML message.
        /// </summary>
        /// <value>The ID.</value>
        public string ID => _artifactResolve.ID;

        /// <summary>
        /// Gets or sets the artifact string.
        /// </summary>
        /// <value>The artifact string.</value>
        public string Artifact
        {
            get => _artifactResolve.Artifact;
            set => _artifactResolve.Artifact = value;
        }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get => _artifactResolve.Issuer.Value;
            set => _artifactResolve.Issuer.Value = value;
        }

        /// <summary>
        /// Returns the ArtifactResolve as an XML document.
        /// </summary>
        public XmlDocument GetXml()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            doc.LoadXml(Serialization.SerializeToXmlString(_artifactResolve));
            return doc;
        }
    }
}