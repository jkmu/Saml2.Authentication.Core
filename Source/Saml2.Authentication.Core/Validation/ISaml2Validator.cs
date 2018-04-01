using System.Security.Cryptography;
using System.Xml;

namespace Saml2.Authentication.Core.Validation
{
    public interface ISaml2Validator
    {
        bool CheckReplayAttack(string inResponseTo, string originalSamlRequestId);

        bool CheckReplayAttack(XmlElement element, string originalSamlRequestId);

        bool CheckStatus(XmlDocument samlResponseDocument);

        Saml20Assertion GetValidatedAssertion(XmlElement assertionElement, AsymmetricAlgorithm key, string audience, bool omitAssertionSignatureCheck = false);
    }
}