using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;StatusDetail&gt; element MAY be used to specify additional information concerning the status of
    /// the request. The additional information consists of zero or more elements from any namespace, with no
    /// requirement for a schema to be present or for schema validation of the &lt;StatusDetail&gt; contents.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class StatusDetail
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "StatusDetail";

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