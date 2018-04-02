using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core.Validation
{
    public class Saml2Validator : ISaml2Validator
    {
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

        public bool ValidateLogoutRequestIssuer(string logoutRequestIssuer, string identityProviderEntityId)
        {
            if (string.IsNullOrEmpty(logoutRequestIssuer) || string.IsNullOrEmpty(identityProviderEntityId))
            {
                throw new Saml2Exception("Empty issuer is not allowed");
            }

            return logoutRequestIssuer.Equals(identityProviderEntityId, StringComparison.OrdinalIgnoreCase);
        }

        public bool CheckStatus(XmlDocument samlResponseDocument)
        {
            var status = GetStatusElement(samlResponseDocument);
            switch (status.StatusCode.Value)
            {
                case Saml2Constants.StatusCodes.Success:
                    return true;
                case Saml2Constants.StatusCodes.NoPassive:
                    throw new Saml2Exception(
                        "IdP responded with statuscode NoPassive. A user cannot be signed in with the IsPassiveFlag set when the user does not have a session with the IdP.");
            }

            throw new Saml2Exception($"Saml2 authentication failed. Status: {status.StatusCode.Value}");

        }

        public Saml2Assertion GetValidatedAssertion(XmlElement assertionElement, AsymmetricAlgorithm key, string audience, bool omitAssertionSignatureCheck = false)
        {
            var keys = new List<AsymmetricAlgorithm> { key };
            var assertion = new Saml2Assertion(assertionElement, keys, AssertionProfile.Core, new List<string> { audience }, false);
            if (!omitAssertionSignatureCheck)
            {
                //TODO: This is checked automaticaly if autovalidation is on
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

        private static Status GetStatusElement(XmlDocument doc)
        {
            var statElem = (XmlElement)doc.GetElementsByTagName(Status.ELEMENT_NAME, Saml2Constants.PROTOCOL)[0];
            return Serialization.DeserializeFromXmlString<Status>(statElem.OuterXml);
        }
    }
}
