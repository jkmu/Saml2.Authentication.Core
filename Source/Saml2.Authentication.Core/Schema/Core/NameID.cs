using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The NameIDType complex type is used when an element serves to represent an entity by a string-valued
    /// name. It is a more restricted form of identifier than the &lt;BaseID&gt; element and is the type underlying both
    /// the &lt;NameID&gt; and &lt;Issuer&gt; elements.
    /// </summary>
    /// 
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public class NameID
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "NameID";

        private string formatField;
        private string nameQualifierField;

        private string sPNameQualifierField;

        private string sPProvidedIDField;

        private string valueField;


        /// <summary>
        /// Gets or sets the name qualifier.
        /// The security or administrative domain that qualifies the name. This attribute provides a means to
        /// federate names from disparate user stores without collision.
        /// </summary>
        /// <value>The name qualifier.</value>
        [XmlAttributeAttribute]
        public string NameQualifier
        {
            get { return nameQualifierField; }
            set { nameQualifierField = value; }
        }


        /// <summary>
        /// Gets or sets the SP name qualifier.
        /// Further qualifies a name with the name of a service provider or affiliation of providers. This
        /// attribute provides an additional means to federate names on the basis of the relying party or
        /// parties.
        /// </summary>
        /// <value>The SP name qualifier.</value>
        [XmlAttributeAttribute]
        public string SPNameQualifier
        {
            get { return sPNameQualifierField; }
            set { sPNameQualifierField = value; }
        }


        /// <summary>
        /// Gets or sets the format.
        /// A URI reference representing the classification of string-based identifier information. See Section
        /// 8.3 for the SAML-defined URI references that MAY be used as the value of the Format attribute
        /// and their associated descriptions and processing rules. Unless otherwise specified by an element
        /// based on this type, if no Format value is provided, then the value
        /// urn:oasis:names:tc:SAML:1.0:nameid-format:unspecified (see Section 8.3.1) is in
        /// effect.
        /// When a Format value other than one specified in Section 8.3 is used, the content of an element
        /// of this type is to be interpreted according to the definition of that format as provided outside of this
        /// specification. If not otherwise indicated by the definition of the format, issues of anonymity,
        /// pseudonymity, and the persistence of the identifier with respect to the asserting and relying parties
        /// are implementation-specific.
        /// </summary>
        /// <value>The format.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string Format
        {
            get { return formatField; }
            set { formatField = value; }
        }


        /// <summary>
        /// Gets or sets the SP provided ID.
        /// A name identifier established by a service provider or affiliation of providers for the entity, if
        /// different from the primary name identifier given in the content of the element. This attribute
        /// provides a means of integrating the use of SAML with existing identifiers already in use by a
        /// service provider. For example, an existing identifier can be "attached" to the entity using the Name
        /// Identifier Management protocol defined in Section 3.6.
        /// </summary>
        /// <value>The SP provided ID.</value>
        [XmlAttributeAttribute]
        public string SPProvidedID
        {
            get { return sPProvidedIDField; }
            set { sPProvidedIDField = value; }
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [XmlTextAttribute]
        public string Value
        {
            get { return valueField; }
            set { valueField = value; }
        }
    }
}