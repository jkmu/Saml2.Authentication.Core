using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Protocol;

namespace dk.nita.saml20.Schema.Core
{
    /// <summary>
    /// The &lt;SubjectConfirmation&gt; element provides the means for a relying party to verify the
    /// correspondence of the subject of the assertion with the party with whom the relying party is
    /// communicating.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.ASSERTION, IsNullable = false)]
    public class SubjectConfirmation
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "SubjectConfirmation";

        /// <summary>
        /// The BEARER_METHOD constant
        /// </summary>
        public const string BEARER_METHOD = "urn:oasis:names:tc:SAML:2.0:cm:bearer";

        private object itemField;

        private string methodField;
        private SubjectConfirmationData subjectConfirmationDataField;


        /// <summary>
        /// Gets or sets the item.
        /// Identifies the entity expected to satisfy the enclosing subject confirmation requirements.
        /// Valid elements are &lt;BaseID&gt;, &lt;NameID&gt;, or &lt;EncryptedID&gt; 
        /// </summary>
        /// <value>The item.</value>
        [XmlElementAttribute("BaseID", typeof (BaseIDAbstract))]
        [XmlElementAttribute("EncryptedID", typeof (EncryptedElement))]
        [XmlElementAttribute("NameID", typeof (NameID))]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        /// <summary>
        /// Gets or sets the subject confirmation data.
        /// Additional confirmation information to be used by a specific confirmation method. For example, typical
        /// content of this element might be a &lt;ds:KeyInfo&gt; element as defined in the XML Signature Syntax
        /// and Processing specification [XMLSig], which identifies a cryptographic key (See also Section
        /// 2.4.1.3). Particular confirmation methods MAY define a schema type to describe the elements,
        /// attributes, or content that may appear in the &lt;SubjectConfirmationData&gt; element.
        /// </summary>
        /// <value>The subject confirmation data.</value>
        public SubjectConfirmationData SubjectConfirmationData
        {
            get { return subjectConfirmationDataField; }
            set { subjectConfirmationDataField = value; }
        }


        /// <summary>
        /// Gets or sets the method.
        /// A URI reference that identifies a protocol or mechanism to be used to confirm the subject. URI
        /// references identifying SAML-defined confirmation methods are currently defined in the SAML profiles
        /// specification [SAMLProf]. Additional methods MAY be added by defining new URIs and profiles or by
        /// private agreement.
        /// </summary>
        /// <value>The method.</value>
        [XmlAttributeAttribute(DataType="anyURI")]
        public string Method
        {
            get { return methodField; }
            set { methodField = value; }
        }
    }
}