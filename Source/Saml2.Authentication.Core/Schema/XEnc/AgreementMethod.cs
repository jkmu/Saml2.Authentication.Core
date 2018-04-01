using System;
using System.Xml;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XmlDSig;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// The AgreementMethod element appears as the content of a ds:KeyInfo since, like other ds:KeyInfo children, 
    /// it yields a key. This ds:KeyInfo is in turn a child of an EncryptedData or EncryptedKey element. The 
    /// Algorithm attribute and KeySize child of the EncryptionMethod element under this EncryptedData or 
    /// EncryptedKey element are implicit parameters to the key agreement computation. In cases where this 
    /// EncryptionMethod algorithm URI is insufficient to determine the key length, a KeySize MUST have been 
    /// included. In addition, the sender may place a KA-Nonce element under AgreementMethod to assure that different 
    /// keying material is generated even for repeated agreements using the same sender and recipient public keys.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XENC, IsNullable=false)]
    public class AgreementMethod
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "AgreementMethod";

        private string algorithmField;
        private XmlNode[] anyField;
        private byte[] kANonceField;

        private KeyInfo originatorKeyInfoField;

        private KeyInfo recipientKeyInfoField;


        /// <summary>
        /// Gets or sets the KA nonce.
        /// </summary>
        /// <value>The KA nonce.</value>
        [XmlElement("KA-Nonce", DataType="base64Binary")]
        public byte[] KANonce
        {
            get { return kANonceField; }
            set { kANonceField = value; }
        }


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
        /// Gets or sets the originator key info.
        /// </summary>
        /// <value>The originator key info.</value>
        public KeyInfo OriginatorKeyInfo
        {
            get { return originatorKeyInfoField; }
            set { originatorKeyInfoField = value; }
        }


        /// <summary>
        /// Gets or sets the recipient key info.
        /// </summary>
        /// <value>The recipient key info.</value>
        public KeyInfo RecipientKeyInfo
        {
            get { return recipientKeyInfoField; }
            set { recipientKeyInfoField = value; }
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