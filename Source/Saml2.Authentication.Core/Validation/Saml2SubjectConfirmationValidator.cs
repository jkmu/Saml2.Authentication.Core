using System;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;
using dk.nita.saml20.Validation;

namespace Saml2.Authentication.Core.Validation
{
    internal class Saml2SubjectConfirmationValidator : ISaml2SubjectConfirmationValidator
    {
        private ISaml2NameIDValidator _nameIdValidator;

        private ISaml2NameIDValidator NameIdValidator => _nameIdValidator ?? (_nameIdValidator = new Saml2NameIdValidator());

        private ISaml2SubjectConfirmationDataValidator _subjectConfirmationDataValidator;
        private ISaml2SubjectConfirmationDataValidator SubjectConfirmationDataValidator
        {
            get
            {
                if (_subjectConfirmationDataValidator != null)
                    return _subjectConfirmationDataValidator;

                _subjectConfirmationDataValidator = new Saml2SubjectConfirmationDataValidator();
                return _subjectConfirmationDataValidator;
            }
        }

        private readonly Saml2KeyInfoValidator _keyInfoValidator = new Saml2KeyInfoValidator();

        public void ValidateSubjectConfirmation(SubjectConfirmation subjectConfirmation)
        {
            if (subjectConfirmation == null) throw new ArgumentNullException("subjectConfirmation");

            if (!Saml2Utils.ValidateRequiredString(subjectConfirmation.Method))
                throw new Saml2FormatException("Method attribute of SubjectConfirmation MUST contain at least one non-whitespace character");

            if (!Uri.IsWellFormedUriString(subjectConfirmation.Method, UriKind.Absolute))
                throw new Saml2FormatException("SubjectConfirmation element has Method attribute which is not a wellformed absolute uri.");

            if (subjectConfirmation.Method == Saml2Constants.SubjectConfirmationMethods.HolderOfKey)
                _keyInfoValidator.ValidateKeyInfo(subjectConfirmation.SubjectConfirmationData);

            if (subjectConfirmation.Item != null)
            {
                if (subjectConfirmation.Item is NameID)
                    NameIdValidator.ValidateNameID((NameID)subjectConfirmation.Item);
                else if (subjectConfirmation.Item is EncryptedElement)
                    NameIdValidator.ValidateEncryptedID((EncryptedElement)subjectConfirmation.Item);
                else
                    throw new Saml2FormatException(String.Format("Identifier of type {0} is not supported for SubjectConfirmation", subjectConfirmation.Item.GetType()));
            }
            else if (subjectConfirmation.SubjectConfirmationData != null)
                SubjectConfirmationDataValidator.ValidateSubjectConfirmationData(subjectConfirmation.SubjectConfirmationData);
        }
    }
}