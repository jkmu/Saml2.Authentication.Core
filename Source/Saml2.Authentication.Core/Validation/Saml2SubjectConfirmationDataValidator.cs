using System;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Utils;

namespace dk.nita.saml20.Validation
{
    internal class Saml2SubjectConfirmationDataValidator : ISaml2SubjectConfirmationDataValidator
    {
        private readonly Saml2XmlAnyAttributeValidator _anyAttrValidator = new Saml2XmlAnyAttributeValidator();
        private readonly Saml2KeyInfoValidator _keyInfoValidator = new Saml2KeyInfoValidator();

        #region ISaml2SubjectConfirmationDataValidator Members

        /// <summary>
        /// [SAML2.0std] section 2.4.1.2
        /// </summary>
        /// <param name="subjectConfirmationData"></param>
        public void ValidateSubjectConfirmationData(SubjectConfirmationData subjectConfirmationData)
        {
            // If present it must be anyUri
            if (subjectConfirmationData.Recipient != null)
            {
                if (!Uri.IsWellFormedUriString(subjectConfirmationData.Recipient, UriKind.Absolute))
                    throw new Saml2FormatException("Recipient of SubjectConfirmationData must be a wellformed absolute URI.");
            }

            // NotBefore MUST BE striclty less than NotOnOrAfter if they are both set
            if (subjectConfirmationData.NotBefore != null && subjectConfirmationData.NotBefore.HasValue
                && subjectConfirmationData.NotOnOrAfter != null && subjectConfirmationData.NotOnOrAfter.HasValue)
            {
                if (!(subjectConfirmationData.NotBefore < subjectConfirmationData.NotOnOrAfter))
                    throw new Saml2FormatException(String.Format("NotBefore {0} MUST BE less than NotOnOrAfter {1} on SubjectConfirmationData", Saml2Utils.ToUTCString(subjectConfirmationData.NotBefore.Value), Saml2Utils.ToUTCString(subjectConfirmationData.NotOnOrAfter.Value)));
            }

            // Make sure the extension-attributes are namespace-qualified and do not use reserved namespaces
            if (subjectConfirmationData.AnyAttr != null)
                _anyAttrValidator.ValidateXmlAnyAttributes(subjectConfirmationData.AnyAttr);

            // Standards-defined extension type which has stricter rules than it's base type
            if (subjectConfirmationData is KeyInfoConfirmationData)
                _keyInfoValidator.ValidateKeyInfo(subjectConfirmationData);
        }

        #endregion
    }
}