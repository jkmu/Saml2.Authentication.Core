using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XmlDSig;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;EntitiesDescriptor&gt; element contains the metadata for an optionally named group of SAML
    /// entities.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class EntitiesDescriptor {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EntitiesDescriptor";
        
        private Signature signatureField;
        
        private ExtensionsType1 extensionsField;
        
        private object[] itemsField;
        
        private DateTime validUntilField;
        
        private bool validUntilFieldSpecified;
        
        private string cacheDurationField;
        
        private string idField;
        
        private string nameField;


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
        /// Gets or sets the items.
        /// Contains the metadata for one or more SAML entities, or a nested group of additional metadata
        /// </summary>
        /// <value>The items.</value>
        [XmlElementAttribute("EntitiesDescriptor", typeof(EntitiesDescriptor))]
        [XmlElementAttribute("EntityDescriptor", typeof(EntityDescriptor))]
        public object[] Items {
            get {
                return itemsField;
            }
            set {
                itemsField = value;
            }
        }


        /// <summary>
        /// Gets or sets the valid until.
        /// Optional attribute indicates the expiration time of the metadata contained in the element and any
        /// contained elements.
        /// </summary>
        /// <value>The valid until.</value>
        [XmlIgnore]
        public DateTime validUntil {
            get {
                return validUntilField;
            }
            set {
                validUntilField = value;
            }
        }

        /// <summary>
        /// Gets or sets the valid until string.
        /// </summary>
        /// <value>The valid until string.</value>
        [XmlAttribute("validUntil")]
        public string validUntilString {
            get {
                return validUntilField.ToUniversalTime().ToString("o");
            }
            set {
                validUntilField = DateTime.Parse(value);
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [valid until specified].
        /// </summary>
        /// <value><c>true</c> if [valid until specified]; otherwise, <c>false</c>.</value>
        [XmlIgnoreAttribute]
        public bool validUntilSpecified {
            get {
                return validUntilFieldSpecified;
            }
            set {
                validUntilFieldSpecified = value;
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
        /// Gets or sets the name.
        /// A string name that identifies a group of SAML entities in the context of some deployment.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttributeAttribute]
        public string Name {
            get {
                return nameField;
            }
            set {
                nameField = value;
            }
        }
    }
}