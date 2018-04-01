using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The Saml20 ProxyRestriction condition class.
    /// Specifies limitations that the asserting party imposes on relying parties that wish to subsequently act
    /// as asserting parties themselves and issue assertions of their own on the basis of the information
    /// contained in the original assertion. Although the schema permits multiple occurrences, there MUST
    /// be at most one instance of this element.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class ProxyRestriction : ConditionAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "ProxyRestriction";

        private string[] audienceField;

        private string countField;

        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        /// <value>The audience.</value>
        [XmlElementAttribute("Audience", DataType="anyURI")]
        public string[] Audience
        {
            get { return audienceField; }
            set { audienceField = value; }
        }


        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        [XmlAttributeAttribute(DataType="nonNegativeInteger")]
        public string Count
        {
            get { return countField; }
            set { countField = value; }
        }
    }
}