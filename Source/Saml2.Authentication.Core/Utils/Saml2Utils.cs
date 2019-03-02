namespace dk.nita.saml20.Utils
{
    using System;
    using System.Xml;

    /// <summary>
    /// Helpers for converting between string and DateTime representations of UTC date-times
    /// and for enforcing the UTC-string-format demand for xml strings in Saml2.0
    /// </summary>
    internal static class Saml2Utils
    {
        public static DateTime FromUtcString(string value)
        {
            try
            {
                return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc);
            }
            catch (FormatException fe)
            {
                throw new Saml2FormatException("Invalid DateTime-string (non-UTC) found in saml:Assertion", fe);
            }
        }

        public static string ToUtcString(DateTime value)
        {
            return XmlConvert.ToString(value, XmlDateTimeSerializationMode.Utc);
        }

        /// <summary>
        /// A string value marked as REQUIRED in [SAML2.0std] must contain at least one non-whitespace character
        /// </summary>
        public static bool ValidateRequiredString(string reqString)
        {
            return !(string.IsNullOrEmpty(reqString) || reqString.Trim().Length == 0);
        }

        /// <summary>
        /// A string value marked as OPTIONAL in [SAML2.0std] must contain at least one non-whitespace character
        /// </summary>
        public static bool ValidateOptionalString(string optString)
        {
            if (optString != null)
            {
                return ValidateRequiredString(optString);
            }

            return true;
        }

        /// <summary>
        /// Make sure that the ID elements is at least 128 bits in length (SAML2.0 std section 1.3.4)
        /// </summary>
        public static bool ValidateIdString(string id)
        {
            return id?.Length >= 16;
        }
    }
}