using System;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Validation;

namespace Saml2.Authentication.Core.Validation
{
    internal class Saml2SubjectValidator : ISaml2SubjectValidator
    {
        #region Properties

        private ISaml2NameIDValidator _nameIdValidator;

        private ISaml2NameIDValidator NameIdValidator => _nameIdValidator ?? (_nameIdValidator = new Saml2NameIdValidator());

        private ISaml2SubjectConfirmationValidator _subjectConfirmationValidator;

        private ISaml2SubjectConfirmationValidator SubjectConfirmationValidator => _subjectConfirmationValidator ??
                                                                                    (_subjectConfirmationValidator =
                                                                                        new
                                                                                            Saml2SubjectConfirmationValidator()
                                                                                    );

        #endregion

        public virtual void ValidateSubject(Subject subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            bool validContentFound = false;

            if (subject.Items == null || subject.Items.Length == 0)
                throw new Saml2FormatException("Subject MUST contain either an identifier or a subject confirmation");

            foreach (object o in subject.Items)
            {
                if (o is NameID)
                {
                    validContentFound = true;
                    NameIdValidator.ValidateNameID((NameID)o);
                }
                else if (o is EncryptedElement)
                {
                    validContentFound = true;
                    NameIdValidator.ValidateEncryptedID((EncryptedElement)o);
                }
                else if (o is SubjectConfirmation)
                {
                    validContentFound = true;
                    SubjectConfirmationValidator.ValidateSubjectConfirmation((SubjectConfirmation)o);
                }
            }

            if (!validContentFound)
                throw new Saml2FormatException("Subject must have either NameID, EncryptedID or SubjectConfirmation subelement.");
        }
    }
}
