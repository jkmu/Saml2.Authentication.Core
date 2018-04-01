using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]    
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class ManageNameIDRequest : RequestAbstract
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "ManageNameIDRequest";

        private object item1Field;
        private object itemField;


        /// <summary>
        /// Gets or sets the item.
        /// The name identifier and associated descriptive data (in plaintext or encrypted form) that specify the
        /// principal as currently recognized by the identity and service providers prior to this request.
        /// </summary>
        /// <value>The item.</value>
        [XmlElement("EncryptedID", typeof (EncryptedElement), Namespace=Saml2Constants.ASSERTION)]
        [XmlElement("NameID", typeof (NameID), Namespace=Saml2Constants.ASSERTION)]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        /// <summary>
        /// Gets or sets the item1.
        /// The new identifier value (in plaintext or encrypted form) to be used when communicating with the
        /// requesting provider concerning this principal, or an indication that the use of the old identifier has
        /// been terminated. In the former case, if the requester is the service provider, the new identifier MUST
        /// appear in subsequent &lt;NameID&gt; elements in the SPProvidedID attribute. If the requester is the
        /// identity provider, the new value will appear in subsequent &lt;NameID&gt; elements as the element's
        /// content.
        /// </summary>
        /// <value>The item1.</value>
        [XmlElement("NewEncryptedID", typeof (EncryptedElement))]
        [XmlElement("NewID", typeof (string))]
        [XmlElement("Terminate", typeof (Terminate))]
        public object Item1
        {
            get { return item1Field; }
            set { item1Field = value; }
        }
    }
}