using System;

namespace dk.nita.saml20.Utils
{
    /// <summary>
    /// Utility functions for validating SAML message time stamps
    /// </summary>
    public static class TimeRestrictionValidation
    {
        /// <summary>
        /// Handle allowed clock skew by decreasing notBefore with allowedClockSkew
        /// </summary>
        public static bool NotBeforeValid(DateTime notBefore, DateTime now, TimeSpan allowedClockSkew)
        {
            return notBefore.Subtract(allowedClockSkew) <= now;
        }

        /// <summary>
        /// Handle allowed clock skew by increasing notOnOrAfter with allowedClockSkew
        /// </summary>
        public static bool NotOnOrAfterValid(DateTime notOnOrAfter, DateTime now, TimeSpan allowedClockSkew)
        {
            return now < notOnOrAfter.Add(allowedClockSkew);
        }
    }
}