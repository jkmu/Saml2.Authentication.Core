using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;

namespace Saml2.Authentication.Core.Extensions
{
    public static class Saml2StringExtensions
    {
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        public static string UrlDecode(this string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        /// <summary>
        ///     Uses DEFLATE compression to compress the input value. Returns the result as a Base64 encoded string.
        /// </summary>
        public static string DeflateEncode(this string value)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(new DeflateStream(memoryStream, CompressionMode.Compress, true),
                new UTF8Encoding(false)))
            {
                writer.Write(value);
                writer.Close();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length,
                    Base64FormattingOptions.None);
            }
        }

        /// <summary>
        ///     Take a Base64-encoded string, decompress the result using the DEFLATE algorithm and return the resulting
        ///     string.
        /// </summary>
        public static string DeflateDecompress(this string value)
        {
            var encoded = Convert.FromBase64String(value);
            var memoryStream = new MemoryStream(encoded);

            var result = new StringBuilder();
            using (var stream = new DeflateStream(memoryStream, CompressionMode.Decompress))
            {
                var testStream = new StreamReader(new BufferedStream(stream), Encoding.UTF8);
                // It seems we need to "peek" on the StreamReader to get it started. If we don't do this, the first call to 
                // ReadToEnd() will return string.empty.
                var peek = testStream.Peek();
                result.Append(testStream.ReadToEnd());

                stream.Close();
            }
            return result.ToString();
        }

        /// <summary>
        ///     Uppercase the URL-encoded parts of the string. Needed because Ping does not seem to be able to handle lower-cased
        ///     URL-encodings.
        /// </summary>
        public static string UpperCaseUrlEncode(this string value)
        {
            var result = new StringBuilder(value);
            for (var i = 0; i < result.Length; i++)
                if (result[i] == '%')
                {
                    result[++i] = char.ToUpper(result[i]);
                    result[++i] = char.ToUpper(result[i]);
                }
            return result.ToString();
        }
    }
}