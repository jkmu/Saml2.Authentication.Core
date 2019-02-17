using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Bindings;
using Saml2.Authentication.Core.Bindings.SignatureProviders;
using Saml2.Authentication.Core.Extensions;
using Saml2.Authentication.Core.Factories;
using Saml2.Authentication.Core.Options;
using Saml2.Authentication.Core.Providers;
using Saml2.Authentication.Core.Services;
using Saml2.Authentication.Core.Session;
using Saml2.Authentication.Core.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Saml2ServiceCollectionExtensions
    {
        public static IServiceCollection AddSaml(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddRequiredServices();
            return services;
        }

        private static void AddRequiredServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<Saml2Configuration>>().Value);

            services.TryAddTransient<ISaml2Validator, Saml2Validator>();
            services.TryAddTransient<ISaml2ClaimFactory, Saml2ClaimFactory>();
            services.TryAddTransient<ISamlProvider, SamlProvider>();
            services.TryAddTransient<ISaml2MessageFactory, Saml2MessageFactory>();
            services.TryAddTransient<ISignatureProviderFactory, SignatureProviderFactory>();
            services.TryAddTransient<IHttpRedirectBinding, HttpRedirectBinding>();
            services.TryAddTransient<IHttpArtifactBinding, HttpArtifactBinding>();
            services.TryAddTransient<ISamlService, SamlService>();
            services.TryAddTransient<ISessionStore, CookieSessionStorage>();
        }

        /// <summary>
        ///     Add signing certificates by thumpprint
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceProviderCertificateThumbprint">ServiceProvider's certificate thumbprint</param>
        /// <param name="identityProviderCertificateThumbprint">IdentityProvider's certificate thumbprint</param>
        /// <returns></returns>
        public static IServiceCollection AddSigningCertificates(this IServiceCollection services,
            string serviceProviderCertificateThumbprint, string identityProviderCertificateThumbprint)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return AddSigningCertificates(services,
                GetSigningCertificates(X509FindType.FindByThumbprint,
                    serviceProviderCertificateThumbprint.TrimSpecialCharacters(),
                    identityProviderCertificateThumbprint.TrimSpecialCharacters()));
        }

        public static IServiceCollection AddSigningCertificates(this IServiceCollection services, X509FindType findType,
            string serviceProviderCertificateName, string identityProviderCertificateName)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return AddSigningCertificates(services,
                GetSigningCertificates(findType, identityProviderCertificateName, serviceProviderCertificateName));
        }

        /// <summary>
        ///     Add signing certificates from file
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceProviderCertificateFileName">ServiceProvider's certificate filename</param>
        /// <param name="x509KeyStorageFlag"></param>
        /// <param name="identityProviderCertificateFileName">IdentityProvider's certificate filename</param>
        /// <param name="serviceProviderCertificatePassword"></param>
        /// <returns></returns>
        public static IServiceCollection AddSigningCertificatesFromFile(this IServiceCollection services,
            string serviceProviderCertificateFileName, string serviceProviderCertificatePassword, X509KeyStorageFlags x509KeyStorageFlag, string identityProviderCertificateFileName)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (string.IsNullOrEmpty(serviceProviderCertificateFileName))
                throw new ArgumentNullException(nameof(serviceProviderCertificateFileName));

            if (string.IsNullOrEmpty(identityProviderCertificateFileName))
                throw new ArgumentNullException(nameof(identityProviderCertificateFileName));

            var serviceProviderCertificateFullFileName = !Path.IsPathRooted(serviceProviderCertificateFileName)
                ? Path.Combine(Directory.GetCurrentDirectory(), serviceProviderCertificateFileName)
                : serviceProviderCertificateFileName;
            var serviceProviderCertificate = new X509Certificate2(serviceProviderCertificateFullFileName,
                serviceProviderCertificatePassword,
                x509KeyStorageFlag);

            var identityProviderCertificateFullFileName = !Path.IsPathRooted(identityProviderCertificateFileName)
                ? Path.Combine(Directory.GetCurrentDirectory(), identityProviderCertificateFileName)
                : identityProviderCertificateFileName;
            var identityProviderCertificate = new X509Certificate2(identityProviderCertificateFullFileName);

            return AddSigningCertificates(services,
                new SigningCertificate(identityProviderCertificate, serviceProviderCertificate));
        }

        private static X509Certificate2 GetCertificate(string findValue, X509FindType findType,
            StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine,
            bool validOnly = false)
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

        private static string SearchDescriptor(string findValue, X509FindType findType, StoreName storeName,
            StoreLocation storeLocation, bool validOnly)
        {
            var message =
                $"The certificate was searched for in {storeLocation}/{storeName}, {findType}='{findValue}', validOnly={validOnly}.";
            if (findType == X509FindType.FindByThumbprint && findValue?.Length > 0 && findValue[0] == 0x200E)
                message =
                    "\nThe configuration for the certificate searches by thumbprint but has an invalid character in the thumbprint string. Make sure you remove the first hidden character in the thumbprint value in the configuration. See https://support.microsoft.com/en-us/help/2023835/certificate-thumbprint-displayed-in-mmc-certificate-snap-in-has-extra-invisible-unicode-character. \n" +
                    message;

            return message;
        }

        private static SigningCertificate GetSigningCertificates(X509FindType findType,
            string serviceProviderCertificateName, string identityProviderCertificateName)
        {
            if (string.IsNullOrEmpty(serviceProviderCertificateName))
                throw new ArgumentNullException(nameof(serviceProviderCertificateName));

            if (string.IsNullOrEmpty(identityProviderCertificateName))
                throw new ArgumentNullException(nameof(identityProviderCertificateName));

            var serviceProviderCertificate = GetCertificate(serviceProviderCertificateName, findType);
            var identityProviderCertificate = GetCertificate(identityProviderCertificateName, findType);
            var certificates = new SigningCertificate(identityProviderCertificate, serviceProviderCertificate);
            return certificates;
        }

        private static void CheckServiceProviderCetificatePrivateKey(SigningCertificate signingCertificates)
        {
            if (signingCertificates == null)
                throw new ArgumentNullException(nameof(signingCertificates));

            if (!signingCertificates.ServiceProvider.HasPrivateKey)
                throw new InvalidOperationException("Certificate does not have a private key.");
        }

        private static IServiceCollection AddSigningCertificates(IServiceCollection services,
            SigningCertificate signingCertificates)
        {
            CheckServiceProviderCetificatePrivateKey(signingCertificates);
            services.AddSingleton<ICertificateProvider>(new CertificateProvider(signingCertificates));
            return services;
        }
    }
}