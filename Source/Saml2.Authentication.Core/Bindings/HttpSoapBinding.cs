using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using dk.nita.saml20.Bindings;

namespace Saml2.Authentication.Core.Bindings
{
    /// <summary>
    ///     Implements the HTTP SOAP binding
    /// </summary>
    public class HttpSoapBinding
    {
        ///// <summary>
        ///// Sends a response message.
        ///// </summary>
        ///// <param name="samlMessage">The saml message.</param>
        //public void SendResponseMessage(string samlMessage)
        //{
        //    Context.Response.ContentType = "text/xml";
        //    var writer = new StreamWriter(Context.Response.Body);
        //    writer.Write(WrapInSoapEnvelope(samlMessage));
        //    writer.Flush();
        //    writer.Close();
        //    //_context.Response.End();
        //}

        ///// <summary>
        ///// Wraps a message in a SOAP envelope.
        ///// </summary>
        ///// <param name="s">The s.</param>
        ///// <returns></returns>
        //public string WrapInSoapEnvelope(string s)
        //{
        //    var builder = new StringBuilder();
        //    builder.AppendLine(SoapConstants.EnvelopeBegin);
        //    builder.AppendLine(SoapConstants.BodyBegin);
        //    builder.AppendLine(s);
        //    builder.AppendLine(SoapConstants.BodyEnd);
        //    builder.AppendLine(SoapConstants.EnvelopeEnd);

        //    return builder.ToString();
        //}

        ///// <summary>
        ///// Validates the server certificate.
        ///// </summary>
        ///// <param name="sender">The sender.</param>
        ///// <param name="certificate">The certificate.</param>
        ///// <param name="chain">The chain.</param>
        ///// <param name="sslPolicyErrors">The SSL policy errors.</param>
        ///// <returns>True if validation of the server certificate generates no policy errors</returns>
        //public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    if (sslPolicyErrors == SslPolicyErrors.None)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        ///     Creates a WCF SSL binding.
        /// </summary>
        /// <returns></returns>
        private static Binding CreateSslBinding()
        {
            return new BasicHttpBinding(BasicHttpSecurityMode.Transport) {TextEncoding = Encoding.UTF8};
        }

        /// <summary>
        ///     Gets a response from the IdP based on a message.
        /// </summary>
        /// <param name="endpoint">The IdP endpoint.</param>
        /// <param name="message">The message.</param>
        /// <param name="relayState"></param>
        /// <param name="isBasicEnabled"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Stream GetResponse(string endpoint, string message, string relayState = null,
            bool isBasicEnabled = false, string username = null, string password = null)
        {
            var binding = CreateSslBinding();
            var request = Message.CreateMessage(binding.MessageVersion, HttpArtifactBindingConstants.SoapAction,
                new SimpleBodyWriter(message));

            request.Headers.To = new Uri(endpoint);

            var property = new HttpRequestMessageProperty {Method = "POST"};
            property.Headers.Add(HttpRequestHeader.ContentType, "text/xml; charset=utf-8");

            //We are using Basic http auth over ssl
            if (isBasicEnabled && username != null && password != null)
            {
                var basicAuthzHeader =
                    "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                property.Headers.Add(HttpRequestHeader.Authorization, basicAuthzHeader);
            }

            request.Properties.Add(HttpRequestMessageProperty.Name, property);
            if (relayState != null)
                request.Properties.Add("relayState", relayState);

            var epa = new EndpointAddress(endpoint);

            var factory = new ChannelFactory<IRequestChannel>(binding, epa);
            var reqChannel = factory.CreateChannel();

            reqChannel.Open();
            var response = reqChannel.Request(request);
            reqChannel.Close();
            var xDoc = new XmlDocument
            {
                XmlResolver = null,
                PreserveWhitespace = true
            };
            xDoc.Load(response.GetReaderAtBodyContents());
            var outerXml = xDoc.DocumentElement.OuterXml;
            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(outerXml));
            return memStream;
        }
    }

    /// <summary>
    ///     A simple body writer
    /// </summary>
    internal class SimpleBodyWriter : BodyWriter
    {
        private readonly string _message;

        public SimpleBodyWriter(string message) : base(false)
        {
            _message = message;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteRaw(_message);
        }
    }
}