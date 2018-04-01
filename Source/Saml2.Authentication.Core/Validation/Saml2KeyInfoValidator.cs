using System;
using System.Xml;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.XmlDSig;

namespace dk.nita.saml20.Validation
{
    internal class Saml2KeyInfoValidator
    {
        /// <summary>
        /// Validates the presence and correctness of a <ds:KeyInfo xmlns:ds="http://www.w3.org/2000/09/xmldsig#"/> among the any-xml-elements of a SubjectConfirmationData
        /// </summary>
        /// <param name="subjectConfirmationData"></param>
        public void ValidateKeyInfo(SubjectConfirmationData subjectConfirmationData)
        {
            if (subjectConfirmationData == null)
                throw new Saml2FormatException("SubjectConfirmationData cannot be null when KeyInfo subelements are required");

            if (subjectConfirmationData.AnyElements == null)
                throw new Saml2FormatException(String.Format("SubjectConfirmationData element MUST have at least one {0} subelement", KeyInfo.ELEMENT_NAME));

            bool keyInfoFound = false;
            foreach (XmlElement element in subjectConfirmationData.AnyElements)
            {
                if (element.NamespaceURI != Saml2Constants.XMLDSIG || element.LocalName != KeyInfo.ELEMENT_NAME)
                    continue;

                keyInfoFound = true;

                // A KeyInfo element MUST identify a cryptographic key
                if (!element.HasChildNodes)
                    throw new Saml2FormatException(String.Format("{0} subelement of SubjectConfirmationData MUST NOT be empty", KeyInfo.ELEMENT_NAME));
            }

            // There MUST BE at least one <ds:KeyInfo> element present (from the arbitrary elements list on SubjectConfirmationData
            if (!keyInfoFound)
                throw new Saml2FormatException(String.Format("SubjectConfirmationData element MUST contain at least one {0} in namespace {1}", KeyInfo.ELEMENT_NAME, Saml2Constants.XMLDSIG));
        }

    }
}
