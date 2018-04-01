using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;EncryptedAssertion&gt; element represents an assertion in encrypted fashion, as defined by the
    /// XML Encryption Syntax and Processing specification [XMLEnc].
    /// </summary>
    [Serializable]
    [XmlType(Namespace = Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public class EncryptedAssertion : EncryptedElement
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "EncryptedAssertion";
    }
}