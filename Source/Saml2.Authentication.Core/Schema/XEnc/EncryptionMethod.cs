using System;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// EncryptionMethod is an optional element that describes the encryption algorithm applied to the cipher data. 
    /// If the element is absent, the encryption algorithm must be known by the recipient or the decryption will fail.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.METADATA, IsNullable=false)]
    public class EncryptionMethod
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EncryptionMethod";

        private string algorithmField;
        private XmlNode[] anyField;
        private string keySizeField;

        private byte[] oAEPparamsField;

        /// <summary>
        /// Gets or sets the size of the key.
        /// </summary>
        /// <value>The size of the key.</value>
        [XmlElement(DataType="integer")]
        public string KeySize
        {
            get { return keySizeField; }
            set { keySizeField = value; }
        }

        /// <summary>
        /// Gets or sets the OAE pparams.
        /// </summary>
        /// <value>The OAE pparams.</value>
        [XmlElement(DataType="base64Binary")]
        public byte[] OAEPparams
        {
            get { return oAEPparamsField; }
            set { oAEPparamsField = value; }
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