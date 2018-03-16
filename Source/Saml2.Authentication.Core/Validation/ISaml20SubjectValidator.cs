using dk.nita.saml20.Schema.Core;

namespace dk.nita.saml20.Validation
{
    interface ISaml20SubjectValidator
    {
        void ValidateSubject(Subject subject);
    }
}