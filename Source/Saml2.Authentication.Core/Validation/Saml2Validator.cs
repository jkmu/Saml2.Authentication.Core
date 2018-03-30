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
        public bool CheckReplayAttack(XmlElement element, string originalSamlRequestId)
        {
            var inResponseToAttribute = element.Attributes["InResponseTo"];
            if (inResponseToAttribute == null)
            {
                throw new Saml20Exception("Received a response message that did not contain an InResponseTo attribute");
            }

            var inResponseTo = inResponseToAttribute.Value;
            if (string.IsNullOrEmpty(originalSamlRequestId) || string.IsNullOrEmpty(inResponseTo))
            {
                throw new Saml20Exception("Empty protocol message id is not allowed.");
            }

            if (inResponseTo.Equals(originalSamlRequestId, StringComparison.OrdinalIgnoreCase))
            {
                throw new Saml20Exception("Replay attack.");
            }

            return true;
        }

        public bool CheckReplayAttack(string inResponseTo, string originalSamlRequestId)
        {
            if (string.IsNullOrEmpty(originalSamlRequestId) || string.IsNullOrEmpty(inResponseTo))
            {
                throw new Saml20Exception("Empty protocol message id is not allowed.");
            }

            if (inResponseTo.Equals(originalSamlRequestId, StringComparison.OrdinalIgnoreCase))
            {
                throw new Saml20Exception("Replay attack.");
            }

            return true;
        }

        public bool CheckStatus(XmlDocument samlResponseDocument)
        {
            var status = GetStatusElement(samlResponseDocument);
            switch (status.StatusCode.Value)
            {
                case Saml20Constants.StatusCodes.Success:
                    return true;
                case Saml20Constants.StatusCodes.NoPassive:
                    throw new Saml20Exception(
                        "IdP responded with statuscode NoPassive. A user cannot be signed in with the IsPassiveFlag set when the user does not have a session with the IdP.");
            }

            return false;
        }

        public Saml20Assertion GetValidatedAssertion(XmlElement assertionElement, AsymmetricAlgorithm privateKey, bool omitAssertionSignatureCheck = false)
        {
            var assertion = new Saml20Assertion(assertionElement, null, false);
            if (!omitAssertionSignatureCheck)
            {
                if (!assertion.CheckSignature(new List<AsymmetricAlgorithm> { privateKey }))
                {
                    throw new Saml20Exception("Invalid signature in assertion");
                }
            }

            if (assertion.IsExpired())
            {
                throw new Saml20Exception("Assertion is expired");
            }

            return assertion;
        }

        private static Status GetStatusElement(XmlDocument doc)
        {
            var statElem = (XmlElement)doc.GetElementsByTagName(Status.ELEMENT_NAME, Saml20Constants.PROTOCOL)[0];
            return Serialization.DeserializeFromXmlString<Status>(statElem.OuterXml);
        }
    }
}
