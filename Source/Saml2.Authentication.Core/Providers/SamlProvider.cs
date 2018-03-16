using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;

namespace Saml2.Authentication.Core.Providers
{
    public class SamlProvider : ISamlProvider
    {
        public XmlDocument GetDecodedSamlResponse(string base64SamlResponse, Encoding encoding)
        {
            var doc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            var samlResponse = encoding.GetString(Convert.FromBase64String(base64SamlResponse));
            doc.LoadXml(samlResponse);
            return doc;
        }

        public XmlElement GetAssertion(XmlElement xmlElement, AsymmetricAlgorithm privateKey)
        {
            if (IsEncrypted(xmlElement))
            {
                return GetDecriptedAssertion(xmlElement, privateKey);
            }

            var assertionList = xmlElement.GetElementsByTagName(Assertion.ELEMENT_NAME, Saml20Constants.ASSERTION);

            var assertion = (XmlElement)assertionList[0];
            if (assertion == null)
            {
                throw new Saml20Exception("Missing assertion");
            }

            return assertion;
        }


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

        public XmlElement GetDecriptedAssertion(XmlElement xmlElement, AsymmetricAlgorithm privateKey)
        {
            var encryptedList = xmlElement.GetElementsByTagName(EncryptedAssertion.ELEMENT_NAME, Saml20Constants.ASSERTION);
            var assertion = (XmlElement)encryptedList[0];
            if (assertion == null)
            {
                throw new Saml20Exception("Missing assertion");
            }

            var encryptedAssertion = new Saml20EncryptedAssertion((RSA)privateKey);
            encryptedAssertion.LoadXml(assertion);
            encryptedAssertion.Decrypt();

            return encryptedAssertion.Assertion.DocumentElement;
        }

        private static bool IsEncrypted(XmlElement element)
        {
            var encryptedList = element.GetElementsByTagName(EncryptedAssertion.ELEMENT_NAME, Saml20Constants.ASSERTION);
            return encryptedList.Count == 1;
        }

        private static Status GetStatusElement(XmlDocument doc)
        {
            var statElem = (XmlElement)doc.GetElementsByTagName(Status.ELEMENT_NAME, Saml20Constants.PROTOCOL)[0];
            return Serialization.DeserializeFromXmlString<Status>(statElem.OuterXml);
        }

    }
}
