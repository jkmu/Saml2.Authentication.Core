using System;
using System.Xml.Serialization;
using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;Response&gt; message element is used when a response consists of a list of zero or more assertions
    /// that satisfy the request. It has the complex type ResponseType, which extends StatusResponseType
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class Response : StatusResponse
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "Response";

        private object[] itemsField;


        /// <summary>
        /// Gets or sets the items.
        /// Specifies an assertion by value, or optionally an encrypted assertion by value.
        /// </summary>
        /// <value>The items.</value>
        [XmlElement("Assertion", typeof (Assertion), Namespace=Saml2Constants.ASSERTION)]
        [XmlElement("EncryptedAssertion", typeof (EncryptedElement),
            Namespace=Saml2Constants.ASSERTION)]
        public object[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }
}