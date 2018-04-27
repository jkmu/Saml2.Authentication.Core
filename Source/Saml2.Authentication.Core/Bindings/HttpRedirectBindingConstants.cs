namespace dk.nita.saml20.Bindings
{
    /// <summary>
    ///     Contains constant string versions of the parameters in the HTTP Redirect Binding.
    ///     Introduced to avoid errors caused by stupid errors in capitalization when using the binding.
    /// </summary>
    public class HttpRedirectBindingConstants
    {
        /// <summary>
        ///     SAMLResponse name
        /// </summary>
        public const string SamlResponse = "SAMLResponse";

        /// <summary>
        ///     SAMLRequest name
        /// </summary>
        public const string SamlRequest = "SAMLRequest";

        /// <summary>
        ///     SigAlg name
        /// </summary>
        public const string SigAlg = "SigAlg";

        /// <summary>
        ///     Relaystate name
        /// </summary>
        public const string RelayState = "RelayState";

        /// <summary>
        ///     Signature name
        /// </summary>
        public const string Signature = "Signature";
    }
}