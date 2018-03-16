using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using dk.nita.saml20.Bindings;
using dk.nita.saml20.config;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Bindings.SignatureProviders;
using Saml2.Authentication.Core.Extensions;

namespace Saml2.Authentication.Core.Bindings
{
    /// <summary>
    /// Handles the creation of redirect locations when using the HTTP redirect binding, which is outlined in [SAMLBind] 
    /// section 3.4. 
    /// </summary>
    internal class HttpRedirectBinding : IHttpRedirectBinding
    {
        private const string SamlResponseText = "SamlResponse";

        private const string SamlRequestText = "SAMLRequest";

        private readonly ISignatureProviderFactory _signatureProviderFactory;

        public HttpRedirectBinding(ISignatureProviderFactory signatureProviderFactory)
        {
            _signatureProviderFactory = signatureProviderFactory;
        }

        public bool IsValid(HttpRequest request)
        {
            return request != null && (request.Method == HttpMethods.Get &&
                                       request.Query.ContainsKey(SamlResponseText) ||
                                       request.Query.ContainsKey(SamlRequestText));
        }

        public string GetSamlResponse(HttpRequest request)
        {
            return request?.Query[SamlResponseText];
        }

        /// <summary>
        /// Returns the query part of the url that should be redirected to.
        /// The resulting string should be pre-pended with either ? or &amp; before use.
        /// </summary>
        public string BuildQuery(string request, AsymmetricAlgorithm signingKey, string hashingAlgorithm)
        {
            var shaHashingAlgorithm = _signatureProviderFactory.ValidateShaHashingAlgorithm(hashingAlgorithm);

            // Check if the key is of a supported type. [SAMLBind] sect. 3.4.4.1 specifies this.
            if (!(signingKey is RSACryptoServiceProvider || signingKey is DSA || signingKey == null))
            {
                throw new ArgumentException("Signing key must be an instance of either RSACryptoServiceProvider or DSA.");
            }

            var result = new StringBuilder();
            result.AddMessageParameter(request, null);
            result.AddRelayState(request, null);

            AddSignature(result, signingKey, shaHashingAlgorithm);
            return result.ToString();
        }

        /// <summary>
        /// If an asymmetric key has been specified, sign the request.
        /// </summary>        
        private void AddSignature(StringBuilder result, AsymmetricAlgorithm signingKey, ShaHashingAlgorithm hashingAlgorithm)
        {
            if (signingKey == null)
                return;

            result.Append(string.Format("&{0}=", HttpRedirectBindingConstants.SigAlg));

            var signingProvider = _signatureProviderFactory.CreateFromAlgorithmName(signingKey.GetType(), hashingAlgorithm);

            var urlEncoded = signingProvider.SignatureUri.UrlEncode();
            result.Append(urlEncoded.UpperCaseUrlEncode());

            // Calculate the signature of the URL as described in [SAMLBind] section 3.4.4.1.            
            var signature = signingProvider.SignData(signingKey, Encoding.UTF8.GetBytes(result.ToString()));
            //byte[] signature = SignData(Encoding.UTF8.GetBytes(result.ToString()));            

            result.AppendFormat("&{0}=", HttpRedirectBindingConstants.Signature);
            result.Append(HttpUtility.UrlEncode(Convert.ToBase64String(signature)));
        }
    }
}