using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Authentication;
using Saml2.Authentication.Core.Bindings;
using Saml2.Authentication.Core.Bindings.SignatureProviders;
using Saml2.Authentication.Core.Factories;
using Saml2.Authentication.Core.Options;
using Saml2.Authentication.Core.Providers;
using Saml2.Authentication.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Saml2ServiceCollectionExtensions
    {

        public static IServiceCollection AddSaml(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddRequiredServices();
            return services;
        }

        public static IServiceCollection AddSigningCertificates(this IServiceCollection services, string serviceProviderCertificateThumpprint, string identityProviderCertificateThumpprint)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(serviceProviderCertificateThumpprint))
            {
                throw new ArgumentNullException(nameof(serviceProviderCertificateThumpprint));
            }

            if (string.IsNullOrEmpty(identityProviderCertificateThumpprint))
            {
                throw new ArgumentNullException(nameof(identityProviderCertificateThumpprint));
            }

            var serviceProviderCertificate = GetCertificate(serviceProviderCertificateThumpprint);
            var identityProviderCertificate = GetCertificate(identityProviderCertificateThumpprint);
            var certificates = new SigningCertificate(identityProviderCertificate, serviceProviderCertificate);

            services.AddSingleton<ICertificateProvider>(new CertificateProvider(certificates));
            return services;
        }

        private static void AddRequiredServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<Saml2Options>>().Value);
            services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<IdentityProviderOptions>>().Value);
            services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<ServiceProviderOptions>>().Value);

            
            services.TryAddTransient<ISamlMessageFactory, SamlMessageFactory>();
            services.TryAddTransient<ISignatureProviderFactory, SignatureProviderFactory>();
            services.TryAddTransient<IHttpRedirectBinding, HttpRedirectBinding>();
            services.TryAddTransient<ISamlService, SamlService>();
        }

        private static X509Certificate2 GetCertificate(string findValue, X509FindType findType = X509FindType.FindByThumbprint, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine, bool validOnly = true)
        {
            var store = new X509Store(storeName, storeLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var found = store.Certificates.Find(findType, findValue, validOnly);
                if (found.Count == 0)
                {
                    var searchDescriptor = SearchDescriptor(findValue, findType, storeName, storeLocation, validOnly);
                    var msg = $"A configured certificate could not be found in the certificate store. {searchDescriptor}";
                    throw new Exception(msg);
                }
                if (found.Count > 1)
                {
                    var searchDescriptor = SearchDescriptor(findValue, findType, storeName, storeLocation, validOnly);
                    var msg = $"Found more than one certificate in the certificate store. Make sure you don't have duplicate certificates installed. {searchDescriptor}";
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
            var message = $"The certificate was searched for in {storeLocation}/{storeName}, {findType}='{findValue}', validOnly={validOnly}.";
            if (findType == X509FindType.FindByThumbprint && findValue?.Length > 0 && findValue[0] == 0x200E)
            {
                message = "\nThe configuration for the certificate searches by thumbprint but has an invalid character in the thumbprint string. Make sure you remove the first hidden character in the thumbprint value in the configuration. See https://support.microsoft.com/en-us/help/2023835/certificate-thumbprint-displayed-in-mmc-certificate-snap-in-has-extra-invisible-unicode-character. \n" + message;
            }

            return message;
        }
    }
}
