using System;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;
using dk.nita.saml20.Validation;

namespace Saml2.Authentication.Core.Validation
{
    internal class Saml20SubjectConfirmationValidator : ISaml20SubjectConfirmationValidator
    {
        private ISaml20NameIDValidator _nameIdValidator;

        private ISaml20NameIDValidator NameIdValidator => _nameIdValidator ?? (_nameIdValidator = new Saml20NameIDValidator());

        private ISaml20SubjectConfirmationDataValidator _subjectConfirmationDataValidator;
        private ISaml20SubjectConfirmationDataValidator SubjectConfirmationDataValidator
        {
            get
            {
                if (_subjectConfirmationDataValidator != null)
                    return _subjectConfirmationDataValidator;

                _subjectConfirmationDataValidator = new Saml20SubjectConfirmationDataValidator();
                return _subjectConfirmationDataValidator;
            }
        }

        private readonly Saml20KeyInfoValidator _keyInfoValidator = new Saml20KeyInfoValidator();

        public void ValidateSubjectConfirmation(SubjectConfirmation subjectConfirmation)
        {
            if (subjectConfirmation == null) throw new ArgumentNullException("subjectConfirmation");

            if (!Saml20Utils.ValidateRequiredString(subjectConfirmation.Method))
                throw new Saml20FormatException("Method attribute of SubjectConfirmation MUST contain at least one non-whitespace character");

            if (!Uri.IsWellFormedUriString(subjectConfirmation.Method, UriKind.Absolute))
                throw new Saml20FormatException("SubjectConfirmation element has Method attribute which is not a wellformed absolute uri.");

            if (subjectConfirmation.Method == Saml20Constants.SubjectConfirmationMethods.HolderOfKey)
                _keyInfoValidator.ValidateKeyInfo(subjectConfirmation.SubjectConfirmationData);

            if (subjectConfirmation.Item != null)
            {
                if (subjectConfirmation.Item is NameID)
                    NameIdValidator.ValidateNameID((NameID)subjectConfirmation.Item);
                else if (subjectConfirmation.Item is EncryptedElement)
                    NameIdValidator.ValidateEncryptedID((EncryptedElement)subjectConfirmation.Item);
                else
                    throw new Saml20FormatException(String.Format("Identifier of type {0} is not supported for SubjectConfirmation", subjectConfirmation.Item.GetType()));
            }
            else if (subjectConfirmation.SubjectConfirmationData != null)
                SubjectConfirmationDataValidator.ValidateSubjectConfirmationData(subjectConfirmation.SubjectConfirmationData);
        }
    }
}