using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// The EncryptedKey element is used to transport encryption keys from the originator to a known recipient(s). 
    /// It may be used as a stand-alone XML document, be placed within an application document, or appear inside 
    /// an EncryptedData element as a child of a ds:KeyInfo element. The key value is always encrypted to the 
    /// recipient(s). When EncryptedKey is decrypted the resulting octets are made available to the EncryptionMethod 
    /// algorithm without any additional processing.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XENC, IsNullable=false)]
    public class EncryptedKey : Encrypted
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EncryptedKey";

        private string carriedKeyNameField;

        private string recipientField;
        private ReferenceList referenceListField;


        /// <summary>
        /// Gets or sets the reference list.
        /// </summary>
        /// <value>The reference list.</value>
        public ReferenceList ReferenceList
        {
            get { return referenceListField; }
            set { referenceListField = value; }
        }


        /// <summary>
        /// Gets or sets the name of the carried key.
        /// </summary>
        /// <value>The name of the carried key.</value>
        public string CarriedKeyName
        {
            get { return carriedKeyNameField; }
            set { carriedKeyNameField = value; }
        }


        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [XmlAttribute]
        public string Recipient
        {
            get { return recipientField; }
            set { recipientField = value; }
        }
    }
}