namespace Saml2.Authentication.Core.Providers
{
    using System.Security.Cryptography.X509Certificates;
    using Configuration;

    public interface IConfigurationProvider
    {
        ServiceProviderConfiguration ServiceProviderConfiguration { get; }

        IdentityProviderConfiguration GetIdentityProviderConfiguration(string providerName);

        X509Certificate2 GetIdentityProviderSigningCertificate(string providerName);

        X509Certificate2 ServiceProviderSigningCertificate();
    }
}