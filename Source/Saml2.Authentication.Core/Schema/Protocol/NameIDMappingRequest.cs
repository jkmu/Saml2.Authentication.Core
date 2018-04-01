using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// To request an alternate name identifier for a principal from an identity provider, a requester sends an
    /// &lt;NameIDMappingRequest&gt; message
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class NameIDMappingRequest : RequestAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "NameIDMappingRequest";

        private object itemField;

        private NameIDPolicy nameIDPolicyField;


        /// <summary>
        /// Gets or sets the item.
        /// The identifier and associated descriptive data that specify the principal as currently recognized by the
        /// requester and the responder.
        /// </summary>
        /// <value>The item.</value>
        [XmlElement("BaseID", typeof (BaseIDAbstract), Namespace=Saml2Constants.ASSERTION)]
        [XmlElement("EncryptedID", typeof (EncryptedElement), Namespace=Saml2Constants.ASSERTION)]
        [XmlElement("NameID", typeof (NameID), Namespace=Saml2Constants.ASSERTION)]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        /// <summary>
        /// Gets or sets the name ID policy.
        /// The requirements regarding the format and optional name qualifier for the identifier to be returned
        /// </summary>
        /// <value>The name ID policy.</value>
        public NameIDPolicy NameIDPolicy
        {
            get { return nameIDPolicyField; }
            set { nameIDPolicyField = value; }
        }
    }
}