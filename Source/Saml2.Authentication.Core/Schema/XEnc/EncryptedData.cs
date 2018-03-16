using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// Encrypted data class
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml20Constants.XENC)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml20Constants.XENC, IsNullable=false)]
    public class EncryptedData : Encrypted
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EncryptedData";
    }
}