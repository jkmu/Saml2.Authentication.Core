using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;IDPSSODescriptor&gt; element extends SSODescriptorType with content reflecting profiles
    /// specific to identity providers supporting SSO.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = Saml20Constants.METADATA)]
    public class IDPSSODescriptor : SSODescriptor {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "IDPSSODescriptor";

        private Endpoint[] singleSignOnServiceField;
        
        private Endpoint[] nameIDMappingServiceField;
        
        private Endpoint[] assertionIDRequestServiceField;
        
        private string[] attributeProfileField;
        
        private SamlAttribute[] attributeField;
        
        private bool wantAuthnRequestsSignedField;
        
        private bool wantAuthnRequestsSignedFieldSpecified;


        /// <summary>
        /// Gets or sets the single sign on service.
        /// One or more elements of type EndpointType that describe endpoints that support the profiles of
        /// the Authentication Request protocol defined in [SAMLProf]. All identity providers support at least
        /// one such endpoint, by definition. The ResponseLocation attribute MUST be omitted.
        /// </summary>
        /// <value>The single sign on service.</value>
        [XmlElementAttribute("SingleSignOnService")]
        public Endpoint[] SingleSignOnService {
            get {
                return singleSignOnServiceField;
            }
            set {
                singleSignOnServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the name ID mapping service.
        /// Zero or more elements of type EndpointType that describe endpoints that support the Name
        /// Identifier Mapping profile defined in [SAMLProf]. The ResponseLocation attribute MUST be
        /// omitted.
        /// </summary>
        /// <value>The name ID mapping service.</value>
        [XmlElementAttribute("NameIDMappingService")]
        public Endpoint[] NameIDMappingService {
            get {
                return nameIDMappingServiceField;
            }
            set {
                nameIDMappingServiceField = value;
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
        /// Gets or sets the attribute profile.
        /// Zero or more elements of type anyURI that enumerate the attribute profiles supported by this
        /// identity provider. See [SAMLProf] for some possible values for this element.
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
        /// Zero or more elements that identify the SAML attributes supported by the identity provider.
        /// Specific values MAY optionally be included, indicating that only certain values permitted by the
        /// attribute's definition are supported. In this context, "support" for an attribute means that the identity
        /// provider has the capability to include it when delivering assertions during single sign-on.
        /// </summary>
        /// <value>The attribute.</value>
        [XmlElementAttribute("Attribute", Namespace=Saml20Constants.ASSERTION)]
        public SamlAttribute[] Attributes    {
            get {
                return attributeField;
            }
            set {
                attributeField = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [want authn requests signed].
        /// Optional attribute that indicates a requirement for the &lt;samlp:AuthnRequest&gt; messages
        /// received by this identity provider to be signed. If omitted, the value is assumed to be false.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [want authn requests signed]; otherwise, <c>false</c>.
        /// </value>
        [XmlAttributeAttribute]
        public bool WantAuthnRequestsSigned {
            get {
                return wantAuthnRequestsSignedField;
            }
            set {
                wantAuthnRequestsSignedField = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [want authn requests signed specified].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [want authn requests signed specified]; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnoreAttribute]
        public bool WantAuthnRequestsSignedSpecified {
            get {
                return wantAuthnRequestsSignedFieldSpecified;
            }
            set {
                wantAuthnRequestsSignedFieldSpecified = value;
            }
        }
    }
}