using Microsoft.AspNetCore.Authentication;

namespace Saml2.Authentication.Core.Authentication
{
    public class SamlAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string AssertionConsumerServiceUrl { get; set; } = "Saml2/Authentication/AssertionConsumerService";

        public string SingleLogoutServiceUrl { get; set; } = "Saml2/Authentication/SingleLogoutService";

        public string SignInScheme { get; set; } = SamlAuthenticationDefaults.SignInScheme;

        public string AuthenticationScheme { get; set; } = SamlAuthenticationDefaults.AuthenticationScheme;
    }
}
