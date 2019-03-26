namespace Saml2.Authentication.Core.Factories
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface ISamlClaimFactory
    {
        IList<Claim> Create(Saml2Assertion assertion);
    }
}