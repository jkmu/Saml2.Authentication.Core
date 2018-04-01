using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// For the inclusion of assertions about the signature itself (e.g., signature semantics, the time of signing 
    /// or the serial number of hardware used in cryptographic processes). Such assertions may be signed by including 
    /// a Reference for the SignatureProperties in SignedInfo. While the signing application should be very careful 
    /// about what it signs (it should understand what is in the SignatureProperty) a receiving application has no 
    /// obligation to understand that semantic (though its parent trust engine may wish to). Any content about the 
    /// signature generation may be located within the SignatureProperty element. The mandatory Target attribute 
    /// references the Signature element to which the property applies. 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class SignatureProperty
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SignatureProperty";

        private string idField;
        private XmlElement[] itemsField;

        private string targetField;
        private string[] textField;


        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        [XmlAnyElement]
        public XmlElement[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
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


        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Target
        {
            get { return targetField; }
            set { targetField = value; }
        }


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttribute(DataType="ID")]
        public string Id
        {
            get { return idField; }
            set { idField = value; }
        }
    }
}