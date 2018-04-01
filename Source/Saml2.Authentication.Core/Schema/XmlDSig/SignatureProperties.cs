using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.XmlDSig
{
    /// <summary>
    /// Contains a list of SignatureProperty instances
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.XMLDSIG)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.XMLDSIG, IsNullable=false)]
    public class SignatureProperties
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SignatureProperties";

        private string idField;
        private SignatureProperty[] signaturePropertyField;


        /// <summary>
        /// Gets or sets the signature property.
        /// </summary>
        /// <value>The signature property.</value>
        [XmlElement("SignatureProperty")]
        public SignatureProperty[] SignatureProperty
        {
            get { return signaturePropertyField; }
            set { signaturePropertyField = value; }
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