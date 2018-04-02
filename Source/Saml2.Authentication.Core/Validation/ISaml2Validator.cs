using System.Security.Cryptography;
using System.Xml;

namespace Saml2.Authentication.Core.Validation
{
    public interface ISaml2Validator
    {
        void CheckReplayAttack(string inResponseTo, string originalSamlRequestId);

        void CheckReplayAttack(XmlElement element, string originalSamlRequestId);

        bool CheckStatus(XmlDocument samlResponseDocument);

        Saml2Assertion GetValidatedAssertion(XmlElement assertionElement, AsymmetricAlgorithm key, string audience, bool omitAssertionSignatureCheck = false);
        bool ValidateLogoutRequestIssuer(string logoutRequestIssuer, string identityProviderEntityId);
    }
}