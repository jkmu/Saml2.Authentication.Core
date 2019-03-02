namespace Saml2.Authentication.Core.Factories
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public interface ISaml2ClaimFactory
    {
        IList<Claim> Create(Saml2Assertion assertion);
    }
}