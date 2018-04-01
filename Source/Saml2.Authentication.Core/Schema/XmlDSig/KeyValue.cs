using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The KeyValue element contains a single public key that may be useful in validating the signature. 
    /// Structured formats for defining DSA (REQUIRED) and RSA (RECOMMENDED) public keys are defined in Signature 
    /// Algorithms (section 6.4). The KeyValue element may include externally defined public keys values 
    /// represented as PCDATA or element types from an external namespace. 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class KeyValue
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "KeyValue";

        private object itemField;

        private string[] textField;


        /// <summary>
        /// Gets or sets the item.
        /// Item is of type DSAKeyValue or RSAKeyValue
        /// </summary>
        /// <value>The item.</value>
        [XmlAnyElement]
        [XmlElement("DSAKeyValue", typeof (DSAKeyValue))]
        [XmlElement("RSAKeyValue", typeof (RSAKeyValue))]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [XmlText]
        public string[] Text
        {
            get { return textField; }
            set { textField = value; }
        }
    }
}