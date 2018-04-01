using System;
using dk.nita.saml20.Schema.Core;

namespace Saml2.Authentication.Core.Validation
{
    internal interface ISaml20AssertionValidator
    {
        void ValidateAssertion(Assertion assertion);

        void ValidateTimeRestrictions(Assertion assertion, TimeSpan allowedClockSkew);
    }
}