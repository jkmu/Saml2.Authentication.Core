using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;Organization&gt; element specifies basic information about an organization responsible for a SAML
    /// entity or role. The use of this element is always optional. Its content is informative in nature and does not
    /// directly map to any core SAML elements or attributes.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class Organization {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Organization";

        private ExtensionsType1 extensionsField;
        
        private LocalizedName[] organizationNameField;
        
        private LocalizedName[] organizationDisplayNameField;
        
        private LocalizedURI[] organizationURLField;
        
        private XmlAttribute[] anyAttrField;


        /// <summary>
        /// Gets or sets the extensions.
        /// This contains optional metadata extensions that are agreed upon between a metadata publisher
        /// and consumer. Extensions MUST NOT include global (non-namespace-qualified) elements or
        /// elements qualified by a SAML-defined namespace within this element.
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
        /// Gets or sets the name of the organization.
        /// One or more language-qualified names that may or may not be suitable for human consumption
        /// </summary>
        /// <value>The name of the organization.</value>
        [XmlElementAttribute("OrganizationName")]
        public LocalizedName[] OrganizationName {
            get {
                return organizationNameField;
            }
            set {
                organizationNameField = value;
            }
        }


        /// <summary>
        /// Gets or sets the display name of the organization.
        /// One or more language-qualified names that are suitable for human consumption.
        /// </summary>
        /// <value>The display name of the organization.</value>
        [XmlElementAttribute("OrganizationDisplayName")]
        public LocalizedName[] OrganizationDisplayName {
            get {
                return organizationDisplayNameField;
            }
            set {
                organizationDisplayNameField = value;
            }
        }


        /// <summary>
        /// Gets or sets the organization URL.
        /// One or more language-qualified URIs that specify a location to which to direct a user for additional
        /// information. Note that the language qualifier refers to the content of the material at the specified
        /// location.
        /// </summary>
        /// <value>The organization URL.</value>
        [XmlElementAttribute("OrganizationURL")]
        public LocalizedURI[] OrganizationURL {
            get {
                return organizationURLField;
            }
            set {
                organizationURLField = value;
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