using System.Security.Cryptography.X509Certificates;

namespace Saml2.Authentication.Core.Providers
{
    internal class CertificateProvider : ICertificateProvider
    {
        private readonly SigningCertificate _signingCertificate;

        public CertificateProvider(SigningCertificate signingCertificate)
        {
            _signingCertificate = signingCertificate;
        }

        public SigningCertificate GetCertificate()
        {
            return _signingCertificate;
        }
    }

    public class SigningCertificate
    {
        public SigningCertificate(
            X509Certificate2 identityProvider,
            X509Certificate2 serviceProvider)
        {
            IdentityProvider = identityProvider;
            ServiceProvider = serviceProvider;
        }

        public X509Certificate2 IdentityProvider { get; }

        public X509Certificate2 ServiceProvider { get; }
    }
}