using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// DataReference elements are used to refer to EncryptedData elements that were encrypted 
    /// using the key defined in the enclosing EncryptedKey element. Multiple DataReference elements 
    /// can occur if multiple EncryptedData elements exist that are encrypted by the same key.
    /// </summary>
    [Serializable]
    [XmlType(TypeName="ReferenceType", Namespace=Saml2Constants.XENC)]
    public class ReferenceType1
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "ReferenceType";

        private XmlElement[] anyField;

        private string uRIField;


        /// <summary>
        /// Gets or sets any.
        /// </summary>
        /// <value>Any.</value>
        [XmlAnyElement]
        public XmlElement[] Any
        {
            get { return anyField; }
            set { anyField = value; }
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