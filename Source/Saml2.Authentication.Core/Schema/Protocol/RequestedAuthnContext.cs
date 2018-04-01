using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;RequestedAuthnContext&gt; element specifies the authentication context requirements of
    /// authentication statements returned in response to a request or query.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class RequestedAuthnContext
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "RequestedAuthnContext";

        private AuthnContextComparisonType comparisonField;

        private bool comparisonFieldSpecified;
        private ItemsChoiceType7[] itemsElementNameField;
        private string[] itemsField;


        /// <summary>
        /// Gets or sets the items.
        /// Specifies one or more URI references identifying authentication context classes or declarations.
        /// </summary>
        /// <value>The items.</value>
        [XmlElement("AuthnContextClassRef", typeof (string), Namespace=Saml2Constants.ASSERTION,
            DataType="anyURI")]
        [XmlElement("AuthnContextDeclRef", typeof (string), Namespace=Saml2Constants.ASSERTION,
            DataType="anyURI")]
        [XmlChoiceIdentifier("ItemsElementName")]
        public string[] Items
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
        public ItemsChoiceType7[] ItemsElementName
        {
            get { return itemsElementNameField; }
            set { itemsElementNameField = value; }
        }


        /// <summary>
        /// Gets or sets the comparison.
        /// Specifies the comparison method used to evaluate the requested context classes or statements, one
        /// of "exact", "minimum", "maximum", or "better". The default is "exact".
        /// If Comparison is set to "exact" or omitted, then the resulting authentication context in the authentication
        /// statement MUST be the exact match of at least one of the authentication contexts specified.
        /// If Comparison is set to "minimum", then the resulting authentication context in the authentication
        /// statement MUST be at least as strong (as deemed by the responder) as one of the authentication
        /// contexts specified.
        /// If Comparison is set to "better", then the resulting authentication context in the authentication
        /// statement MUST be stronger (as deemed by the responder) than any one of the authentication contexts
        /// specified.
        /// If Comparison is set to "maximum", then the resulting authentication context in the authentication
        /// statement MUST be as strong as possible (as deemed by the responder) without exceeding the strength
        /// of at least one of the authentication contexts specified.
        /// </summary>
        /// <value>The comparison.</value>
        [XmlAttribute]
        public AuthnContextComparisonType Comparison
        {
            get { return comparisonField; }
            set { comparisonField = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [comparison specified].
        /// </summary>
        /// <value><c>true</c> if [comparison specified]; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool ComparisonSpecified
        {
            get { return comparisonFieldSpecified; }
            set { comparisonFieldSpecified = value; }
        }
    }
}