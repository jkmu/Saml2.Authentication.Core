using System.Collections.Generic;
using System.Security.Claims;

namespace Saml2.Authentication.Core.Factories
{
    internal interface ISaml2ClaimFactory
    {
        IList<Claim> Create(Saml20Assertion assertion);
    }
}