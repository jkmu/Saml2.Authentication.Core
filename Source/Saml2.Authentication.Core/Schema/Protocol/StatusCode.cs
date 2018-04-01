using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;StatusCode&gt; element specifies a code or a set of nested codes representing the status of the
    /// corresponding request.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class StatusCode
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "StatusCode";

        private StatusCode statusCodeField;

        private string valueField;

        /// <summary>
        /// Gets or sets the sub status code.
        /// A subordinate status code that provides more specific information on an error condition. Note that
        /// responders MAY omit subordinate status codes in order to prevent attacks that seek to probe for
        /// additional information by intentionally presenting erroneous requests.
        /// </summary>
        /// <value>The sub status code.</value>
        [XmlElement("StatusCode", Namespace = Saml2Constants.PROTOCOL)]
        public StatusCode SubStatusCode
        {
            get { return statusCodeField; }
            set { statusCodeField = value; }
        }


        /// <summary>
        /// Gets or sets the value.
        /// The status code value. This attribute contains a URI reference. The value of the topmost
        /// &lt;StatusCode&gt; element MUST be from the top-level list provided in this section.
        /// </summary>
        /// <value>The value.</value>
        [XmlAttribute(DataType="anyURI")]
        public string Value
        {
            get { return valueField; }
            set { valueField = value; }
        }
    }
}