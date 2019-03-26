namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Extensions;
    using Options;
    using Saml2.Authentication.Core.Bindings;
    using Saml2.Authentication.Core.Bindings.SignatureProviders;
    using Saml2.Authentication.Core.Configuration;
    using Saml2.Authentication.Core.Factories;
    using Saml2.Authentication.Core.Providers;
    using Saml2.Authentication.Core.Services;
    using Saml2.Authentication.Core.Session;
    using Saml2.Authentication.Core.Validation;

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

        private static void AddRequiredServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.TryAddSingleton(resolver => resolver.GetRequiredService<IOptions<Saml2Configuration>>().Value);
            services.TryAddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.TryAddTransient<ISamlValidator, SamlValidator>();
            services.TryAddTransient<ISamlClaimFactory, SamlClaimFactory>();
            services.TryAddTransient<ISamlXmlProvider, SamlXmlProvider>();
            services.TryAddTransient<ISamlMessageFactory, SamlMessageFactory>();
            services.TryAddTransient<ISignatureProviderFactory, SignatureProviderFactory>();
            services.TryAddTransient<IHttpRedirectBinding, HttpRedirectBinding>();
            services.TryAddTransient<IHttpArtifactBinding, HttpArtifactBinding>();
            services.TryAddTransient<ISamlService, SamlService>();
            services.TryAddTransient<ISessionStore, CookieSessionStorage>();
        }
    }
}