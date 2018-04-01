using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// contains an X.509 issuer distinguished name/serial number pair that SHOULD be compliant with RFC2253 
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    public class X509IssuerSerial
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "X509IssuerSerial";

        private string x509IssuerNameField;

        private string x509SerialNumberField;


        /// <summary>
        /// Gets or sets the name of the X509 issuer.
        /// </summary>
        /// <value>The name of the X509 issuer.</value>
        public string X509IssuerName
        {
            get { return x509IssuerNameField; }
            set { x509IssuerNameField = value; }
        }


        /// <summary>
        /// Gets or sets the X509 serial number.
        /// </summary>
        /// <value>The X509 serial number.</value>
        [XmlElement(DataType="integer")]
        public string X509SerialNumber
        {
            get { return x509SerialNumberField; }
            set { x509SerialNumberField = value; }
        }
    }
}