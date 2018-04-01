using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The SignatureValue element contains the actual value of the digital signature; it is always encoded using 
    /// base64 [MIME]. While we identify two SignatureMethod algorithms, one mandatory and one optional to 
    /// implement, user specified algorithms may be used as well. 
    /// </summary>
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class SignatureValue {
        
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SignatureValue";

        private string idField;
        
        private byte[] valueField;


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttributeAttribute(DataType="ID")]
        public string Id {
            get {
                return idField;
            }
            set {
                idField = value;
            }
        }


        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [XmlTextAttribute(DataType="base64Binary")]
        public byte[] Value {
            get {
                return valueField;
            }
            set {
                valueField = value;
            }
        }
    }
}