namespace dk.nita.saml20
{
    /// <summary>
    ///     Constants related to SAML 2.0
    /// </summary>
    public class Saml2Constants
    {
        /// <summary>
        ///     SAML Version
        /// </summary>
        public const string Version = "2.0";

        /// <summary>
        ///     The XML namespace of the SAML 2.0 assertion schema.
        /// </summary>
        public const string ASSERTION = "urn:oasis:names:tc:SAML:2.0:assertion";

        /// <summary>
        ///     The XML namespace of the SAML 2.0 protocol schema
        /// </summary>
        public const string PROTOCOL = "urn:oasis:names:tc:SAML:2.0:protocol";

        /// <summary>
        ///     The XML namespace of the SAML 2.0 metadata schema
        /// </summary>
        public const string METADATA = "urn:oasis:names:tc:SAML:2.0:metadata";

        /// <summary>
        ///     The XML namespace of XmlDSig
        /// </summary>
        public const string XMLDSIG = "http://www.w3.org/2000/09/xmldsig#";

        /// <summary>
        ///     The XML namespace of XmlEnc
        /// </summary>
        public const string XENC = "http://www.w3.org/2001/04/xmlenc#";

        /// <summary>
        ///     The default value of the Format property for a NameID element
        /// </summary>
        public const string DEFAULTNAMEIDFORMAT = "urn:oasis:names:tc:SAML:1.0:nameid-format:unspecified";

        /// <summary>
        ///     The mime type that must be used when publishing a metadata document.
        /// </summary>
        public const string METADATA_MIMETYPE = "application/samlmetadata+xml";

        /// <summary>
        ///     A mandatory prefix for translating arbitrary saml2.0 claim names to saml1.1 attributes
        /// </summary>
        public const string DKSAML20_CLAIMTYPE_PREFIX = "dksaml20/";

        /// <summary>
        ///     Represents the Uniform Resource Identifier (URI) for the RSA SHA-256 signature method variation for XML digital
        ///     signatures.
        /// </summary>
        public const string XmlDsigRSASHA256Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

        /// <summary>
        ///     Represents the Uniform Resource Identifier (URI) for the RSA SHA-512 signature method variation for XML digital
        ///     signatures.
        /// </summary>
        public const string XmlDsigRSASHA512Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

        /// <summary>
        ///     All the namespaces defined and reserved by the SAML 2.0 standard
        /// </summary>
        public static readonly string[] SAML_NAMESPACES = {ASSERTION, PROTOCOL, METADATA};

        /// <summary>
        ///     Formats of nameidentifiers
        /// </summary>
        public static class NameIdentifierFormats
        {
            /// <summary>
            ///     urn for Unspecified name identifier format
            /// </summary>
            public const string Unspecified = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified";

            /// <summary>
            ///     urn for Email name identifier format
            /// </summary>
            public const string Email = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress";

            /// <summary>
            ///     urn for X509SubjectName name identifier format
            /// </summary>
            public const string X509SubjectName = "urn:oasis:names:tc:SAML:1.1:nameid-format:X509SubjectName";

            /// <summary>
            ///     urn for Windows name identifier format
            /// </summary>
            public const string Windows = "urn:oasis:names:tc:SAML:1.1:nameid-format:WindowsDomainQualifiedName";

            /// <summary>
            ///     urn for Kerberos name identifier format
            /// </summary>
            public const string Kerberos = "urn:oasis:names:tc:SAML:2.0:nameid-format:kerberos";

            /// <summary>
            ///     urn for Entity name identifier format
            /// </summary>
            public const string Entity = "urn:oasis:names:tc:SAML:2.0:nameid-format:entity";

            /// <summary>
            ///     urn for Persistent name identifier format
            /// </summary>
            public const string Persistent = "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent";

            /// <summary>
            ///     urn for Transient name identifier format
            /// </summary>
            public const string Transient = "urn:oasis:names:tc:SAML:2.0:nameid-format:transient";
        }

