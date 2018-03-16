using System;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XmlDSig;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;AffiliationDescriptor&gt; element is an alternative to the sequence of role descriptors
    /// described in Section 2.4 that is used when an &lt;EntityDescriptor&gt; describes an affiliation of SAML
    /// entities (typically service providers) rather than a single entity. The &lt;AffiliationDescriptor&gt;
    /// element provides a summary of the individual entities that make up the affiliation along with general
    /// information about the affiliation itself.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class AffiliationDescriptor
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "AffiliationDescriptor";

        private string[] affiliateMemberField;

        private string affiliationOwnerIDField;
        private XmlAttribute[] anyAttrField;

        private string cacheDurationField;
        private ExtensionsType1 extensionsField;

        private string idField;
        private KeyDescriptor[] keyDescriptorField;
        private Signature signatureField;
        private DateTime? validUntilField;


        /// <summary>
        /// Gets or sets the signature.
        /// An XML signature that authenticates the containing element and its contents
        /// </summary>
        /// <value>The signature.</value>
        [XmlElement(Namespace=Saml20Constants.XMLDSIG)]
        public Signature Signature
        {
            get { return signatureField; }
            set { signatureField = value; }
        }


        /// <summary>
        /// Gets or sets the extensions.
        /// This contains optional metadata extensions that are agreed upon between a metadata publisher
        /// and consumer. Extension elements MUST be namespace-qualified by a non-SAML-defined
        /// namespace.
        /// </summary>
        /// <value>The extensions.</value>
        public ExtensionsType1 Extensions
        {
            get { return extensionsField; }
            set { extensionsField = value; }
        }


        /// <summary>
        /// Gets or sets the affiliate member.
        /// One or more elements enumerating the members of the affiliation by specifying each member's
        /// unique identifier.
        /// </summary>
        /// <value>The affiliate member.</value>
        [XmlElement("AffiliateMember", DataType="anyURI")]
        public string[] AffiliateMember
        {
            get { return affiliateMemberField; }
            set { affiliateMemberField = value; }
        }


        /// <summary>
        /// Gets or sets the key descriptor.
        /// Optional sequence of elements that provides information about the cryptographic keys that the
        /// affiliation uses as a whole, as distinct from keys used by individual members of the affiliation,
        /// which are published in the metadata for those entities.
        /// </summary>
        /// <value>The key descriptor.</value>
        [XmlElement("KeyDescriptor")]
        public KeyDescriptor[] KeyDescriptor
        {
            get { return keyDescriptorField; }
            set { keyDescriptorField = value; }
        }


        /// <summary>
        /// Gets or sets the affiliation owner ID.
        /// Specifies the unique identifier of the entity responsible for the affiliation. The owner is NOT
        /// presumed to be a member of the affiliation; if it is a member, its identifier MUST also appear in an
        /// &lt;AffiliateMember&gt; element.
        /// </summary>
        /// <value>The affiliation owner ID.</value>
        [XmlAttribute(DataType="anyURI")]
        public string affiliationOwnerID
        {
            get { return affiliationOwnerIDField; }
            set { affiliationOwnerIDField = value; }
        }


        /// <summary>
        /// Gets or sets the valid until.
        /// Optional attribute indicates the expiration time of the metadata contained in the element and any
        /// contained elements.
        /// </summary>
        /// <value>The valid until.</value>
        [XmlIgnore]
        public DateTime? validUntil
        {
            get { return validUntilField; }
            set { validUntilField = value; }
        }


        /// <summary>
        /// Gets or sets the valid until string.
        /// </summary>
        /// <value>The valid until string.</value>
        [XmlAttribute("validUntil")]
        public string validUntilString
        {
            get
            {
                if (validUntilField.HasValue)
                    return Saml20Utils.ToUTCString(validUntilField.Value);
                else
                    return null;
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    validUntilField = null;
                else
                    validUntilField = Saml20Utils.FromUTCString(value);

            }
        }

        /// <summary>
        /// Gets or sets the cache duration.
        /// Optional attribute indicates the maximum length of time a consumer should cache the metadata
        /// contained in the element and any contained elements.
        /// </summary>
        /// <value>The cache duration.</value>
        [XmlAttribute(DataType="duration")]
        public string cacheDuration
        {
            get { return cacheDurationField; }
            set { cacheDurationField = value; }
        }


        /// <summary>
        /// Gets or sets the ID.
        /// A document-unique identifier for the element, typically used as a reference point when signing.
        /// </summary>
        /// <value>The ID.</value>
        [XmlAttribute(DataType="ID")]
        public string ID
        {
            get { return idField; }
            set { idField = value; }
        }


        /// <summary>
        /// Gets or sets any attr.
        /// </summary>
        /// <value>Any attr.</value>
        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr
        {
            get { return anyAttrField; }
            set { anyAttrField = value; }
        }
    }
}