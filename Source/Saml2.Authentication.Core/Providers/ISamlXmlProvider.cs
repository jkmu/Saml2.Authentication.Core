namespace Saml2.Authentication.Core.Providers
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Xml;

    using Bindings;

    using dk.nita.saml20.Schema.Protocol;

    public interface ISamlXmlProvider
    {
        XmlElement GetAssertion(XmlElement xmlElement, AsymmetricAlgorithm privateKey);

        XmlDocument GetDecodedSamlResponse(Saml2Response saml2Response);

        LogoutResponse GetLogoutResponse(string logoutResponseMessage);

        XmlElement GetArtifactResponse(Stream stream);
    }
}