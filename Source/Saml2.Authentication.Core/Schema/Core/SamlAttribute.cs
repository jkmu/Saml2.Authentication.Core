using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Metadata;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;Attribute&gt; element identifies an attribute by name and optionally includes its value(s). It has the
    /// AttributeType complex type. It is used within an attribute statement to express particular attributes and
    /// values associated with an assertion subject, as described in the previous section. It is also used in an
    /// attribute query to request that the values of specific SAML attributes be returned (see Section 3.3.2.3 for
    /// more information).
    /// </summary>
    [XmlInclude(typeof(RequestedAttribute))]
    [Serializable]
    [XmlType(Namespace = Saml2Constants.ASSERTION)]
    [XmlRoot(SamlAttribute.ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public class SamlAttribute
    {
        /// <summary>
        /// Nameformat "uri".
        /// </summary>
        public const string NAMEFORMAT_URI = "urn:oasis:names:tc:SAML:2.0:attrname-format:uri";

        /// <summary>
        /// Nameformat "basic".
        /// </summary>
        public const string NAMEFORMAT_BASIC = "urn:oasis:names:tc:SAML:2.0:attrname-format:basic";



        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Attribute";

        private XmlAttribute[] anyAttrField;
        private string[] attributeValueField;
        private string friendlyNameField;

        private string nameField;

        private string nameFormatField;


        /// <summary>
        /// Gets or sets the attribute value.
        /// Contains a value of the attribute. If an attribute contains more than one discrete value, it is
        /// RECOMMENDED that each value appear in its own &lt;AttributeValue&gt; element. If more than
        /// one &lt;AttributeValue&gt; element is supplied for an attribute, and any of the elements have a
        /// datatype assigned through xsi:type, then all of the &lt;AttributeValue&gt; elements must have
        /// the identical datatype assigned.
        /// </summary>
        /// <value>The attribute value.</value>
        [XmlElement("AttributeValue", IsNullable = true)]
        public string[] AttributeValue
        {
            get { return attributeValueField; }
            set { attributeValueField = value; }
        }


        /// <summary>
        /// The name of the attribute.
        /// </summary>
        [XmlAttributeAttribute]
        public string Name
        {
            get { return nameField; }
            set { nameField = value; }
        }


        /// <summary>
        /// A URI reference representing the classification of the attribute name for purposes of interpreting the
        /// name. See Section 8.2 for some URI references that MAY be used as the value of the NameFormat
        /// attribute and their associated descriptions and processing rules. If no NameFormat value is provided,
        /// the identifier urn:oasis:names:tc:SAML:2.0:attrname-format:unspecified (see Section
        /// 8.2.1) is in effect.
        /// </summary>
        [XmlAttributeAttribute(DataType = "anyURI")]
        public string NameFormat
        {
            get { return nameFormatField; }
            set { nameFormatField = value; }
        }


        /// <summary>
        /// A string that provides a more human-readable form of the attribute's name, which may be useful in
        /// cases in which the actual Name is complex or opaque, such as an OID or a UUID. This attribute's
        /// value MUST NOT be used as a basis for formally identifying SAML attributes.
        /// </summary>
        [XmlAttributeAttribute]
        public string FriendlyName
        {
            get { return friendlyNameField; }
            set { friendlyNameField = value; }
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

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var value in AttributeValue)
            {
                builder.Append(value);
            }
            return builder.ToString();
        }
    }
}