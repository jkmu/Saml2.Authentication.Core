using System;
using Microsoft.AspNetCore.Authentication;
using Saml2.Authentication.Core.Authentication;
    
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SamlAuthenticationExtensions
    {
        public static AuthenticationBuilder AddSaml(this AuthenticationBuilder builder)
          => builder.AddSaml(SamlAuthenticationDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddSaml(this AuthenticationBuilder builder, Action<SamlAuthenticationOptions> configureOptions)
        => builder.AddSaml(SamlAuthenticationDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddSaml(this AuthenticationBuilder builder, string authenticationScheme, Action<SamlAuthenticationOptions> configureOptions)
         => builder.AddSaml(authenticationScheme, SamlAuthenticationDefaults.AuthenticationSchemeDisplayName, configureOptions);

        public static AuthenticationBuilder AddSaml(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<SamlAuthenticationOptions> configureOptions)
        => builder.AddScheme<SamlAuthenticationOptions, SamlAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}
