using System;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XmlDSig;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;RoleDescriptor&gt; element is an abstract extension point that contains common descriptive
    /// information intended to provide processing commonality across different roles. New roles can be defined
    /// by extending its abstract RoleDescriptorType complex type
    /// </summary>
    [XmlInclude(typeof(AttributeAuthorityDescriptor))]
    [XmlInclude(typeof(PDPDescriptor))]
    [XmlInclude(typeof(AuthnAuthorityDescriptor))]
    [XmlInclude(typeof(SSODescriptor))]
    [XmlInclude(typeof(SPSSODescriptor))]
    [XmlInclude(typeof(IDPSSODescriptor))]    
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public abstract class RoleDescriptor {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "RoleDescriptor";
        
        private Signature signatureField;
        
        private ExtensionsType1 extensionsField;
        
        private KeyDescriptor[] keyDescriptorField;
        
        private Organization organizationField;
        
        private Contact[] contactPersonField;
        
        private string idField;
        
        private DateTime? validUntilField;
        
        private string cacheDurationField;
        
        private string protocolSupportEnumerationField;
        
        private string errorURLField;
        
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
            get {
                return extensionsField;
            }
            set {
                extensionsField = value;
            }
        }


        /// <summary>
        /// Gets or sets the key descriptor.
        /// Optional sequence of elements that provides information about the cryptographic keys that the
        /// entity uses when acting in this role.
        /// </summary>
        /// <value>The key descriptor.</value>
        [XmlElementAttribute("KeyDescriptor")]
        public KeyDescriptor[] KeyDescriptor {
            get {
                return keyDescriptorField;
            }
            set {
                keyDescriptorField = value;
            }
        }


        /// <summary>
        /// Gets or sets the organization.
        /// Optional element specifies the organization associated with this role. Identical to the element used
        /// within the &lt;EntityDescriptor&gt; element.
        /// </summary>
        /// <value>The organization.</value>
        public Organization Organization {
            get {
                return organizationField;
            }
            set {
                organizationField = value;
            }
        }


        /// <summary>
        /// Gets or sets the contact person.
        /// Optional sequence of elements specifying contacts associated with this role. Identical to the
        /// element used within the &lt;EntityDescriptor&gt; element.
        /// </summary>
        /// <value>The contact person.</value>
        [XmlElementAttribute("ContactPerson")]
        public Contact[] ContactPerson {
            get {
                return contactPersonField;
            }
            set {
                contactPersonField = value;
            }
        }


        /// <summary>
        /// Gets or sets the ID.
        /// A document-unique identifier for the element, typically used as a reference point when signing.
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
        /// </summary>
        /// <value>The valid until string.</value>
        [XmlAttribute("validUntil")]
        public string validUntilString {
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
        /// Gets or sets the protocol support enumeration.
        /// A whitespace-delimited set of URIs that identify the set of protocol specifications supported by the
        /// role element. For SAML V2.0 entities, this set MUST include the SAML protocol namespace URI,
        /// urn:oasis:names:tc:SAML:2.0:protocol. Note that future SAML specifications might
        /// share the same namespace URI, but SHOULD provide alternate "protocol support" identifiers to
        /// ensure discrimination when necessary.
        /// </summary>
        /// <remarks>
        /// <seealso cref="protocolSupportEnumeration"/>.
        /// </remarks>
        /// <value>The protocol support enumeration.</value>
        [XmlAttributeAttribute(DataType = "anyURI", AttributeName = "protocolSupportEnumeration")]
        public string protocolSupportEnumerationAsString
        {
            get
            {
                return protocolSupportEnumerationField;
            }
            set
            {
                protocolSupportEnumerationField = value;
            }
        }

        /// <summary>
        /// Gets or sets the protocol support enumeration.
        /// The array is a set of URIs that identify the set of protocol specifications
        /// supported by the role element.
        /// </summary>
        /// <remarks>
        /// <seealso cref="protocolSupportEnumerationAsString"/>
        /// </remarks>
        [XmlIgnore]
        public string[] protocolSupportEnumeration
        {
            get
            {
                return protocolSupportEnumerationField.Split(' ');
            }
            set
            {
                protocolSupportEnumerationField = String.Join(" ", value);
            }
        }

        /// <summary>
        /// Gets or sets the error URL.
        /// Optional URI attribute that specifies a location to direct a user for problem resolution and
        /// additional support related to this role.
        /// </summary>
        /// <value>The error URL.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string errorURL {
            get {
                return errorURLField;
            }
            set {
                errorURLField = value;
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