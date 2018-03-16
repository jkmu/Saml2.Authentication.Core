using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;PDPDescriptor&gt; element extends RoleDescriptorType with content reflecting profiles specific to
    /// policy decision points, SAML authorities that respond to &lt;samlp:AuthzDecisionQuery&gt; messages.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class PDPDescriptor : RoleDescriptor {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "Organization";

        private Endpoint[] authzServiceField;
        
        private Endpoint[] assertionIDRequestServiceField;
        
        private string[] nameIDFormatField;


        /// <summary>
        /// Gets or sets the authz service.
        /// One or more elements of type EndpointType that describe endpoints that support the profile of
        /// the Authorization Decision Query protocol defined in [SAMLProf]. All policy decision points support
        /// at least one such endpoint, by definition.
        /// </summary>
        /// <value>The authz service.</value>
        [XmlElementAttribute("AuthzService")]
        public Endpoint[] AuthzService {
            get {
                return authzServiceField;
            }
            set {
                authzServiceField = value;
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
    }
}