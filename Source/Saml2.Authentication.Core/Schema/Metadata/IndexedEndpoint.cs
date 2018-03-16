using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The complex type IndexedEndpointType extends EndpointType with a pair of attributes to permit the
    /// indexing of otherwise identical endpoints so that they can be referenced by protocol messages.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml20Constants.METADATA, IsNullable = false)]
    public class IndexedEndpoint : Endpoint {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "ArtifactResolutionService";

        private ushort indexField;
        
        private bool? isDefaultField;
        
        /// <summary>
        /// Gets or sets the index.
        /// A required attribute that assigns a unique integer value to the endpoint so that it can be
        /// referenced in a protocol message. The index value need only be unique within a collection of like
        /// elements contained within the same parent element (i.e., they need not be unique across the
        /// entire instance).
        /// </summary>
        /// <value>The index.</value>
        [XmlAttribute]
        public ushort index {
            get {
                return indexField;
            }
            set {
                indexField = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// An optional boolean attribute used to designate the default endpoint among an indexed set. If
        /// omitted, the value is assumed to be false.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool? isDefault {
            get {
                return isDefaultField;
            }
            set {
                isDefaultField = value;
            }
        }

        /// <summary>
        /// Gets or sets the isDefault string.
        /// </summary>
        /// <value>The isDefault string.</value>
        [XmlAttribute("isDefault")]
        public string isDefaultString
        {
            get
            {
                if (isDefaultField == null)
                    return null;
                else
                    return XmlConvert.ToString(isDefaultField.Value);
            }
            set
            {
                if (value == null)
                    isDefaultField = null;
                else
                    isDefaultField = XmlConvert.ToBoolean(value);
            }
        }
    }
}