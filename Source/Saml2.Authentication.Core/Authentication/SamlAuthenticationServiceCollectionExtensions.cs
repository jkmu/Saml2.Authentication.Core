using System;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SamlAuthenticationServiceCollectionExtensions
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

        public static IServiceCollection AddRequiredServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<SamlAuthenticationOptions>>().Value);

            //services.AddTransient<Service, IService>();
            return services;
        }
    }
}
