using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;

namespace Saml2.Authentication.Core.Extensions
{
    public static class SamlStringExtensions
    {
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// Uses DEFLATE compression to compress the input value. Returns the result as a Base64 encoded string.
        /// </summary>
        public static string DeflateEncode(this string value)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(new DeflateStream(memoryStream, CompressionMode.Compress, true), new UTF8Encoding(false)))
            {
                writer.Write(value);
                writer.Close();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, Base64FormattingOptions.None);
            }
        }

        /// <summary>
        /// Uppercase the URL-encoded parts of the string. Needed because Ping does not seem to be able to handle lower-cased URL-encodings.
        /// </summary>
        public static string UpperCaseUrlEncode(this string value)
        {
            var result = new StringBuilder(value);
            for (var i = 0; i < result.Length; i++)
            {
                if (result[i] == '%')
                {
                    result[++i] = char.ToUpper(result[i]);
                    result[++i] = char.ToUpper(result[i]);
                }
            }
            return result.ToString();
        }
    }
}
