using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Protocol;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;Evidence&gt; element contains one or more assertions or assertion references that the SAML
    /// authority relied on in issuing the authorization decision.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class Evidence
    {
        private ItemsChoiceType6[] itemsElementNameField;
        private object[] itemsField;

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Evidence";

        /// <summary>
        /// Gets or sets the items.
        /// Items may be of types Assertion, AssertionIDRef, AssertionURIRef and EncryptedAssertion
        /// </summary>
        /// <value>The items.</value>
        [XmlElementAttribute("Assertion", typeof (Assertion))]
        [XmlElementAttribute("AssertionIDRef", typeof (string), DataType="NCName")]
        [XmlElementAttribute("AssertionURIRef", typeof (string), DataType="anyURI")]
        [XmlElementAttribute("EncryptedAssertion", typeof (EncryptedElement))]
        [XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }


        /// <summary>
        /// Gets or sets the name of the items element.
        /// </summary>
        /// <value>The name of the items element.</value>
        [XmlElementAttribute("ItemsElementName")]
        [XmlIgnoreAttribute]
        public ItemsChoiceType6[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set { itemsElementNameField = value; }
        }
    }
}