using System;
using System.Text;
using dk.nita.saml20.Bindings;

namespace Saml2.Authentication.Core.Extensions
{
    public static class Saml2StringBuilderExtensions
    {
        /// <summary>
        ///     If the RelayState property has been set, this method adds it to the query string.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="relayState"></param>
        /// <param name="request"></param>
        public static void AddRelayState(this StringBuilder result, string request, string relayState)
        {
            if (relayState == null)
                return;

            result.Append("&RelayState=");
            // Encode the relay state if we're building a request. Otherwise, append unmodified.
            result.Append(request != null ? relayState.DeflateEncode().UrlEncode() : relayState);
        }

        /// <summary>
        ///     Depending on which one is specified, this method adds the SAMLRequest or SAMLResponse parameter to the URL query.
        /// </summary>
        public static void AddMessageParameter(this StringBuilder result, string request, string response)
        {
            if (!(response == null || request == null))
                throw new Exception("Request or Response property MUST be set.");

            string value;
            if (request != null)
            {
                result.AppendFormat("{0}=", HttpRedirectBindingConstants.SamlRequest);
                value = request;
            }
            else
            {
                result.AppendFormat("{0}=", HttpRedirectBindingConstants.SamlResponse);
                value = response;
            }
            var encoded = value.DeflateEncode();
            var urlEncoded = encoded.UrlEncode();
            result.Append(urlEncoded.UpperCaseUrlEncode());
        }

        public static string TrimSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str)
                if (c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '.' || c == '_')
                    sb.Append(c);
            return sb.ToString();
        }
    }
}