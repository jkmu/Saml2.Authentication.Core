using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;ArtifactResolve&gt; message is used to request that a SAML protocol message be returned in an
    /// &lt;ArtifactResponse&gt; message by specifying an artifact that represents the SAML protocol message.
    /// The original transmission of the artifact is governed by the specific protocol binding that is being used; see
    /// [SAMLBind] for more information on the use of artifacts in bindings
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class ArtifactResolve : RequestAbstract
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "ArtifactResolve";

        private string artifactField;

        /// <summary>
        /// Gets or sets the artifact.
        /// The artifact value that the requester received and now wishes to translate into the protocol message it
        /// represents. See [SAMLBind] for specific artifact format information.
        /// </summary>
        /// <value>The artifact.</value>
        public string Artifact
        {
            get { return artifactField; }
            set { artifactField = value; }
        }
    }
}