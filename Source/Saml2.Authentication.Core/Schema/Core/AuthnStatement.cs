namespace dk.nita.saml20.Schema.Core
{
    using System;
    using System.Xml.Serialization;
    using Utils;

    /// <summary>
    /// The &lt;AuthnStatement&gt; element describes a statement by the SAML authority asserting that the
    /// assertion subject was authenticated by a particular means at a particular time. Assertions containing
    /// &lt;AuthnStatement&gt; elements MUST contain a &lt;Subject&gt; element.
    /// </summary>
    [Serializable]
    [XmlType(Namespace=Saml2Constants.ASSERTION)]
    [XmlRoot(ELEMENT_NAME, Namespace=Saml2Constants.ASSERTION, IsNullable=false)]
    public class AuthnStatement : StatementAbstract
    {
        /// <summary>
        /// The XML Element name of this class
        /// </summary>
        public new const string ELEMENT_NAME = "AuthnStatement";

        private AuthnContext _authnContextField;

        private DateTime? _authnInstantField;

        private string _sessionIndexField;

        private DateTime? _sessionNotOnOrAfterField;

        private SubjectLocality _subjectLocalityField;
        
        /// <summary>
        /// Gets or sets the subject locality.
        /// Specifies the DNS domain name and IP address for the system from which the assertion subject was
        /// apparently authenticated.
        /// </summary>
        /// <value>The subject locality.</value>
        public SubjectLocality SubjectLocality
        {
            get => _subjectLocalityField;
            set => _subjectLocalityField = value;
        }

        /// <summary>
        /// Gets or sets the authn context.
        /// The context used by the authenticating authority up to and including the authentication event that
        /// yielded this statement. Contains an authentication context class reference, an authentication context
        /// declaration or declaration reference, or both. See the Authentication Context specification
        /// [SAMLAuthnCxt] for a full description of authentication context information.
        /// </summary>
        /// <value>The authn context.</value>
        public AuthnContext AuthnContext
        {
            get => _authnContextField;
            set => _authnContextField = value;
        }
        
        /// <summary>
        /// Gets or sets the authn instant.
        /// Specifies the time at which the authentication took place. The time value is encoded in UTC
        /// </summary>
        /// <value>The authn instant.</value>
        [XmlIgnore]
        public DateTime? AuthnInstant
        {
            get => _authnInstantField;
            set => _authnInstantField = value;
        }

        /// <summary>
        /// Gets or sets the authn instant string.
        /// </summary>
        /// <value>The authn instant string.</value>
        [XmlAttribute("AuthnInstant")]
        public string AuthnInstantString
        {
            get => _authnInstantField.HasValue ? Saml2Utils.ToUtcString(_authnInstantField.Value) : null;

            set => _authnInstantField = Saml2Utils.FromUtcString(value);
        }

        /// <summary>
        /// Gets or sets the index of the session.
        /// Specifies the index of a particular session between the principal identified by the subject and the
        /// authenticating authority.
        /// </summary>
        /// <value>The index of the session.</value>
        [XmlAttributeAttribute]
        public string SessionIndex
        {
            get { return _sessionIndexField; }
            set { _sessionIndexField = value; }
        }


        /// <summary>
        /// Gets or sets the session not on or after.
        /// Specifies a time instant at which the session between the principal identified by the subject and the
        /// SAML authority issuing this statement MUST be considered ended. The time value is encoded in
        /// UTC, as described in Section 1.3.3. There is no required relationship between this attribute and a
        /// NotOnOrAfter condition attribute that may be present in the assertion.
        /// </summary>
        /// <value>The session not on or after.</value>
        [XmlIgnore]
        public DateTime? SessionNotOnOrAfter
        {
            get => _sessionNotOnOrAfterField;
            set => _sessionNotOnOrAfterField = value;
        }

        /// <summary>
        /// Gets or sets the session not on or after string.
        /// </summary>
        /// <value>The session not on or after string.</value>
        [XmlAttribute("SessionNotOnOrAfter")]
        public string SessionNotOnOrAfterString
        {
            get => _sessionNotOnOrAfterField.HasValue ? Saml2Utils.ToUtcString(_sessionNotOnOrAfterField.Value) : null;

            set =>
                _sessionNotOnOrAfterField = string.IsNullOrEmpty(value) ? (DateTime?) null : Saml2Utils.FromUtcString(value);
        }
    }
}