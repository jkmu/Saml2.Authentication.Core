using dk.nita.saml20;

namespace Saml2.Authentication.Core.Factories
{
    public interface ISamlMessageFactory
    {
        Saml20AuthnRequest CreateAuthnRequest();
    }
}