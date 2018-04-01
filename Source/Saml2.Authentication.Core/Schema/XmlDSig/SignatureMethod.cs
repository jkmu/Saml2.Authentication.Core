using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// SignatureMethod is a required element that specifies the algorithm used for signature generation and 
    /// validation. This algorithm identifies all cryptographic functions involved in the signature operation 
    /// (e.g. hashing, public key algorithms, MACs, padding, etc.). This element uses the general structure 
    /// here for algorithms described in section 6.1: Algorithm Identifiers and Implementation Requirements. 
    /// While there is a single identifier, that identifier may specify a format containing multiple distinct 
    /// signature values. 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class SignatureMethod {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SignatureMethod";

        private string hMACOutputLengthField;
        
        private XmlNode[] anyField;
        
        private string algorithmField;


        /// <summary>
        /// Gets or sets the length of the HMAC output.
        /// </summary>
        /// <value>The length of the HMAC output.</value>
        [XmlElementAttribute(DataType="integer")]
        public string HMACOutputLength {
            get {
                return hMACOutputLengthField;
            }
            set {
                hMACOutputLengthField = value;
            }
        }


        /// <summary>
        /// Gets or sets any.
        /// </summary>
        /// <value>Any.</value>
        [XmlTextAttribute]
        [XmlAnyElementAttribute]
        public XmlNode[] Any {
            get {
                return anyField;
            }
            set {
                anyField = value;
            }
        }


        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string Algorithm {
            get {
                return algorithmField;
            }
            set {
                algorithmField = value;
            }
        }
    }
}