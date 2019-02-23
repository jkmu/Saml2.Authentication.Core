using dk.nita.saml20.Bindings;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Saml2.Authentication.Core.Bindings
{
    /// <summary>
    ///     Implements the HTTP SOAP binding
    /// </summary>
    public class HttpSoapBinding
    {
        /// <summary>
        ///     Gets a response from the IdP based on a message.
        /// </summary>
        /// <param name="endpoint">The IdP endpoint.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Stream GetResponse(string endpoint, string message)
        {
            try
            {
                var soapMessage = WrapInSoapEnvelope(message);
                var webRequest = WebRequest.Create(endpoint);
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.ContentType = "text/xml";
                webRequest.Headers.Add("SOAPAction", HttpArtifactBindingConstants.SoapAction);

                var doc = new XmlDocument();
                doc.LoadXml(soapMessage);

                using (var requestStream = webRequest.GetRequestStream())
                {
                    doc.Save(requestStream);
                }

                using (var webResponse = webRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException("ArtifactResolution failed")))
                    {
                        return GetResponseStream(streamReader.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("ArtifactResolution failed", e);
            }
        }

        private static Stream GetResponseStream(string responseText)
        {
            var document = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            document.LoadXml(responseText);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(document.OuterXml));
            return stream;
        }

        private static string WrapInSoapEnvelope(string s)
        {
            var builder = new StringBuilder();
            builder.AppendLine(SoapConstants.EnvelopeBegin);
            builder.AppendLine(SoapConstants.BodyBegin);
            builder.AppendLine(s);
            builder.AppendLine(SoapConstants.BodyEnd);
            builder.AppendLine(SoapConstants.EnvelopeEnd);

            return builder.ToString();
        }
    }
}