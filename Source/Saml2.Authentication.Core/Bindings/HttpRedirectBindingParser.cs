using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using dk.nita.saml20.Schema.Protocol;
using dk.nita.saml20.Utils;
using Saml2.Authentication.Core.Extensions;
using CONSTS = dk.nita.saml20.Bindings.HttpRedirectBindingConstants;

namespace dk.nita.saml20.Bindings
{
    /// <summary>
    /// Parses and validates the query parameters of a HttpRedirectBinding. [SAMLBind] section 3.4.
    /// </summary>
    public class HttpRedirectBindingParser
    {
        /// <summary>
        /// Parses the query string.
        /// </summary>
        /// <param name="uri">The URL that the user was redirected to by the IDP. It is essential for the survival of the signature,
        /// that the URL is not modified in any way, eg. by URL-decoding it.</param>
        public HttpRedirectBindingParser(Uri uri)
        {
            var paramDict = ToDictionary(uri);

            foreach (var param in paramDict)
                SetParam(param.Key, HttpUtility.UrlDecode(param.Value));

            // If the message is signed, save the original, encoded parameters so that the signature can be verified.
            if (IsSigned)
                CreateSignatureSubject(paramDict);

            ReadMessageParameter();
        }

        private static Dictionary<string, string> ToDictionary(Uri uri)
        {
            var parameters = uri.Query.Substring(1).Split('&');
            var result = new Dictionary<string, string>(parameters.Length);
            foreach (var s in parameters)
            {
                var parameter = s.Split('=');
                result.Add(parameter[0], parameter[1]);
            }

            return result;
        }

        #region Query parameters
        private string _message;

        /// <summary>
        /// Returns the message that was contained in the query. Use the <code>IsResponse</code> or the <code>IsRequest</code> property 
        /// to determine the kind of message.
        /// </summary>
        public string Message => _message;

        private string _relaystate;

        /// <summary>
        /// Returns the relaystate that was included with the query. The result will still be encoded according to the 
        /// rules given in section 3.4.4.1 of [SAMLBind], ie. base64-encoded and DEFLATE-compressed. Use the property 
        /// <code>RelayStateDecoded</code> to get the decoded contents of the RelayState parameter.
        /// </summary>
        public string RelayState => _relaystate;

        private string _relaystateDecoded;

        /// <summary>
        /// Returns a decoded and decompressed version of the RelayState parameter.
        /// </summary>
        public string RelayStateDecoded => _relaystateDecoded ?? (_relaystateDecoded = _relaystate.DeflateDecompress());


        private string _signatureAlgorithm;

        private string _signature;

        /// <summary>
        /// The signed part of the query is recreated in this string.
        /// </summary>
        private string _signedquery;

        #endregion

        /// <summary>
        /// If the parsed query string contained a SAMLResponse, this variable is set to true.
        /// </summary>
        private bool _isResponse;

        public string SignedQuery => _signedquery;

        /// <summary>
        /// <code>true</code> if the parsed message contains a response message.
        /// </summary>
        public bool IsResponse => _isResponse;

        /// <summary>
        /// <code>true</code> if the parsed message contains a request message.
        /// </summary>
        public bool IsRequest => !_isResponse;

        /// <summary>
        /// <code>true</code> if the parsed message contains a signature.
        /// </summary>
        public bool IsSigned => _signature != null;

        /// <summary>
        /// Gets the signature value
        /// </summary>
        public string Signature => _signature;

        /// <summary>
        /// Gets the signature algorithm.
        /// </summary>
        /// <value>The signature algorithm.</value>
        public string SignatureAlgorithm => _signatureAlgorithm;

        /// <summary>
        /// Decodes the Signature parameter.
        /// </summary>
        public byte[] DecodeSignature()
        {
            if (!IsSigned)
            {
                throw new InvalidOperationException("Query does not contain a signature.");
            }

            return Convert.FromBase64String(_signature);
        }

        /// <summary>
        /// Re-creates the list of parameters that are signed, in order to verify the signature.
        /// </summary>
        private void CreateSignatureSubject(IDictionary<string, string> queryParams)
        {
            var signedQuery = new StringBuilder();
            if (IsResponse)
            {
                signedQuery.AppendFormat("{0}=", CONSTS.SamlResponse);
                signedQuery.Append(queryParams[CONSTS.SamlResponse]);
            }
            else
            {
                signedQuery.AppendFormat("{0}=", CONSTS.SamlResponse);
                signedQuery.Append(queryParams[CONSTS.SamlResponse]);
            }

            if (_relaystate != null)
                signedQuery.AppendFormat("&{0}=", CONSTS.RelayState).Append(queryParams[CONSTS.RelayState]);

            if (_signature != null)
                signedQuery.AppendFormat("&{0}=", CONSTS.SigAlg).Append(queryParams[CONSTS.SigAlg]);

            _signedquery = signedQuery.ToString();
        }

        /// <summary>
        /// Decodes the message parameter.
        /// </summary>
        private void ReadMessageParameter()
        {
            _message = _message.DeflateDecompress();
        }

        ///// <summary>
        ///// Take a Base64-encoded string, decompress the result using the DEFLATE algorithm and return the resulting 
        ///// string.
        ///// </summary>
        //private static string DeflateDecompress(string str)
        //{
        //    var encoded = Convert.FromBase64String(str);            
        //    var memoryStream = new MemoryStream(encoded);

        //    var result = new StringBuilder();
        //    using (var stream = new DeflateStream(memoryStream, CompressionMode.Decompress))
        //    {
        //        var testStream = new StreamReader(new BufferedStream(stream), Encoding.UTF8);
        //        // It seems we need to "peek" on the StreamReader to get it started. If we don't do this, the first call to 
        //        // ReadToEnd() will return string.empty.
        //        testStream.Peek();
        //        result.Append(testStream.ReadToEnd());

        //        stream.Close();
        //    }
        //    return result.ToString();
        //}

        /// <summary>
        /// Set the parameter fields of the class.
        /// </summary>
        private void SetParam(string key, string value)
        {
            switch (key.ToLower())
            {
                case "samlrequest":
                    _isResponse = false;
                    _message = value;
                    return;
                case "samlresponse":
                    _isResponse = true;
                    _message = value;
                    return;
                case "relaystate":
                    _relaystate = value;
                    return;
                case "sigalg":
                    _signatureAlgorithm = value;
                    return;
                case "signature":
                    _signature = value;
                    return;
            }
        }

        /// <summary>
        /// Returns the LogoutRequest string in deserialized form.
        /// </summary>
        /// <returns></returns>
        public LogoutRequest LogoutRequest => !_isResponse ? Serialization.DeserializeFromXmlString<LogoutRequest>(_message) : null;
    }
}