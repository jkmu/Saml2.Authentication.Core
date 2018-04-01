using System;
using System.Xml.Serialization;

namespace dk.nita.saml20.Schema.Protocol
{
    /// <summary>
    /// The &lt;AuthnQuery&gt; message element is used to make the query "What assertions containing
    /// authentication statements are available for this subject?" A successful &lt;Response&gt; will contain one or
    /// more assertions containing authentication statements.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.PROTOCOL)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.PROTOCOL, IsNullable=false)]
    public class AuthnQuery : SubjectQueryAbstract
    {

        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AuthnQuery";

        private RequestedAuthnContext requestedAuthnContextField;

        private string sessionIndexField;


        /// <summary>
        /// Gets or sets the requested authn context.
        /// If present, specifies a filter for possible responses. Such a query asks the question "What assertions
        /// containing authentication statements do you have for this subject that satisfy the authentication
        /// context requirements in this element?"
        /// </summary>
        /// <value>The requested authn context.</value>
        public RequestedAuthnContext RequestedAuthnContext
        {
            get { return requestedAuthnContextField; }
            set { requestedAuthnContextField = value; }
        }

        /// <summary>
        /// Gets or sets the index of the session.
        /// If present, specifies a filter for possible responses. Such a query asks the question "What assertions
        /// containing authentication statements do you have for this subject within the context of the supplied
        /// session information?"
        /// </summary>
        /// <value>The index of the session.</value>
        [XmlAttribute]
        public string SessionIndex
        {
            get { return sessionIndexField; }
            set { sessionIndexField = value; }
        }
    }
}