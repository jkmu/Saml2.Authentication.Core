namespace Saml2.Authentication.Core.Validation
{
    using System;
    using dk.nita.saml20.Schema.Core;

    internal interface ISaml2AssertionValidator
    {
        void ValidateAssertion(Assertion assertion);

        void ValidateTimeRestrictions(Assertion assertion, TimeSpan allowedClockSkew);
    }
}