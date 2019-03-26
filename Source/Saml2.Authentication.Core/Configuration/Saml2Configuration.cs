namespace Saml2.Authentication.Core.Configuration
{
    using System.Collections.Generic;

    public class Saml2Configuration
    {
        public ServiceProviderConfiguration ServiceProviderConfiguration { get; set; }

        public IList<IdentityProviderConfiguration> IdentityProviderConfiguration { get; set; }
    }
}