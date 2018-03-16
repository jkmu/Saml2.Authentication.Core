using System;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XmlDSig;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;EntityDescriptor&gt; element specifies metadata for a single SAML entity. A single entity may act
    /// in many different roles in the support of multiple profiles. This specification directly supports the following
    /// concrete roles as well as the abstract &lt;RoleDescriptor&gt; element for extensibility (see subsequent
    /// sections for more details):
    /// * SSO Identity Provider
    /// * SSO Service Provider
    /// * Authentication Authority
    /// * Attribute Authority
    /// * Policy Decision Point
    /// * Affiliation
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class EntityDescriptor {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EntityDescriptor";
        
        private Signature signatureField;
        
        private ExtensionsType1 extensionsField;
        
        private object[] itemsField;
        
        private Organization organizationField;
        
        private Contact[] contactPersonField;
        
        private AdditionalMetadataLocation[] additionalMetadataLocationField;
        
        private string entityIDField;
        
        private DateTime? validUntilField;
        
        private string cacheDurationField;
        
        private string idField;
        
        private XmlAttribute[] anyAttrField;


        /// <summary>
        /// Gets or sets the signature.
        /// An XML signature that authenticates the containing element and its contents
        /// </summary>
        /// <value>The signature.</value>
        [XmlElementAttribute(Namespace=Saml20Constants.XMLDSIG)]
        public Signature Signature {
            get {
                return signatureField;
            }
            set {
                signatureField = value;
            }
        }


        /// <summary>
        /// Gets or sets the extensions.
        /// This contains optional metadata extensions that are agreed upon between a metadata publisher
        /// and consumer. Extension elements MUST be namespace-qualified by a non-SAML-defined
        /// namespace.
        /// </summary>
        /// <value>The extensions.</value>
        public ExtensionsType1 Extensions {
            get { return extensionsField; }
            set { extensionsField = value; }
        }


        /// <summary>
        /// Gets or sets the items.
        /// &lt;RoleDescriptor&gt;, &lt;IDPSSODescriptor&gt;, &lt;SPSSODescriptor&gt;,
        /// &lt;AuthnAuthorityDescriptor&gt;, &lt;AttributeAuthorityDescriptor&gt;, &lt;PDPDescriptor&gt;
        /// &lt;AffiliationDescriptor&gt;
        /// </summary>
        /// <value>The items.</value>
        [XmlElementAttribute("AffiliationDescriptor", typeof(AffiliationDescriptor))]
        [XmlElementAttribute("AttributeAuthorityDescriptor", typeof(AttributeAuthorityDescriptor))]
        [XmlElementAttribute("AuthnAuthorityDescriptor", typeof(AuthnAuthorityDescriptor))]
        [XmlElementAttribute("IDPSSODescriptor", typeof(IDPSSODescriptor))]
        [XmlElementAttribute("PDPDescriptor", typeof(PDPDescriptor))]
        [XmlElementAttribute("RoleDescriptor", typeof(RoleDescriptor))]
        [XmlElementAttribute("SPSSODescriptor", typeof(SPSSODescriptor))]
        public object[] Items {
            get { return itemsField; }
            set { itemsField = value; }
        }


        /// <summary>
        /// Gets or sets the organization.
        /// Optional element identifying the organization responsible for the SAML entity described by the
        /// element.
        /// </summary>
        /// <value>The organization.</value>
        public Organization Organization {
            get { return organizationField; }
            set { organizationField = value; }
        }


        /// <summary>
        /// Gets or sets the contact person.
        /// Optional sequence of elements identifying various kinds of contact personnel.
        /// </summary>
        /// <value>The contact person.</value>
        [XmlElementAttribute("ContactPerson")]
        public Contact[] ContactPerson {
            get { return contactPersonField; }
            set { contactPersonField = value; }
        }


        /// <summary>
        /// Gets or sets the additional metadata location.
        /// Optional sequence of namespace-qualified locations where additional metadata exists for the
        /// SAML entity. This may include metadata in alternate formats or describing adherence to other
        /// non-SAML specifications.
        /// </summary>
        /// <value>The additional metadata location.</value>
        [XmlElementAttribute("AdditionalMetadataLocation")]
        public AdditionalMetadataLocation[] AdditionalMetadataLocation {
            get {
                return additionalMetadataLocationField;
            }
            set {
                additionalMetadataLocationField = value;
            }
        }


        /// <summary>
        /// Gets or sets the entity ID.
        /// Specifies the unique identifier of the SAML entity whose metadata is described by the element's
        /// contents.
        /// </summary>
        /// <value>The entity ID.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string entityID {
            get {
                return entityIDField;
            }
            set {
                entityIDField = value;
            }
        }


        /// <summary>
        /// Gets or sets the valid until.
        /// Optional attribute indicates the expiration time of the metadata contained in the element and any
        /// contained elements.
        /// </summary>
        /// <value>The valid until.</value>
        [XmlIgnore]
        public DateTime? validUntil {
            get { return validUntilField; }
            set { validUntilField = value; }
        }

        /// <summary>
        /// Gets or sets the valid until string.
        /// Optional attribute indicates the expiration time of the metadata contained in the element and any
        /// contained elements.
        /// </summary>
        /// <value>The valid until string.</value>
        [XmlAttribute("validUntil")]
        public string validUntilString {
            get
            {
                if (validUntilField == null)
                    return null;
                else
                    return validUntilField.Value.ToUniversalTime().ToString("o");
            }
            set
            {
                if (value == null)
                    validUntilField = null;
                else
                    validUntilField = DateTime.Parse(value);
            }
        }
        
        /// <summary>
        /// Gets or sets the cache duration.
        /// Optional attribute indicates the maximum length of time a consumer should cache the metadata
        /// contained in the element and any contained elements.
        /// </summary>
        /// <value>The cache duration.</value>
        [XmlAttributeAttribute(DataType="duration")]
        public string cacheDuration {
            get {
                return cacheDurationField;
            }
            set {
                cacheDurationField = value;
            }
        }


        /// <summary>
        /// Gets or sets the ID.
        /// A document-unique identifier for the element, typically used as a reference point when signing
        /// </summary>
        /// <value>The ID.</value>
        [XmlAttributeAttribute(DataType="ID")]
        public string ID {
            get {
                return idField;
            }
            set {
                idField = value;
            }
        }


        /// <summary>
        /// Gets or sets any attr.
        /// </summary>
        /// <value>Any attr.</value>
        [XmlAnyAttributeAttribute]
        public XmlAttribute[] AnyAttr {
            get {
                return anyAttrField;
            }
            set {
                anyAttrField = value;
            }
        }
    }
}