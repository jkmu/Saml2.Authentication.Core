namespace Saml2.Authentication.Core.Providers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Configuration;
    using Extensions;

    internal class ConfigurationProvider : IConfigurationProvider
    {
        private readonly Saml2Configuration _configuration;

        public ConfigurationProvider(Saml2Configuration configuration)
        {
            _configuration = configuration;
        }

        public ServiceProviderConfiguration ServiceProviderConfiguration => _configuration.ServiceProviderConfiguration;

        public IdentityProviderConfiguration GetIdentityProviderConfiguration(string providerName)
        {
            if (providerName.IsNullOrEmpty())
            {
                return _configuration.IdentityProviderConfiguration.FirstOrDefault();
            }

            return _configuration.IdentityProviderConfiguration.SingleOrDefault(c =>
                c.Name == providerName);
        }

        public X509Certificate2 GetIdentityProviderSigningCertificate(string providerName)
        {
            var certificateDetails = GetIdentityProviderConfiguration(providerName).Certificate;

            var certificate = LoadCertificate(certificateDetails);
            if (certificate == null)
            {
                throw new InvalidOperationException("Missing IdentityProvider certificate");
            }

            return certificate;
        }

        public X509Certificate2 ServiceProviderSigningCertificate()
        {
            var certificateDetails = ServiceProviderConfiguration.Certificate;

            var certificate = LoadCertificate(certificateDetails);
            if (certificate == null)
            {
                throw new InvalidOperationException("Missing ServiceProvider certificate");
            }

            CheckPrivateKey(certificate);
            return certificate;
        }

        private X509Certificate2 LoadCertificate(Certificate certificateDetails) =>
            certificateDetails.Thumbprint.IsNotNullOrEmpty()
                ? FindCertificate(
                    certificateDetails.Thumbprint,
                    X509FindType.FindByThumbprint,
                    certificateDetails.GetStoreName(),
                    certificateDetails.GetStoreLocation())
                : LoadCertificateFromFile(
                    certificateDetails.FileName,
                    certificateDetails.Password,
                    certificateDetails.GetKeyStorageFlags());

        private X509Certificate2 LoadCertificateFromFile(
            string filename,
            string password,
            X509KeyStorageFlags flags = X509KeyStorageFlags.PersistKeySet)
        {
            var fullFileName = !Path.IsPathRooted(filename)
                ? Path.Combine(Directory.GetCurrentDirectory(), filename)
                : filename;

            return new X509Certificate2(
                fullFileName,
                password,
                flags);
        }

        private static X509Certificate2 FindCertificate(string findValue, X509FindType findType, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine, bool validOnly = false)
        {
            var store = new X509Store(storeName, storeLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var found = store.Certificates.Find(findType, findValue, validOnly);
                if (found.Count == 0)
                {
                    var searchDescriptor = SearchDescriptor(findValue, findType, storeName, storeLocation, validOnly);
                    var msg =
                        $"A configured certificate could not be found in the certificate store. {searchDescriptor}";
                    throw new Exception(msg);
                }

                if (found.Count > 1)
                {
                    var searchDescriptor = SearchDescriptor(findValue, findType, storeName, storeLocation, validOnly);
                    var msg =
                        $"Found more than one certificate in the certificate store. Make sure you don't have duplicate certificates installed. {searchDescriptor}";
                    throw new Exception(msg);
                }

                return found[0];
            }
            finally
            {
                store.Close();
            }
        }

        private static string SearchDescriptor(string findValue, X509FindType findType, StoreName storeName, StoreLocation storeLocation, bool validOnly)
        {
            var message =
                $"The certificate was searched for in {storeLocation}/{storeName}, {findType}='{findValue}', validOnly={validOnly}.";
            if (findType == X509FindType.FindByThumbprint && findValue?.Length > 0 && findValue[0] == 0x200E)
            {
                message =
                    "\nThe configuration for the certificate searches by thumbprint but has an invalid character in the thumbprint string. Make sure you remove the first hidden character in the thumbprint value in the configuration. See https://support.microsoft.com/en-us/help/2023835/certificate-thumbprint-displayed-in-mmc-certificate-snap-in-has-extra-invisible-unicode-character. \n" +
                    message;
            }

            return message;
        }

        private static void CheckPrivateKey(X509Certificate2 x509Certificate)
        {
            if (!x509Certificate.HasPrivateKey)
            {
                throw new InvalidOperationException($"Certificate with thumbprint {x509Certificate.Thumbprint} does not have a private key.");
            }
        }
    }
}
