using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// This extension point contains optional protocol message extension elements that are agreed on
    /// between the communicating parties. No extension schema is required in order to make use of this
    /// extension point, and even if one is provided, the lax validation setting does not impose a requirement
    /// for the extension to be valid. SAML extension elements MUST be namespace-qualified in a non-
    /// SAML-defined namespace.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class Extensions
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