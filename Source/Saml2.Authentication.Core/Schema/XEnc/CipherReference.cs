using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// The CipherData is a mandatory element that provides the encrypted data. It must either contain the 
    /// encrypted octet sequence as base64 encoded text of the CipherValue element, or provide a reference to 
    /// an external location containing the encrypted octet sequence via the CipherReference element.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XENC, IsNullable=false)]
    public class CipherReference
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "CipherReference";

        private TransformsType1 itemField;

        private string uRIField;


        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        [XmlElement("Transforms")]
        public TransformsType1 Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        [XmlAttribute(DataType="anyURI")]
        public string URI
        {
            get { return uRIField; }
            set { uRIField = value; }
        }
    }
}