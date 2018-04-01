using System.Collections.Generic;
using System.Security.Claims;

namespace Saml2.Authentication.Core.Factories
{
    public interface ISaml2ClaimFactory
    {
        IList<Claim> Create(Saml2Assertion assertion);
    }
}