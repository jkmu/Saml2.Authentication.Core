using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Validation
{
    interface ISaml2SubjectValidator
    {
        void ValidateSubject(Subject subject);
    }
}