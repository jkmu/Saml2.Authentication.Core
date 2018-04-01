using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// DigestMethod is a required element that identifies the digest algorithm to be applied to the signed 
    /// object. This element uses the general structure here for algorithms specified in Algorithm Identifiers 
    /// and Implementation Requirements (section 6.1). 
    /// 
    /// If the result of the URI dereference and application of Transforms is an XPath node-set (or sufficiently 
    /// functional replacement implemented by the application) then it must be converted as described in the 
    /// Reference Processing Model (section  4.3.3.2). If the result of URI dereference and application of 
    /// transforms is an octet stream, then no conversion occurs (comments might be present if the Canonical 
    /// XML with Comments was specified in the Transforms). The digest algorithm is applied to the data octets 
    /// of the resulting octet stream
    /// 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class DigestMethod
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "DigestMethod";

        private string algorithmField;
        private XmlNode[] anyField;


        /// <summary>
        /// Gets or sets any.
        /// </summary>
        /// <value>Any.</value>
        [XmlText]
        [XmlAnyElement]
        public XmlNode[] Any
        {
            get { return anyField; }
            set { anyField = value; }
        }


        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Algorithm
        {
            get { return algorithmField; }
            set { algorithmField = value; }
        }
    }
}