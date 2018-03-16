using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;SPSSODescriptor&gt; element extends SSODescriptorType with content reflecting profiles specific
    /// to service providers.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class SPSSODescriptor : SSODescriptor {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "SPSSODescriptor";
        
        private IndexedEndpoint[] assertionConsumerServiceField;
        
        private AttributeConsumingService[] attributeConsumingServiceField;
        
        private bool? authnRequestsSignedField;
        
        private bool? wantAssertionsSignedField;

        /// <summary>
        /// Gets or sets the assertion consumer service.
        /// One or more elements that describe indexed endpoints that support the profiles of the
        /// Authentication Request protocol defined in [SAMLProf]. All service providers support at least one
        /// such endpoint, by definition.
        /// </summary>
        /// <value>The assertion consumer service.</value>
        [XmlElementAttribute("AssertionConsumerService")]
        public IndexedEndpoint[] AssertionConsumerService {
            get {
                return assertionConsumerServiceField;
            }
            set {
                assertionConsumerServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the attribute consuming service.
        /// Zero or more elements that describe an application or service provided by the service provider
        /// that requires or desires the use of SAML attributes.
        /// </summary>
        /// <value>The attribute consuming service.</value>
        [XmlElementAttribute("AttributeConsumingService")]
        public AttributeConsumingService[] AttributeConsumingService {
            get {
                return attributeConsumingServiceField;
            }
            set {
                attributeConsumingServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [authn requests signed].
        /// Optional attribute that indicates whether the &lt;samlp:AuthnRequest&gt; messages sent by this
        /// service provider will be signed. If omitted, the value is assumed to be false.
        /// </summary>
        /// <value><c>true</c> if [authn requests signed]; otherwise, <c>false</c>.</value>
        [XmlAttributeAttribute]
        public string AuthnRequestsSigned {
            get 
            {
                if (authnRequestsSignedField == null)
                    return null;
                else
                    return XmlConvert.ToString(authnRequestsSignedField.Value);
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    authnRequestsSignedField = null;
                else 
                    authnRequestsSignedField = XmlConvert.ToBoolean(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [want assertions signed].
        /// Optional attribute that indicates a requirement for the &lt;saml:Assertion&gt; elements received by
        /// this service provider to be signed. If omitted, the value is assumed to be false. This requirement
        /// is in addition to any requirement for signing derived from the use of a particular profile/binding
        /// combination.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [want assertions signed]; otherwise, <c>false</c>.
        /// </value>
        [XmlAttributeAttribute]
        public string WantAssertionsSigned {
            get 
            {
                if (wantAssertionsSignedField == null)
                    return null;
                else
                    return XmlConvert.ToString(wantAssertionsSignedField.Value);
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    wantAssertionsSignedField = null;
                else 
                    wantAssertionsSignedField = XmlConvert.ToBoolean(value);
            }
        }
    }
}