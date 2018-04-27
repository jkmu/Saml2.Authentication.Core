using System;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core
{
    /// <summary>
    ///     Encapsulates the ArtifactResolve schema class.
    /// </summary>
    public class Saml2ArtifactResolve
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Saml2ArtifactResolve" /> class.
        /// </summary>
        public Saml2ArtifactResolve()
        {
            Resolve = new ArtifactResolve
            {
                Version = Saml2Constants.Version,
                ID = "id" + Guid.NewGuid().ToString("N"),
                Issuer = new NameID(),
                IssueInstant = DateTime.Now
            };
        }

        /// <summary>
        ///     Gets the underlying schema instance.
        /// </summary>
        /// <value>The resolve.</value>
        public ArtifactResolve Resolve { get; }

        /// <summary>
        ///     Gets the ID of the SAML message.
        /// </summary>
        /// <value>The ID.</value>
        public string ID => Resolve.ID;

        /// <summary>
        ///     Gets or sets the artifact string.
        /// </summary>
        /// <value>The artifact string.</value>
        public string Artifact
        {
            get => Resolve.Artifact;
            set => Resolve.Artifact = value;
        }

        /// <summary>
        ///     Gets or sets the issuer.
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer
        {
            get => Resolve.Issuer.Value;
            set => Resolve.Issuer.Value = value;
        }

        /// <summary>
        ///     Returns the ArtifactResolve as an XML document.
        /// </summary>
        public XmlDocument GetXml()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            doc.LoadXml(Serialization.SerializeToXmlString(Resolve));
            return doc;
        }
    }
}