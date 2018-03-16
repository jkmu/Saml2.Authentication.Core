using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;ContactPerson&gt; element specifies basic contact information about a person responsible in some
    /// capacity for a SAML entity or role. The use of this element is always optional. Its content is informative in
    /// nature and does not directly map to any core SAML elements or attributes.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml20Constants.METADATA, IsNullable = false)]
    public class Contact
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public Contact() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="contactType">Type of the contact.</param>
        public Contact(ContactType contactType)
        {
            contactTypeField = contactType;
        }

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "ContactPerson";

        private XmlAttribute[] anyAttrField;
        private string companyField;
        private ContactType contactTypeField;
        private string[] emailAddressField;
        private ExtensionsType1 extensionsField;

        private string givenNameField;

        private string surNameField;

        private string[] telephoneNumberField;


        /// <summary>
        /// Gets or sets the extensions.
        /// This contains optional metadata extensions that are agreed upon between a metadata publisher
        /// and consumer. Extension elements MUST be namespace-qualified by a non-SAML-defined
        /// namespace.
        /// </summary>
        /// <value>The extensions.</value>
        public ExtensionsType1 Extensions
        {
            get { return extensionsField; }
            set { extensionsField = value; }
        }


        /// <summary>
        /// Gets or sets the company.
        /// Optional string element that specifies the name of the company for the contact person.
        /// </summary>
        /// <value>The company.</value>
        public string Company
        {
            get { return companyField; }
            set { companyField = value; }
        }


        /// <summary>
        /// Gets or sets the name of the given.
        /// Optional string element that specifies the given (first) name of the contact person.
        /// </summary>
        /// <value>The name of the given.</value>
        public string GivenName
        {
            get { return givenNameField; }
            set { givenNameField = value; }
        }


        /// <summary>
        /// Optional string element that specifies the surname of the contact person.
        /// </summary>
        /// <value>The name of the sur.</value>
        public string SurName
        {
            get { return surNameField; }
            set { surNameField = value; }
        }


        /// <summary>
        /// Gets or sets the email address.
        /// Zero or more elements containing mailto: URIs representing e-mail addresses belonging to the
        /// contact person.
        /// </summary>
        /// <value>The email address.</value>
        [XmlElement("EmailAddress", DataType="anyURI")]
        public string[] EmailAddress
        {
            get { return emailAddressField; }
            set { emailAddressField = value; }
        }


        /// <summary>
        /// Gets or sets the telephone number.
        /// Zero or more string elements specifying a telephone number of the contact person.
        /// </summary>
        /// <value>The telephone number.</value>
        [XmlElement("TelephoneNumber")]
        public string[] TelephoneNumber
        {
            get { return telephoneNumberField; }
            set { telephoneNumberField = value; }
        }


        /// <summary>
        /// Gets or sets the type of the contact.
        /// Specifies the type of contact using the ContactTypeType enumeration. The possible values are
        /// technical, support, administrative, billing, and other.
        /// </summary>
        /// <value>The type of the contact.</value>
        [XmlAttribute]
        public ContactType contactType
        {
            get { return contactTypeField; }
            set { contactTypeField = value; }
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