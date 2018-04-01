using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// The Signature element is the root element of an XML Signature
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class Signature 
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Signature";

        private string idField;
        private KeyInfo keyInfoField;

        private ObjectType[] objectField;
        private SignatureValue signatureValueField;
        private SignedInfo signedInfoField;

        /// <summary>
        /// Gets or sets the signed info.
        /// The structure of SignedInfo includes the canonicalization algorithm, a signature algorithm, and one or 
        /// more references. The SignedInfo element may contain an optional ID attribute that will allow it to be 
        /// referenced by other signatures and objects. 
        /// </summary>
        /// <value>The signed info.</value>
        public SignedInfo SignedInfo
        {
            get { return signedInfoField; }
            set { signedInfoField = value; }
        }

        /// <summary>
        /// Gets or sets the signature value.
        /// </summary>
        /// <value>The signature value.</value>
        public SignatureValue SignatureValue
        {
            get { return signatureValueField; }
            set { signatureValueField = value; }
        }

        /// <summary>
        /// Gets or sets the key info.
        /// </summary>
        /// <value>The key info.</value>
        public KeyInfo KeyInfo
        {
            get { return keyInfoField; }
            set { keyInfoField = value; }
        }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        [XmlElement("Object")]
        public ObjectType[] Object
        {
            get { return objectField; }
            set { objectField = value; }
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