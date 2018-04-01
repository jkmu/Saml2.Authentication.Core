using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// Additional information items concerning the generation of the EncryptedData or EncryptedKey can be placed 
    /// in an EncryptionProperty element (e.g., date/time stamp or the serial number of cryptographic hardware used 
    /// during encryption). The Target attribute identifies the EncryptedType structure being described. anyAttribute 
    /// permits the inclusion of attributes from the XML namespace to be included (i.e., xml:space, xml:lang, and 
    /// xml:base).
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XENC, IsNullable=false)]
    public class EncryptionProperty
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EncryptionProperty";

        private XmlAttribute[] anyAttrField;
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


        /// <summary>
        /// Gets or sets any attr.
        /// </summary>
        /// <value>Any attr.</value>
        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr
        {
            get { return anyAttrField; }
            set { anyAttrField = value; }
        }
    }
}