using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The localizedURIType complex type extends a URI-valued element with a standard XML language
    /// attribute.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml20Constants.METADATA, IsNullable = false)]
    public class LocalizedURI {

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedURI"/> class.
        /// </summary>
        public LocalizedURI() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedURI"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lang">The language.</param>
        public LocalizedURI(string value, string lang)
        {
            valueField = value;
            langField = lang;
        }

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "OrganizationURL";

        private string langField;
        
        private string valueField;


        /// <summary>
        /// Gets or sets the lang.
        /// </summary>
        /// <value>The lang.</value>
        [XmlAttribute(Form=XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
        public string lang {
            get {
                return langField;
            }
            set {
                langField = value;
            }
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [XmlText(DataType="anyURI")]
        public string Value {
            get {
                return valueField;
            }
            set {
                valueField = value;
            }
        }
    }
}