        /// <summary>
        ///     Protocol bindings
        /// </summary>
        public static class ProtocolBindings
        {
            /// <summary>
            ///     HTTP Redirect protocol binding
            /// </summary>
            public const string HTTP_Redirect = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect";

            /// <summary>
            ///     HTTP Post protocol binding
            /// </summary>
            public const string HTTP_Post = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST";

            /// <summary>
            ///     HTTP Artifact protocol binding
            /// </summary>
            public const string HTTP_Artifact = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Artifact";

            /// <summary>
            ///     HTTP SOAP  protocol binding
            /// </summary>
            public const string HTTP_SOAP = "urn:oasis:names:tc:SAML:2.0:bindings:SOAP";
        }

        /// <summary>
        ///     Subject confirmation methods
        /// </summary>
        public static class SubjectConfirmationMethods
        {
            /// <summary>
            ///     Holder of key confirmation method
            /// </summary>
            public const string HolderOfKey = "urn:oasis:names:tc:SAML:2.0:cm:holder-of-key";
        }

        /// <summary>
        ///     Logout reasons
        /// </summary>
        public static class Reasons
        {
            /// <summary>
            ///     Specifies that the message is being sent because the principal wishes to terminate the indicated session.
            /// </summary>
            public const string User = "urn:oasis:names:tc:SAML:2.0:logout:user";

            /// <summary>
            ///     Specifies that the message is being sent because an administrator wishes to terminate the indicated
            ///     session for that principal.
            /// </summary>
            public const string Admin = "urn:oasis:names:tc:SAML:2.0:logout:admin";
        }

        /// <summary>
        ///     Status codes
        /// </summary>
        public static class StatusCodes
        {
            /// <summary>
            ///     The request succeeded.
            /// </summary>
            public const string Success = "urn:oasis:names:tc:SAML:2.0:status:Success";

            /// <summary>
            ///     The request could not be performed due to an error on the part of the requester.
            /// </summary>
            public const string Requester = "urn:oasis:names:tc:SAML:2.0:status:Requester";

            /// <summary>
            ///     The request could not be performed due to an error on the part of the SAML responder or SAML authority.
            /// </summary>
            public const string Responder = "urn:oasis:names:tc:SAML:2.0:status:Responder";

            /// <summary>
            ///     The SAML responder could not process the request because the version of the request message was incorrect.
            /// </summary>
            public const string VersionMismatch = "urn:oasis:names:tc:SAML:2.0:status:VersionMismatch";

            /// <summary>
            ///     The responding provider was unable to successfully authenticate the principal.
            /// </summary>
            public const string AuthnFailed = "urn:oasis:names:tc:SAML:2.0:status:AuthnFailed";

            /// <summary>
            ///     Unexpected or invalid content was encountered within a &lt;saml:Attribute&gt; or &lt;saml:AttributeValue&gt;
            ///     element.
            /// </summary>
            public const string InvalidAttrNameOrValue = "urn:oasis:names:tc:SAML:2.0:status:InvalidAttrNameOrValue";

            /// <summary>
            ///     The responding provider cannot or will not support the requested name identifier policy.
            /// </summary>
            public const string InvalidNameIdPolicy = "urn:oasis:names:tc:SAML:2.0:status:InvalidNameIDPolicy";

            /// <summary>
            ///     The specified authentication context requirements cannot be met by the responder.
            /// </summary>
            public const string NoAuthnContext = "urn:oasis:names:tc:SAML:2.0:status:NoAuthnContext";

            /// <summary>
            ///     Used by an intermediary to indicate that none of the supported identity provider &lt;Loc&gt; elements in an
            ///     &lt;IDPList&gt; can be resolved or that none of the supported identity providers are available.
            /// </summary>
            public const string NoAvailableIDP = "urn:oasis:names:tc:SAML:2.0:status:NoAvailableIDP";

