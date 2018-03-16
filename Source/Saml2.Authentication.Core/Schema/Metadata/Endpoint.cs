using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The complex type EndpointType describes a SAML protocol binding endpoint at which a SAML entity can
    /// be sent protocol messages. Various protocol or profile-specific metadata elements are bound to this type.
    /// </summary>
    [XmlInclude(typeof(IndexedEndpoint))]
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml20Constants.METADATA, IsNullable = false)]
    public class Endpoint {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SingleLogoutService";

        private XmlElement[] anyField;
        
        private string bindingField;
        
        private string locationField;
        
        private string responseLocationField;
        
        private XmlAttribute[] anyAttrField;


        /// <summary>
        /// Gets or sets any.
        /// </summary>
        /// <value>Any.</value>
        [XmlAnyElementAttribute]
        public XmlElement[] Any {
            get {
                return anyField;
            }
            set {
                anyField = value;
            }
        }


        /// <summary>
        /// Gets or sets the binding.
        /// A required attribute that specifies the SAML binding supported by the endpoint. Each binding is
        /// assigned a URI to identify it.
        /// </summary>
        /// <value>The binding.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string Binding {
            get {
                return bindingField;
            }
            set {
                bindingField = value;
            }
        }


        /// <summary>
        /// Gets or sets the location.
        /// A required URI attribute that specifies the location of the endpoint. The allowable syntax of this
        /// URI depends on the protocol binding.
        /// </summary>
        /// <value>The location.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string Location {
            get {
                return locationField;
            }
            set {
                locationField = value;
            }
        }


        /// <summary>
        /// Gets or sets the response location.
        /// Optionally specifies a different location to which response messages sent as part of the protocol
        /// or profile should be sent. The allowable syntax of this URI depends on the protocol binding.
        /// </summary>
        /// <value>The response location.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string ResponseLocation {
            get {
                return responseLocationField;
            }
            set {
                responseLocationField = value;
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