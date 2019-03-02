namespace dk.nita.saml20.Validation
{
    using Schema.Core;

    interface ISaml2SubjectValidator
    {
        void ValidateSubject(Subject subject);
    }
}