            /// <summary>
            ///     Indicates the responding provider cannot authenticate the principal passively, as has been requested.
            /// </summary>
            public const string NoPassive = "urn:oasis:names:tc:SAML:2.0:status:NoPassive";

            /// <summary>
            ///     Used by an intermediary to indicate that none of the identity providers in an &lt;IDPList&gt; are
            ///     supported by the intermediary.
            /// </summary>
            public const string NoSupportedIDP = "urn:oasis:names:tc:SAML:2.0:status:NoSupportedIDP";

            /// <summary>
            ///     Used by a session authority to indicate to a session participant that it was not able to propagate logout
            ///     to all other session participants.
            /// </summary>
            public const string PartialLogout = "urn:oasis:names:tc:SAML:2.0:status:PartialLogout";

            /// <summary>
            ///     Indicates that a responding provider cannot authenticate the principal directly and is not permitted to
            ///     proxy the request further.
            /// </summary>
            public const string ProxyCountExceeded = "urn:oasis:names:tc:SAML:2.0:status:ProxyCountExceeded";

            /// <summary>
            ///     The SAML responder or SAML authority is able to process the request but has chosen not to respond.
            ///     This status code MAY be used when there is concern about the security context of the request
            ///     message or the sequence of request messages received from a particular requester.
            /// </summary>
            public const string RequestDenied = "urn:oasis:names:tc:SAML:2.0:status:RequestDenied";

            /// <summary>
            ///     The SAML responder or SAML authority does not support the request.
            /// </summary>
            public const string RequestUnsupported = "urn:oasis:names:tc:SAML:2.0:status:RequestUnsupported";

            /// <summary>
            ///     The SAML responder cannot process any requests with the protocol version specified in the request.
            /// </summary>
            public const string RequestVersionDeprecated =
                "urn:oasis:names:tc:SAML:2.0:status:RequestVersionDeprecated";

            /// <summary>
            ///     The SAML responder cannot process the request because the protocol version specified in the
            ///     request message is a major upgrade from the highest protocol version supported by the responder.
            /// </summary>
            public const string RequestVersionTooHigh = "urn:oasis:names:tc:SAML:2.0:status:RequestVersionTooHigh";

            /// <summary>
            ///     The SAML responder cannot process the request because the protocol version specified in the
            ///     request message is too low.
            /// </summary>
            public const string RequestVersionTooLow = "urn:oasis:names:tc:SAML:2.0:status:RequestVersionTooLow";

            /// <summary>
            ///     The resource value provided in the request message is invalid or unrecognized.
            /// </summary>
            public const string ResourceNotRecognized = "urn:oasis:names:tc:SAML:2.0:status:ResourceNotRecognized";

            /// <summary>
            ///     The response message would contain more elements than the SAML responder is able to return.
            /// </summary>
            public const string TooManyResponses = "urn:oasis:names:tc:SAML:2.0:status:TooManyResponses";

            /// <summary>
            ///     An entity that has no knowledge of a particular attribute profile has been presented with an attribute
            ///     drawn from that profile.
            /// </summary>
            public const string UnknownAttrProfile = "urn:oasis:names:tc:SAML:2.0:status:UnknownAttrProfile";

            /// <summary>
            ///     The responding provider does not recognize the principal specified or implied by the request.
            /// </summary>
            public const string UnknownPrincipal = "urn:oasis:names:tc:SAML:2.0:status:UnknownPrincipal";

            /// <summary>
            ///     The SAML responder cannot properly fulfill the request using the protocol binding specified in the
            ///     request.
            /// </summary>
            public const string UnsupportedBinding = "urn:oasis:names:tc:SAML:2.0:status:UnsupportedBinding";
        }
    }

    /// <summary>
    ///     SAML Actions
    /// </summary>
    public enum SAMLAction
    {
        /// <summary>
        ///     Request
        /// </summary>
        SAMLRequest,

        /// <summary>
        ///     Response
        /// </summary>
        SAMLResponse
    }
}