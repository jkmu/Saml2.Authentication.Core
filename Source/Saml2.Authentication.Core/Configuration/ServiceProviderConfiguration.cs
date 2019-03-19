namespace Saml2.Authentication.Core.Configuration
{
    public class ServiceProviderConfiguration
    {
        public string EntityId { get; set; }

        public bool OmitAssertionSignatureCheck { get; set; }

        public string AssertionConsumerServiceUrl { get; set; } = "Saml2/AssertionConsumerService";

        public string SingleLogoutServiceUrl { get; set; } = "Saml2/SingleLogoutService";

        public string SingleLogoutResponseServiceUrl { get; set; } = "Saml2/SingleLogoutService";

        public Certificate Certificate { get; set; }
    }
}
