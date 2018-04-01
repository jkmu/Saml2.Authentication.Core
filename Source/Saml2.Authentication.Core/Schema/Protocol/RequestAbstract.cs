using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.XmlDSig;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// All SAML requests are of types that are derived from the abstract RequestAbstractType complex type.
    /// This type defines common attributes and elements that are associated with all SAML requests
    /// </summary>
    [XmlInclude(typeof (NameIDMappingRequest))]
    [XmlInclude(typeof (LogoutRequest))]
    [XmlInclude(typeof (ManageNameIDRequest))]
    [XmlInclude(typeof (ArtifactResolve))]
    [XmlInclude(typeof (AuthnRequest))]
    [XmlInclude(typeof (SubjectQueryAbstract))]
    [XmlInclude(typeof (AuthzDecisionQuery))]
    [XmlInclude(typeof (AttributeQuery))]
    [XmlInclude(typeof (AuthnQuery))]
    [XmlInclude(typeof (AssertionIDRequest))]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    public abstract class RequestAbstract
    {
        private string consentField;
        private string destinationField;
        private Extensions extensionsField;

        private string idField;

        private DateTime? issueInstantField;
        private NameID issuerField;

        private Signature signatureField;
        private string versionField;


        /// <summary>
        /// Gets or sets the issuer.
        /// Identifies the entity that generated the request message.
        /// </summary>
        /// <value>The issuer.</value>
        [XmlElement(Namespace=Saml2Constants.ASSERTION)]
        public NameID Issuer
        {
            get { return issuerField; }
            set { issuerField = value; }
        }


        /// <summary>
        /// Gets or sets the signature.
        /// An XML Signature that authenticates the requester and provides message integrity
        /// </summary>
        /// <value>The signature.</value>
        [XmlElement(Namespace="http://www.w3.org/2000/09/xmldsig#")]
        public Signature Signature
        {
            get { return signatureField; }
            set { signatureField = value; }
        }


        /// <summary>
        /// Gets or sets the extensions.
        /// This extension point contains optional protocol message extension elements that are agreed on
        /// between the communicating parties. No extension schema is required in order to make use of this
        /// extension point, and even if one is provided, the lax validation setting does not impose a requirement
        /// for the extension to be valid. SAML extension elements MUST be namespace-qualified in a non-
        /// SAML-defined namespace
        /// </summary>
        /// <value>The extensions.</value>
        public Extensions Extensions
        {
            get { return extensionsField; }
            set { extensionsField = value; }
        }


        /// <summary>
        /// An identifier for the request. It is of type xs:ID and MUST follow the requirements specified in Section
        /// 1.3.4 for identifier uniqueness. The values of the ID attribute in a request and the InResponseTo
        /// attribute in the corresponding response MUST match.
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        [XmlAttribute(DataType="ID")]
        public string ID
        {
            get { return idField; }
            set { idField = value; }
        }


        /// <summary>
        /// Gets or sets the version.
        /// The version of this request. The identifier for the version of SAML defined in this specification is "2.0".
        /// </summary>
        /// <value>The version.</value>
        [XmlAttribute]
        public string Version
        {
            get { return versionField; }
            set { versionField = value; }
        }


        /// <summary>
        /// Gets or sets the issue instant.
        /// The time instant of issue of the request.
        /// </summary>
        /// <value>The issue instant.</value>
        [XmlIgnore]
        public DateTime? IssueInstant
        {
            get { return issueInstantField; }
            set { issueInstantField = value; }
        }

        /// <summary>
        /// Gets or sets the issue instant string.
        /// </summary>
        /// <value>The issue instant string.</value>
        [XmlAttribute("IssueInstant")]
        public string IssueInstantString
        {
            get 
            {
                if (issueInstantField.HasValue)
                    return Saml2Utils.ToUTCString(issueInstantField.Value);
                else
                {
                    return null;
                }
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    issueInstantField = null;
                else
                    issueInstantField = Saml2Utils.FromUTCString(value);
            }
        }


        /// <summary>
        /// Gets or sets the destination.
        /// A URI reference indicating the address to which this request has been sent. This is useful to prevent
        /// malicious forwarding of requests to unintended recipients, a protection that is required by some
        /// protocol bindings. If it is present, the actual recipient MUST check that the URI reference identifies the
        /// location at which the message was received. If it does not, the request MUST be discarded. Some
        /// protocol bindings may require the use of this attribute (see [SAMLBind]).
        /// </summary>
        /// <value>The destination.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Destination
        {
            get { return destinationField; }
            set { destinationField = value; }
        }


        /// <summary>
        /// Gets or sets the consent.
        /// Indicates whether or not (and under what conditions) consent has been obtained from a principal in
        /// the sending of this request.
        /// </summary>
        /// <value>The consent.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Consent
        {
            get { return consentField; }
            set { consentField = value; }
        }
    }
}