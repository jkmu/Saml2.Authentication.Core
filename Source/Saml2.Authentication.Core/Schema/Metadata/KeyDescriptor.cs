using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.XEnc;
using dk.nita.saml20.Schema.XmlDSig;

namespace dk.nita.saml20.Schema.Metadata
{
    /// <summary>
    /// The &lt;KeyDescriptor&gt; element provides information about the cryptographic key(s) that an entity uses
    /// to sign data or receive encrypted keys, along with additional cryptographic details.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.METADATA)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.METADATA, IsNullable=false)]
    public class KeyDescriptor
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "KeyDescriptor";

        private EncryptionMethod[] encryptionMethodField;
        private KeyInfo keyInfoField;

        private KeyTypes useField;

        private bool useFieldSpecified;


        /// <summary>
        /// The XML Signature element KeyInfo. Can be implicitly converted to the .NET class System.Security.Cryptography.Xml.KeyInfo.
        /// </summary>
        [XmlElement(Namespace=Saml2Constants.XMLDSIG)]
        public KeyInfo KeyInfo
        {
            get { return keyInfoField; }
            set { keyInfoField = value; }
        }


        /// <summary>
        /// Gets or sets the encryption method.
        /// Optional element specifying an algorithm and algorithm-specific settings supported by the entity.
        /// The exact content varies based on the algorithm supported. See [XMLEnc] for the definition of this
        /// element's xenc:EncryptionMethodType complex type.
        /// </summary>
        /// <value>The encryption method.</value>
        [XmlElement("EncryptionMethod")]
        public EncryptionMethod[] EncryptionMethod
        {
            get { return encryptionMethodField; }
            set { encryptionMethodField = value; }
        }


        /// <summary>
        /// Gets or sets the use.
        /// Optional attribute specifying the purpose of the key being described. Values are drawn from the
        /// KeyTypes enumeration, and consist of the values encryption and signing.
        /// </summary>
        /// <value>The use.</value>
        [XmlAttribute]
        public KeyTypes use
        {
            get { return useField; }
            set { useField = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [use specified].
        /// </summary>
        /// <value><c>true</c> if [use specified]; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool useSpecified
        {
            get { return useFieldSpecified; }
            set { useFieldSpecified = value; }
        }
    }
}