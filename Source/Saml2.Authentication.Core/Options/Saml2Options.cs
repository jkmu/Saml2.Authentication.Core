using Microsoft.AspNetCore.Authentication;
using Saml2.Authentication.Core.Authentication;

namespace Saml2.Authentication.Core.Options
{
    public class Saml2Options : AuthenticationSchemeOptions
    {
        public string AssertionConsumerServiceUrl { get; set; } = "Saml2/Authentication/AssertionConsumerService";

        public string SingleLogoutServiceUrl { get; set; } = "Saml2/Authentication/SingleLogoutService";

        public string SignInScheme { get; set; } = Saml2Defaults.SignInScheme;

        public string AuthenticationScheme { get; set; } = Saml2Defaults.AuthenticationScheme;
    }
}
