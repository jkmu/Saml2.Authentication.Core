using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The recipient of a &lt;NameIDMappingRequest&gt; message MUST respond with a
    /// &lt;NameIDMappingResponse&gt; message.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace = Saml2Constants.PROTOCOL, IsNullable = false)]
    public class NameIDMappingResponse : StatusResponse
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "NameIDMappingResponse";

        private object itemField;


        /// <summary>
        /// Gets or sets the item.
        /// The identifier and associated attributes that specify the principal in the manner requested, usually in
        /// encrypted form
        /// </summary>
        /// <value>The item.</value>
        [XmlElement("EncryptedID", typeof (EncryptedElement), Namespace=Saml2Constants.ASSERTION)]
        [XmlElement("NameID", typeof (NameID), Namespace=Saml2Constants.ASSERTION)]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }
    }
}