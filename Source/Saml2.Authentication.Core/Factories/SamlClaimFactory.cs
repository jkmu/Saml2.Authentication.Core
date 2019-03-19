namespace Saml2.Authentication.Core.Factories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Authentication;
    using Extensions;

    internal class SamlClaimFactory : ISamlClaimFactory
    {
        public IList<Claim> Create(Saml2Assertion assertion)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(assertion.Subject.Value))
            {
                claims.Add(new Claim(Saml2ClaimTypes.Subject, assertion.Subject.Value));
                claims.Add(new Claim(Saml2ClaimTypes.Name, assertion.Subject.Value));
                claims.Add(new Claim(Saml2ClaimTypes.NameIdentifier, assertion.Subject.Value));
            }

            claims.Add(assertion.SessionIndex.IsNotNullOrEmpty()
                ? new Claim(Saml2ClaimTypes.SessionIndex, assertion.SessionIndex)
                : new Claim(Saml2ClaimTypes.SessionIndex, assertion.Id));

            claims.AddRange(assertion.Attributes.Select(attribute =>
                new Claim(attribute.Name, attribute.AttributeValue[0].ToString())));

            return claims;
        }
    }
}