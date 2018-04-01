using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Protocol;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;AttributeStatement&gt; element describes a statement by the SAML authority asserting that the
    /// assertion subject is associated with the specified attributes. Assertions containing
    /// &lt;AttributeStatement&gt; elements MUST contain a &lt;Subject&gt; element.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class AttributeStatement : StatementAbstract
    {
        private object[] itemsField;

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AttributeStatement";

        /// <summary>
        /// Gets or sets the items.
        /// Items may be of type Attribute and EncryptedAttribute
        /// </summary>
        /// <value>The items.</value>
        [XmlElementAttribute("Attribute", typeof (SamlAttribute))]
        [XmlElementAttribute("EncryptedAttribute", typeof (EncryptedElement))]
        public object[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }
}