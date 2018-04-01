using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XEnc;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// Represents an encrypted element
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    // NOTE: XmlRoot parameter manually changed from "NewEncryptedID" to "EncryptedElementType".    
    [XmlInclude(typeof(EncryptedAssertion))]
    public class EncryptedElement
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EncryptedElement";

        private EncryptedData encryptedDataField;
        private EncryptedKey[] encryptedKeyField;

        /// <summary>
        /// Gets or sets the encrypted data.
        /// </summary>
        /// <value>The encrypted data.</value>
        [XmlElement("EncryptedData", Namespace=Saml2Constants.XENC)]
        public EncryptedData encryptedData
        {
            get { return encryptedDataField; }
            set { encryptedDataField = value; }
        }

        /// <summary>
        /// Gets or sets the encrypted key.
        /// </summary>
        /// <value>The encrypted key.</value>
        [XmlElement("EncryptedKey", Namespace=Saml2Constants.XENC)]
        public EncryptedKey[] encryptedKey
        {
            get { return encryptedKeyField; }
            set { encryptedKeyField = value; }
        }        
    }
}