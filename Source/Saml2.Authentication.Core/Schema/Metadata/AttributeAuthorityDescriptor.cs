using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;AttributeAuthorityDescriptor&gt; element extends RoleDescriptorType with content
    /// reflecting profiles specific to attribute authorities, SAML authorities that respond to
    /// &lt;samlp:AttributeQuery&gt; messages.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class AttributeAuthorityDescriptor : RoleDescriptor {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AttributeAuthorityDescriptor";

        private Endpoint[] attributeServiceField;
        
        private Endpoint[] assertionIDRequestServiceField;
        
        private string[] nameIDFormatField;
        
        private string[] attributeProfileField;
        
        private dk.nita.saml20.config.Attribute[] attributeField;


        /// <summary>
        /// Gets or sets the attribute service.
        /// One or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Attribute Query protocol defined in [SAMLProf]. All attribute authorities support at least one
        /// such endpoint, by definition.
        /// </summary>
        /// <value>The attribute service.</value>
        [XmlElementAttribute("AttributeService")]
        public Endpoint[] AttributeService {
            get {
                return attributeServiceField;
            }
            set {
                attributeServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the assertion ID request service.
        /// Zero or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Assertion Request protocol defined in [SAMLProf] or the special URI binding for assertion
        /// requests defined in [SAMLBind].
        /// </summary>
        /// <value>The assertion ID request service.</value>
        [XmlElementAttribute("AssertionIDRequestService")]
        public Endpoint[] AssertionIDRequestService {
            get {
                return assertionIDRequestServiceField;
            }
            set {
                assertionIDRequestServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the name ID format.
        /// Zero or more elements of type anyURI that enumerate the name identifier formats supported by
        /// this authority.
        /// </summary>
        /// <value>The name ID format.</value>
        [XmlElementAttribute("NameIDFormat", DataType="anyURI")]
        public string[] NameIDFormat {
            get {
                return nameIDFormatField;
            }
            set {
                nameIDFormatField = value;
            }
        }


        /// <summary>
        /// Gets or sets the attribute profile.
        /// Zero or more elements of type anyURI that enumerate the attribute profiles supported by this
        /// authority.
        /// </summary>
        /// <value>The attribute profile.</value>
        [XmlElementAttribute("AttributeProfile", DataType="anyURI")]
        public string[] AttributeProfile {
            get {
                return attributeProfileField;
            }
            set {
                attributeProfileField = value;
            }
        }


        /// <summary>
        /// Gets or sets the attribute.
        /// Zero or more elements that identify the SAML attributes supported by the authority. Specific
        /// values MAY optionally be included, indicating that only certain values permitted by the attribute's
        /// definition are supported.
        /// </summary>
        /// <value>The attribute.</value>
        [XmlElementAttribute("Attribute", Namespace=Saml20Constants.METADATA)]
        public dk.nita.saml20.config.Attribute[] Attribute {
            get {
                return attributeField;
            }
            set {
                attributeField = value;
            }
        }
    }
}