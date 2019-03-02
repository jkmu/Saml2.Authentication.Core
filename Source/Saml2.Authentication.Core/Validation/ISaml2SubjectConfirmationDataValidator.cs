namespace dk.nita.saml20.Validation
{
    using Schema.Core;

    internal interface ISaml2SubjectConfirmationDataValidator
    {
        void ValidateSubjectConfirmationData(SubjectConfirmationData data);
    }
}