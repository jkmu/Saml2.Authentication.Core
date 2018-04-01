using System;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// KeyInfo is an optional element that enables the recipient(s) to obtain the key needed to validate the 
    /// signature.  KeyInfo may contain keys, names, certificates and other public key management information, 
    /// such as in-band key distribution or key agreement data. This specification defines a few simple types 
    /// but applications may extend those types or all together replace them with their own key identification 
    /// and exchange semantics using the XML namespace facility. [XML-ns] However, questions of trust of such 
    /// key information (e.g., its authenticity or  strength) are out of scope of this specification and left 
    /// to the application. 
    /// 
    /// If KeyInfo is omitted, the recipient is expected to be able to identify the key based on application 
    /// context. Multiple declarations within KeyInfo refer to the same key. While applications may define and 
    /// use any mechanism they choose through inclusion of elements from a different namespace, compliant 
    /// versions MUST implement KeyValue (section 4.4.2) and SHOULD implement RetrievalMethod (section 4.4.3). 
    /// 
    /// The schema/DTD specifications of many of KeyInfo's children (e.g., PGPData, SPKIData, X509Data) permit 
    /// their content to be extended/complemented with elements from another namespace. This may be done only 
    /// if it is safe to ignore these extension elements while claiming support for the types defined in this 
    /// specification. Otherwise, external elements, including alternative structures to those defined by this 
    /// specification, MUST be a child of KeyInfo. For example, should a complete XML-PGP standard be defined, 
    /// its root element MUST be a child of KeyInfo. (Of course, new structures from external namespaces can 
    /// incorporate elements from the dsig namespace via features of the type definition language.
    /// 
    /// </summary>
    [Serializable]    
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class KeyInfo
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "KeyInfo";

        /// <summary>
        /// An implicit conversion between our Xml Serialization class, and the .NET framework's built-in version of KeyInfo.
        /// </summary>
        public static explicit operator System.Security.Cryptography.Xml.KeyInfo(KeyInfo ki)
        {
            System.Security.Cryptography.Xml.KeyInfo result = new System.Security.Cryptography.Xml.KeyInfo();
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.LoadXml(Serialization.SerializeToXmlString(ki));
            result.LoadXml(doc.DocumentElement);
            return result;
        }

        private string idField;
        private ItemsChoiceType2[] itemsElementNameField;
        private object[] itemsField;

        private string[] textField;

        /// <summary>
        /// Gets or sets the items.
        /// Items are of types:
        /// KeyName, KeyValue, MgmtData, PGPData, RetrievalMethod, SPKIData, X509Data
        /// </summary>
        /// <value>The items.</value>
        [XmlAnyElement]
        [XmlElement("KeyName", typeof (string))]
        [XmlElement("KeyValue", typeof (KeyValue))]
        [XmlElement("MgmtData", typeof (string))]
        [XmlElement("PGPData", typeof (PGPData))]
        [XmlElement("RetrievalMethod", typeof (RetrievalMethod))]
        [XmlElement("SPKIData", typeof (SPKIData))]
        [XmlElement("X509Data", typeof (X509Data))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }

        /// <summary>
        /// Gets or sets the name of the items element.
        /// </summary>
        /// <value>The name of the items element.</value>
        [XmlElement("ItemsElementName")]
        [XmlIgnore]
        public ItemsChoiceType2[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set { itemsElementNameField = value; }
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