namespace dk.nita.saml20.Validation
{
    using Schema.Core;

    internal interface ISaml2SubjectConfirmationValidator
    {
        void ValidateSubjectConfirmation(SubjectConfirmation subjectConfirmation);
    }
}