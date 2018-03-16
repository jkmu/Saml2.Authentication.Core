using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Saml2.Authentication.Core.Providers
{
    public interface ISamlProvider
    {
        XmlElement GetAssertion(XmlElement xmlElement, AsymmetricAlgorithm privateKey);
        
        XmlDocument GetDecodedSamlResponse(string base64SamlResponse, Encoding encoding);

        bool CheckReplayAttack(XmlElement element, string originalSamlRequestId);

        bool CheckStatus(XmlDocument samlResponseDocument);
    }
}