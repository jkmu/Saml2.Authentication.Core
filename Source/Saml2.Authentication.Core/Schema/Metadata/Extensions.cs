using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// Extension type
    /// </summary>
    [Serializable]
    [XmlType(TypeName="ExtensionsType", Namespace=Saml20Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml20Constants.METADATA, IsNullable = false)]
    public class ExtensionsType1
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Extensions";
        
        private XmlElement[] anyField;

        /// <summary>
        /// Gets or sets any.
        /// </summary>
        /// <value>Any.</value>
        [XmlAnyElement]
        public XmlElement[] Any
        {
            get { return anyField; }
            set { anyField = value; }
        }
    }
}