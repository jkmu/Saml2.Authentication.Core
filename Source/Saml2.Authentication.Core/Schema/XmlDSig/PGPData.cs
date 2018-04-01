using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The PGPData element within KeyInfo is used to convey information related to PGP public key pairs and 
    /// signatures on such keys. The PGPKeyID's value is a base64Binary sequence containing a standard PGP public 
    /// key identifier as defined in [PGP, section 11.2]. The PGPKeyPacket contains a base64-encoded Key Material 
    /// Packet as defined in [PGP, section 5.5]. These children element types can be complemented/extended by 
    /// siblings from an external namespace within PGPData, or PGPData can be replaced all together with an 
    /// alternative PGP XML structure as a child of KeyInfo. PGPData must contain one PGPKeyID and/or one 
    /// PGPKeyPacket and 0 or more elements from an external namespace. 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class PGPData
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "PGPData";

        private ItemsChoiceType1[] itemsElementNameField;
        private object[] itemsField;


        /// <summary>
        /// Gets or sets the items.
        /// Items are of type PGPKeyID or PGPKeyPacket
        /// </summary>
        /// <value>The items.</value>
        [XmlAnyElement]
        [XmlElement("PGPKeyID", typeof (byte[]), DataType="base64Binary")]
        [XmlElement("PGPKeyPacket", typeof (byte[]), DataType="base64Binary")]
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
        public ItemsChoiceType1[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set { itemsElementNameField = value; }
        }
    }
}