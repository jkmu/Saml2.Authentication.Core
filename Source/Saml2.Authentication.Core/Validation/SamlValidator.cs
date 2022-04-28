using System.Security.Cryptography.X509Certificates;

namespace Saml2.Authentication.Core.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Xml;
    using Configuration;
    using dk.nita.saml20;
    using dk.nita.saml20.Schema.Protocol;
    using dk.nita.saml20.Utils;
    using Providers;

    public class SamlValidator : ISamlValidator
    {
        private readonly ISamlXmlProvider _xmlProvider;

        private readonly IConfigurationProvider _configurationProvider;

        public SamlValidator(
            ISamlXmlProvider xmlProvider,
            IConfigurationProvider configurationProvider)
        {
            _xmlProvider = xmlProvider;
            _configurationProvider = configurationProvider;
        }

        private ServiceProviderConfiguration ServiceProviderConfiguration => _configurationProvider.ServiceProviderConfiguration;

        public bool Validate(XmlElement samlResponse, string originalRequestId)
        {
            CheckReplayAttack(samlResponse, originalRequestId);

            return CheckStatus(samlResponse);
        }

        public void CheckReplayAttack(XmlElement element, string originalSamlRequestId)
        {
            var inResponseToAttribute = element.Attributes["InResponseTo"];
            if (inResponseToAttribute == null)
            {
                throw new Saml2Exception("Received a response message that did not contain an InResponseTo attribute");
            }

            var inResponseTo = inResponseToAttribute.Value;
            if (string.IsNullOrEmpty(originalSamlRequestId) || string.IsNullOrEmpty(inResponseTo))
            {
                throw new Saml2Exception("Empty protocol message id is not allowed.");
            }

            if (!inResponseTo.Equals(originalSamlRequestId, StringComparison.OrdinalIgnoreCase))
            {
                throw new Saml2Exception("Replay attack.");
            }
        }

        public void CheckReplayAttack(string inResponseTo, string originalSamlRequestId)
        {
            if (string.IsNullOrEmpty(originalSamlRequestId) || string.IsNullOrEmpty(inResponseTo))
            {
                throw new Saml2Exception("Empty protocol message id is not allowed.");
            }

            if (!inResponseTo.Equals(originalSamlRequestId, StringComparison.OrdinalIgnoreCase))
            {
                throw new Saml2Exception("Replay attack.");
            }
        }

        public bool ValidateLogoutRequestIssuer(string providerName, string logoutRequestIssuer)
        {
            var entityId = _configurationProvider.GetIdentityProviderConfiguration(providerName).EntityId;
            if (string.IsNullOrEmpty(logoutRequestIssuer) || string.IsNullOrEmpty(entityId))
            {
                throw new Saml2Exception("Empty issuer is not allowed");
            }

            return logoutRequestIssuer.Equals(entityId, StringComparison.OrdinalIgnoreCase);
        }

        public bool CheckStatus(XmlElement samlResponse)
        {
            var status = GetStatusElement(samlResponse);
            switch (status.StatusCode.Value)
            {
                case Saml2Constants.StatusCodes.Success:
                    return true;
                case Saml2Constants.StatusCodes.NoPassive:
                    throw new Saml2Exception(
                        "IdP responded with status code NoPassive. A user cannot be signed in with the IsPassiveFlag set when the user does not have a session with the IdP.");
            }

            throw new Saml2Exception($"Saml2 authentication failed. Status: {status.StatusCode.Value}");
        }

        public Saml2Assertion GetValidatedAssertion(XmlElement element)
        {
            var signingCertificate = _configurationProvider.ServiceProviderSigningCertificate();

            var assertionElement = _xmlProvider.GetAssertion(element, signingCertificate.GetRSAPrivateKey());
            var key = signingCertificate.PublicKey.GetRSAPublicKey();
            var audience = ServiceProviderConfiguration.EntityId;

            var keys = new List<AsymmetricAlgorithm> { key };
            var assertion = new Saml2Assertion(assertionElement, keys, AssertionProfile.Core, new List<string> { audience }, false);

            if (!ServiceProviderConfiguration.OmitAssertionSignatureCheck)
            {
                // TODO: This is checked automatically if auto-validation is on
                if (!assertion.CheckSignature(keys))
                {
                    throw new Saml2Exception("Invalid signature in assertion");
                }
            }

            if (assertion.IsExpired())
            {
                throw new Saml2Exception("Assertion is expired");
            }

            return assertion;
        }

        public bool ValidateLogoutResponse(LogoutResponse response, string originalRequestId)
        {
            CheckReplayAttack(response.InResponseTo, originalRequestId);

            return response.Status.StatusCode.Value == Saml2Constants.StatusCodes.Success;
        }

        private static Status GetStatusElement(XmlElement element)
        {
            var statElem = (XmlElement)element.GetElementsByTagName(Status.ELEMENT_NAME, Saml2Constants.PROTOCOL)[0];
            return Serialization.DeserializeFromXmlString<Status>(statElem.OuterXml);
        }
    }
}
