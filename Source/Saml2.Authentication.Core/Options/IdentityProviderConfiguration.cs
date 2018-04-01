using System.ComponentModel;

namespace Saml2.Authentication.Core.Options
{
    public class IdentityProviderConfiguration
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string IssuerFormat { get; set; }

        [DefaultValue("urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified")]
        public string NameIdPolicyFormat { get; set; } = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified";

        public bool? AllowCreate { get; set; }

        [DefaultValue("http://www.w3.org/2001/04/xmlenc#sha256")]
        public string DigestAlgorithm { get; set; } = "SHA256";

        [DefaultValue("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256")]
        public string HashingAlgorithm { get; set; } = "SHA256";

        public string ProtocolBinding { get; set; }

        public string SingleSignOnService { get; set; }

        public string SingleSignOutService { get; set; }

        public string ArtifactResolveService { get; set; }

        public string SigningCertificateThumprint { get; set; }

        public string AuthnContextComparisonType { get; set; }

        public string[] AuthnContextComparisonItems { get; set; }

        public bool OmitAssertionSignatureCheck { get; set; }

        public bool SignAuthnRequest { get; set; }
    }
}
