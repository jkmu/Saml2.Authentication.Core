using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;AttributeConsumingService&gt; element defines a particular service offered by the service
    /// provider in terms of the attributes the service requires or desires.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class AttributeConsumingService
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "AttributeConsumingService";

        private ushort indexField;

        private bool? isDefaultField;

        private RequestedAttribute[] requestedAttributeField;
        private LocalizedName[] serviceDescriptionField;
        private LocalizedName[] serviceNameField;


        /// <summary>
        /// Gets or sets the name of the service.
        /// One or more language-qualified names for the service.
        /// </summary>
        /// <value>The name of the service.</value>
        [XmlElement("ServiceName")]
        public LocalizedName[] ServiceName
        {
            get { return serviceNameField; }
            set { serviceNameField = value; }
        }


        /// <summary>
        /// Gets or sets the service description.
        /// Zero or more language-qualified strings that describe the service.
        /// </summary>
        /// <value>The service description.</value>
        [XmlElement("ServiceDescription")]
        public LocalizedName[] ServiceDescription
        {
            get { return serviceDescriptionField; }
            set { serviceDescriptionField = value; }
        }


        /// <summary>
        /// Gets or sets the requested attribute.
        /// One or more elements specifying attributes required or desired by this service.
        /// </summary>
        /// <value>The requested attribute.</value>
        [XmlElement("RequestedAttribute")]
        public RequestedAttribute[] RequestedAttribute
        {
            get { return requestedAttributeField; }
            set { requestedAttributeField = value; }
        }


        /// <summary>
        /// Gets or sets the index.
        /// A required attribute that assigns a unique integer value to the element so that it can be referenced
        /// in a protocol message.
        /// </summary>
        /// <value>The index.</value>
        [XmlAttribute]
        public ushort index
        {
            get { return indexField; }
            set { indexField = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// Identifies the default service supported by the service provider. Useful if the specific service is not
        /// otherwise indicated by application context. If omitted, the value is assumed to be false.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool? isDefault
        {
            get { return isDefaultField; }
            set { isDefaultField = value; }
        } 

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// Identifies the default service supported by the service provider. Useful if the specific service is not
        /// otherwise indicated by application context. If omitted, the value is assumed to be false.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is default; otherwise, <c>false</c>.
        /// </value>
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
                if (string.IsNullOrEmpty(value))
                    isDefaultField = null;
                else 
                    isDefaultField = XmlConvert.ToBoolean(value);
            }
        }
    }
}