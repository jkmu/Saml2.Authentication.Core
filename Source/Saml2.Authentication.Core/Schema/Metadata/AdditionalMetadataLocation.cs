using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;AdditionalMetadataLocation&gt; element is a namespace-qualified URI that specifies where
    /// additional XML-based metadata may exist for a SAML entity. Its AdditionalMetadataLocationType
    /// complex type extends the anyURI type with a namespace attribute (also of type anyURI). This required
    /// attribute MUST contain the XML namespace of the root element of the instance document found at the
    /// specified location.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.METADATA, IsNullable=false)]
    public class AdditionalMetadataLocation
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "AdditionalMetadataLocation";

        private string namespaceField;

        private string valueField;


        /// <summary>
        /// Gets or sets the @namespace.
        /// </summary>
        /// <value>The @namespace.</value>
        [XmlAttribute(DataType="anyURI")]
        public string @namespace
        {
            get { return namespaceField; }
            set { namespaceField = value; }
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [XmlText(DataType="anyURI")]
        public string Value
        {
            get { return valueField; }
            set { valueField = value; }
        }
    }
}