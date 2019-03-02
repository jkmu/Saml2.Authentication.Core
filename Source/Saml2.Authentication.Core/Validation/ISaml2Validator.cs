namespace Saml2.Authentication.Core.Validation
{
    using System.Security.Cryptography;
    using System.Xml;

    using dk.nita.saml20.Schema.Protocol;

    public interface ISaml2Validator
    {
        bool Validate(XmlElement samlResponse, string originalRequestId);

        Saml2Assertion GetValidatedAssertion(XmlElement element);

        void CheckReplayAttack(string inResponseTo, string originalSamlRequestId);

        void CheckReplayAttack(XmlElement element, string originalSamlRequestId);

        bool CheckStatus(XmlElement samlResponse);

        bool ValidateLogoutRequestIssuer(string logoutRequestIssuer);

        bool ValidateLogoutResponse(LogoutResponse response, string originalRequestId);
    }
}