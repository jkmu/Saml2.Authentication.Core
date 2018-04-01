namespace Saml2.Authentication.Core.Options
{
    public class ServiceProviderConfiguration
    {
        public string Id { get; set; }

        public bool ForceAuth { get; set; }

        public bool IsPassive { get; set; }

        public string SigningCertificateThumprint { get; set; }
    }
}
