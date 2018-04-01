using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.XmlDSig;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// All SAML responses are of types that are derived from the StatusResponseType complex type. This type
    /// defines common attributes and elements that are associated with all SAML responses
    /// </summary>
    [XmlInclude(typeof (NameIDMappingResponse))]
    [XmlInclude(typeof (ArtifactResponse))]
    [XmlInclude(typeof (LogoutResponse))]
    [XmlInclude(typeof (Response))]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.PROTOCOL, IsNullable = false)]
    public class StatusResponse
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "ManageNameIDResponse";

        private string consentField;
        private string destinationField;
        private Extensions extensionsField;

        private string idField;

        private string inResponseToField;

        private DateTime? issueInstantField;
        private NameID issuerField;

        private Signature signatureField;
        private Status statusField;
        private string versionField;


        /// <summary>
        /// Gets or sets the issuer.
        /// Identifies the entity that generated the response message.
        /// </summary>
        /// <value>The issuer.</value>
        [XmlElement(Namespace=Saml2Constants.ASSERTION, Form = XmlSchemaForm.Qualified)]
        public NameID Issuer
        {
            get { return issuerField; }
            set { issuerField = value; }
        }


        /// <summary>
        /// Gets or sets the signature.
        /// An XML Signature that authenticates the responder and provides message integrity
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
        /// between the communicating parties. . No extension schema is required in order to make use of this
        /// extension point, and even if one is provided, the lax validation setting does not impose a requirement
        /// for the extension to be valid. SAML extension elements MUST be namespace-qualified in a non-
        /// SAML-defined namespace.
        /// </summary>
        /// <value>The extensions.</value>
        public Extensions Extensions
        {
            get { return extensionsField; }
            set { extensionsField = value; }
        }


        /// <summary>
        /// Gets or sets the status.
        /// A code representing the status of the corresponding request
        /// </summary>
        /// <value>The status.</value>
        public Status Status
        {
            get { return statusField; }
            set { statusField = value; }
        }


        /// <summary>
        /// Gets or sets the ID.
        /// An identifier for the response. It is of type xs:ID, and MUST follow the requirements specified in
        /// Section 1.3.4 for identifier uniqueness.
        /// </summary>
        /// <value>The ID.</value>
        [XmlAttribute(DataType="ID")]
        public string ID
        {
            get { return idField; }
            set { idField = value; }
        }


        /// <summary>
        /// Gets or sets the in response to.
        /// A reference to the identifier of the request to which the response corresponds, if any. If the response
        /// is not generated in response to a request, or if the ID attribute value of a request cannot be
        /// determined (for example, the request is malformed), then this attribute MUST NOT be present.
        /// Otherwise, it MUST be present and its value MUST match the value of the corresponding request's
        /// ID attribute.
        /// </summary>
        /// <value>The in response to.</value>
        [XmlAttribute(DataType="NCName")]
        public string InResponseTo
        {
            get { return inResponseToField; }
            set { inResponseToField = value; }
        }


        /// <summary>
        /// Gets or sets the version.
        /// The version of this response. The identifier for the version of SAML defined in this specification is "2.0".
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
        /// The time instant of issue of the response.
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
                    return null;
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
        /// A URI reference indicating the address to which this response has been sent. This is useful to prevent
        /// malicious forwarding of responses to unintended recipients, a protection that is required by some
        /// protocol bindings. If it is present, the actual recipient MUST check that the URI reference identifies the
        /// location at which the message was received. If it does not, the response MUST be discarded. Some
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
        /// the sending of this response. See Section 8.4 for some URI references that MAY be used as the value
        /// of the Consent attribute and their associated descriptions. If no Consent value is provided, the
        /// identifier urn:oasis:names:tc:SAML:2.0:consent:unspecified (see Section 8.4.1) is in
        /// effect.
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