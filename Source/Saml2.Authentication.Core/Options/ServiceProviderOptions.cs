namespace Saml2.Authentication.Core.Options
{
    public class ServiceProviderOptions
    {
        public const string SectionName = "ServiceProvider";

        public string Id { get; set; }

        public bool ForceAuth { get; set; }

        public bool IsPassive { get; set; }

        public string Organization { get; set; }

        public string SigningCertificateThumprint { get; set; }
    }
}
