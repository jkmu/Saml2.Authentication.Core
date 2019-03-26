namespace dk.nita.saml20.Validation
{
    using System;
    using Schema.Core;
    using Utils;

    internal class Saml2SubjectConfirmationDataValidator : ISaml2SubjectConfirmationDataValidator
    {
        private readonly Saml2XmlAnyAttributeValidator _anyAttrValidator = new Saml2XmlAnyAttributeValidator();
        private readonly Saml2KeyInfoValidator _keyInfoValidator = new Saml2KeyInfoValidator();
        
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
                {
                    throw new Saml2FormatException("Recipient of SubjectConfirmationData must be a wellformed absolute URI.");
                }
            }

            // NotBefore MUST BE striclty less than NotOnOrAfter if they are both set
            if (subjectConfirmationData.NotBefore != null && subjectConfirmationData.NotBefore.HasValue
                && subjectConfirmationData.NotOnOrAfter != null && subjectConfirmationData.NotOnOrAfter.HasValue)
            {
                if (!(subjectConfirmationData.NotBefore < subjectConfirmationData.NotOnOrAfter))
                {
                    throw new Saml2FormatException(
                        $"NotBefore {Saml2Utils.ToUtcString(subjectConfirmationData.NotBefore.Value)} MUST BE less than NotOnOrAfter {Saml2Utils.ToUtcString(subjectConfirmationData.NotOnOrAfter.Value)} on SubjectConfirmationData");
                }
            }

            // Make sure the extension-attributes are namespace-qualified and do not use reserved namespaces
            if (subjectConfirmationData.AnyAttr != null)
            {
                _anyAttrValidator.ValidateXmlAnyAttributes(subjectConfirmationData.AnyAttr);
            }

            // Standards-defined extension type which has stricter rules than it's base type
            if (subjectConfirmationData is KeyInfoConfirmationData)
            {
                _keyInfoValidator.ValidateKeyInfo(subjectConfirmationData);
            }
        }
    }
}