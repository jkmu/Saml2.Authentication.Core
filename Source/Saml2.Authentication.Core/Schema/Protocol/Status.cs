using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The saml protocol status class.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class Status
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public const string ELEMENT_NAME = "Status";

        private StatusCode statusCodeField;

        private StatusDetail statusDetailField;
        private string statusMessageField;


        /// <summary>
        /// Gets or sets the status code.
        /// A code representing the status of the activity carried out in response to the corresponding request.
        /// </summary>
        /// <value>The status code.</value>
        public StatusCode StatusCode
        {
            get { return statusCodeField; }
            set { statusCodeField = value; }
        }


        /// <summary>
        /// Gets or sets the status message.
        /// A message which MAY be returned to an operator.
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage
        {
            get { return statusMessageField; }
            set { statusMessageField = value; }
        }


        /// <summary>
        /// Gets or sets the status detail.
        /// Additional information concerning the status of the request.
        /// </summary>
        /// <value>The status detail.</value>
        public StatusDetail StatusDetail
        {
            get { return statusDetailField; }
            set { statusDetailField = value; }
        }
    }
}