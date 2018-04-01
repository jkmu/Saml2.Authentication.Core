using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XEnc
{
    /// <summary>
    /// Holds list of EncryptionProperty elements
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XENC)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XENC, IsNullable=false)]
    public class EncryptionProperties
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "EncryptionProperties";

        private EncryptionProperty[] encryptionPropertyField;

        private string idField;


        /// <summary>
        /// Gets or sets the encryption property.
        /// </summary>
        /// <value>The encryption property.</value>
        [XmlElement("EncryptionProperty")]
        public EncryptionProperty[] EncryptionProperty
        {
            get { return encryptionPropertyField; }
            set { encryptionPropertyField = value; }
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
    }
}