using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The SPKIData element within KeyInfo is used to convey information related to SPKI public key pairs, 
    /// certificates and other SPKI data. SPKISexp is the base64 encoding of a SPKI canonical S-expression. 
    /// SPKIData must have at least one SPKISexp; SPKISexp can be complemented/extended by siblings from an 
    /// external namespace within SPKIData, or SPKIData can be entirely replaced with an alternative SPKI XML 
    /// structure as a child of KeyInfo. 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class SPKIData
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SPKIData";

        private XmlElement anyField;
        private byte[][] sPKISexpField;


        /// <summary>
        /// Gets or sets the SPKI sexp.
        /// </summary>
        /// <value>The SPKI sexp.</value>
        [XmlElement("SPKISexp", DataType="base64Binary")]
        public byte[][] SPKISexp
        {
            get { return sPKISexpField; }
            set { sPKISexpField = value; }
        }


        /// <summary>
        /// Gets or sets any.
        /// </summary>
        /// <value>Any.</value>
        [XmlAnyElement]
        public XmlElement Any
        {
            get { return anyField; }
            set { anyField = value; }
        }
    }
}