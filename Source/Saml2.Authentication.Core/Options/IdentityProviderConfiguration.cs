using System.ComponentModel;

namespace Saml2.Authentication.Core.Options
{
    public class IdentityProviderConfiguration
    {
        public string EntityId { get; set; }

        public string Name { get; set; }

        public string IssuerFormat { get; set; }

        [DefaultValue("urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified")]
        public string NameIdPolicyFormat { get; set; } = "urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified";

        [DefaultValue("http://www.w3.org/2001/04/xmlenc#sha256")]
        public string DigestAlgorithm { get; set; } = "SHA256";

        [DefaultValue("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256")]
        public string HashingAlgorithm { get; set; } = "SHA256";

        [DefaultValue("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect")]
        public string ProtocolBinding { get; set; } = "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect";

        public string SingleSignOnService { get; set; }

        public string SingleSignOutService { get; set; }

        public string ArtifactResolveService { get; set; }
    }
}