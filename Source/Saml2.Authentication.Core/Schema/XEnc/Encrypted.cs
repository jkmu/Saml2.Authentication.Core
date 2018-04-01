using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XmlDSig;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// The base class for EncryptedKey and EncryptedData
    /// </summary>
    [XmlInclude(typeof (EncryptedKey))]
    [XmlInclude(typeof (EncryptedData))]
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC)]
    public abstract class Encrypted
    {
        private CipherData cipherDataField;
        private string encodingField;
        private EncryptionMethod encryptionMethodField;

        private EncryptionProperties encryptionPropertiesField;

        private string idField;
        private KeyInfo keyInfoField;

        private string mimeTypeField;
        private string typeField;


        /// <summary>
        /// Gets or sets the encryption method.
        /// the RSA public key algorithm.
        /// </summary>
        /// <value>The encryption method.</value>
        public EncryptionMethod EncryptionMethod
        {
            get { return encryptionMethodField; }
            set { encryptionMethodField = value; }
        }


        /// <summary>
        /// Gets or sets the key info.
        /// </summary>
        /// <value>The key info.</value>
        [XmlElement(Namespace=Saml2Constants.XMLDSIG)]
        public KeyInfo KeyInfo
        {
            get { return keyInfoField; }
            set { keyInfoField = value; }
        }


        /// <summary>
        /// Gets or sets the cipher data.
        /// </summary>
        /// <value>The cipher data.</value>
        public CipherData CipherData
        {
            get { return cipherDataField; }
            set { cipherDataField = value; }
        }


        /// <summary>
        /// Gets or sets the encryption properties.
        /// </summary>
        /// <value>The encryption properties.</value>
        public EncryptionProperties EncryptionProperties
        {
            get { return encryptionPropertiesField; }
            set { encryptionPropertiesField = value; }
        }


        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [XmlAttribute(DataType="ID")]
        public string Id
        {
            get { return idField; }
            set { idField = value; }
        }


        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Type
        {
            get { return typeField; }
            set { typeField = value; }
        }


        /// <summary>
        /// Gets or sets the type of the MIME.
        /// </summary>
        /// <value>The type of the MIME.</value>
        [XmlAttribute]
        public string MimeType
        {
            get { return mimeTypeField; }
            set { mimeTypeField = value; }
        }


        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Encoding
        {
            get { return encodingField; }
            set { encodingField = value; }
        }
    }
}