using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// If the requester knows the unique identifier of one or more assertions, the &lt;AssertionIDRequest&gt;
    /// message element can be used to request that they be returned in a &lt;Response&gt; message. The
    /// &lt;saml:AssertionIDRef&gt; element is used to specify each assertion to return. See Section 2.3.1 for
    /// more information on this element.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class AssertionIDRequest : RequestAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "AssertionIDRequest";

        private string[] assertionIDRefField;

        /// <summary>
        /// Gets or sets the assertion ID ref.
        /// </summary>
        /// <value>The assertion ID ref.</value>
        [XmlElement("AssertionIDRef", Namespace=Saml2Constants.ASSERTION, DataType="NCName")]
        public string[] AssertionIDRef
        {
            get { return assertionIDRefField; }
            set { assertionIDRefField = value; }
        }
    }
}