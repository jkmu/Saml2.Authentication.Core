using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;BaseID&gt; element is an extension point that allows applications to add new kinds of identifiers. Its
    /// BaseIDAbstractType complex type is abstract and is thus usable only as the base of a derived type.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public abstract class BaseIDAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "BaseID";

        private string nameQualifierField;

        private string sPNameQualifierField;

        /// <summary>
        /// Gets or sets the name qualifier.
        /// The security or administrative domain that qualifies the identifier. This attribute provides a means
        /// to federate identifiers from disparate user stores without collision.
        /// </summary>
        /// <value>The name qualifier.</value>
        [XmlAttribute]
        public string NameQualifier
        {
            get { return nameQualifierField; }
            set { nameQualifierField = value; }
        }

        /// <summary>
        /// Gets or sets the SP name qualifier.
        /// Further qualifies an identifier with the name of a service provider or affiliation of providers. This
        /// attribute provides an additional means to federate identifiers on the basis of the relying party or
        /// parties.
        /// </summary>
        /// <value>The SP name qualifier.</value>
        [XmlAttribute]
        public string SPNameQualifier
        {
            get { return sPNameQualifierField; }
            set { sPNameQualifierField = value; }
        }
    }
}