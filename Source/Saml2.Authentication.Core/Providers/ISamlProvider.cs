using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using dk.nita.saml20.Schema.Protocol;

namespace Saml2.Authentication.Core.Providers
{
    public interface ISamlProvider
    {
        XmlElement GetAssertion(XmlElement xmlElement, AsymmetricAlgorithm privateKey);

        XmlDocument GetDecodedSamlResponse(string base64SamlResponse, Encoding encoding);

        LogoutResponse GetLogoutResponse(string logoutResponseMessage);

        XmlElement GetArtifactResponse(Stream stream);
    }
}