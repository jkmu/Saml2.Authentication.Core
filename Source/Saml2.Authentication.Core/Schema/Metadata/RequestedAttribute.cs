using System;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;RequestedAttribute&gt; element specifies a service provider's interest in a specific SAML
    /// attribute, optionally including specific values.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = Saml2Constants.METADATA)]
    [XmlRoot(RequestedAttribute.ELEMENT_NAME, Namespace = Saml2Constants.METADATA, IsNullable = false)]
    public class RequestedAttribute : SamlAttribute
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "RequestedAttribute";

        private bool? isRequiredField;



        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// Optional XML attribute indicates if the service requires the corresponding SAML attribute in order
        /// to function at all (as opposed to merely finding an attribute useful or desirable).
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        [XmlIgnore]
        public bool? isRequired
        {
            get { return isRequiredField; }
            set { isRequiredField = value; }
        }

        /// <summary>
        /// Gets or sets the is required string.
        /// </summary>
        /// <value>The is required string.</value>
        [XmlAttribute("isRequired")]
        public string isRequiredString
        {
            get {
                if (isRequiredField.HasValue)
                    return XmlConvert.ToString(isRequiredField.Value);
                else
                    return null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    isRequiredField = null;
                else
                    isRequiredField = XmlConvert.ToBoolean(value);
            }
        }
    }
}