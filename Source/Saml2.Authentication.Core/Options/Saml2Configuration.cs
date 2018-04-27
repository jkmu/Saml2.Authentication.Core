namespace Saml2.Authentication.Core.Options
{
    public class Saml2Configuration
    {
        public bool ForceAuth { get; set; }

        public bool IsPassive { get; set; }

        public bool SignAuthnRequest { get; set; }

        public string AuthnContextComparisonType { get; set; }

        public string[] AuthnContextComparisonItems { get; set; }

        public bool? AllowCreate { get; set; }

        public bool OmitAssertionSignatureCheck { get; set; }

        public IdentityProviderConfiguration IdentityProviderConfiguration { get; set; }

        public ServiceProviderConfiguration ServiceProviderConfiguration { get; set; }
    }
}