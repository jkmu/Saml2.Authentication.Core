using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;AuthnContext&gt; element specifies the context of an authentication event. The element can contain
    /// an authentication context class reference, an authentication context declaration or declaration reference,
    /// or both.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class AuthnContext
    {
        private string[] authenticatingAuthorityField;
        private ItemsChoiceType5[] itemsElementNameField;
        private object[] itemsField;

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "AuthnContext";


        /// <summary>
        /// Gets or sets the items.
        /// Items may be of types: AuthnContextClassRef, AuthnContextDecl and AuthnContextDeclRef
        /// </summary>
        /// <value>The items.</value>
        [XmlElementAttribute("AuthnContextClassRef", typeof (string), DataType="anyURI")]
        [XmlElementAttribute("AuthnContextDecl", typeof (object))]
        [XmlElementAttribute("AuthnContextDeclRef", typeof (string), DataType="anyURI")]
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
        public ItemsChoiceType5[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set { itemsElementNameField = value; }
        }


        /// <summary>
        /// Gets or sets the authenticating authority.
        /// Zero or more unique identifiers of authentication authorities that were involved in the authentication of
        /// the principal (not including the assertion issuer, who is presumed to have been involved without being
        /// explicitly named here).
        /// </summary>
        /// <value>The authenticating authority.</value>
        [XmlElementAttribute("AuthenticatingAuthority", DataType="anyURI")]
        public string[] AuthenticatingAuthority
        {
            get { return authenticatingAuthorityField; }
            set { authenticatingAuthorityField = value; }
        }
    }
}