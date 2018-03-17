using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Saml2.Authentication.Core.Factories
{
    public class Saml2ClaimFactory : ISaml2ClaimFactory
    {
        public IList<Claim> Create(Saml20Assertion assertion)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(assertion.Subject.Value))
            {
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                    assertion.Subject.Value));
                claims.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    assertion.Subject.Value));
            }

            claims.AddRange(assertion.Attributes.Select(attribute =>
                new Claim(attribute.Name, attribute.AttributeValue.ToString())));

            return claims;
        }
    }
}
