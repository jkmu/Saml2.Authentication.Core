using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// ReferenceList is an element that contains pointers from a key value of an EncryptedKey to items 
    /// encrypted by that key value (EncryptedData or EncryptedKey elements).
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType=true, Namespace=Saml2Constants.XENC)]
    [XmlRoot(Namespace=Saml2Constants.XENC, IsNullable=false)]
    public class ReferenceList
    {
        private ItemsChoiceType3[] itemsElementNameField;
        private ReferenceType1[] itemsField;


        /// <summary>
        /// Gets or sets the items.
        /// DataReferencee and KeyReference elements
        /// </summary>
        /// <value>The items.</value>
        [XmlElement("DataReference", typeof (ReferenceType1))]
        [XmlElement("KeyReference", typeof (ReferenceType1))]
        [XmlChoiceIdentifier("ItemsElementName")]
        public ReferenceType1[] Items
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
        public ItemsChoiceType3[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set { itemsElementNameField = value; }
        }
    }
}