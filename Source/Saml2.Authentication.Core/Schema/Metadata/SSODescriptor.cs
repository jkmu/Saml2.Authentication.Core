using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The SSODescriptorType abstract type is a common base type for the concrete types
    /// SPSSODescriptorType and IDPSSODescriptorType, described in subsequent sections. It extends
    /// RoleDescriptorType with elements reflecting profiles common to both identity providers and service
    /// providers that support SSO
    /// </summary>
    [XmlInclude(typeof(SPSSODescriptor))]
    [XmlIncludeAttribute(typeof(IDPSSODescriptor))]
    [Serializable]
    [DebuggerStepThrough]
    [XmlTypeAttribute(Namespace=Saml20Constants.METADATA)]
    public abstract class SSODescriptor : RoleDescriptor {
        
        private IndexedEndpoint[] artifactResolutionServiceField;
        
        private Endpoint[] singleLogoutServiceField;
        
        private Endpoint[] manageNameIDServiceField;
        
        private string[] nameIDFormatField;


        /// <summary>
        /// Gets or sets the artifact resolution service.
        /// Zero or more elements of type IndexedEndpointType that describe indexed endpoints that
        /// support the Artifact Resolution profile defined in [SAMLProf]. The ResponseLocation attribute
        /// MUST be omitted.
        /// </summary>
        /// <value>The artifact resolution service.</value>
        [XmlElementAttribute("ArtifactResolutionService")]
        public IndexedEndpoint[] ArtifactResolutionService {
            get {
                return artifactResolutionServiceField;
            }
            set {
                artifactResolutionServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the single logout service.
        /// Zero or more elements of type EndpointType that describe endpoints that support the Single
        /// Logout profiles defined in [SAMLProf].
        /// </summary>
        /// <value>The single logout service.</value>
        [XmlElementAttribute("SingleLogoutService")]
        public Endpoint[] SingleLogoutService {
            get {
                return singleLogoutServiceField;
            }
            set {
                singleLogoutServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the manage name ID service.
        /// Zero or more elements of type EndpointType that describe endpoints that support the Name
        /// Identifier Management profiles defined in [SAMLProf].
        /// </summary>
        /// <value>The manage name ID service.</value>
        [XmlElementAttribute("ManageNameIDService")]
        public Endpoint[] ManageNameIDService {
            get {
                return manageNameIDServiceField;
            }
            set {
                manageNameIDServiceField = value;
            }
        }


        /// <summary>
        /// Gets or sets the name ID format.
        /// Zero or more elements of type anyURI that enumerate the name identifier formats supported by
        /// this system entity acting in this role. See Section 8.3 of [SAMLCore] for some possible values for
        /// this element.
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