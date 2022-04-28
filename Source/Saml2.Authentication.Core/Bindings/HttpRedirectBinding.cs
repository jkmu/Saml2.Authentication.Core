using System.Security.Cryptography.X509Certificates;

namespace Saml2.Authentication.Core.Bindings
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    using dk.nita.saml20;
    using dk.nita.saml20.Bindings;

    using Extensions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;

    using Providers;

    using SignatureProviders;

    /// <summary>
    ///     Handles the creation of redirect locations when using the HTTP redirect binding, which is outlined in [SAMLBind]
    ///     section 3.4.
    /// </summary>
    internal class HttpRedirectBinding : IHttpRedirectBinding
    {
        private const string SamlResponseQueryKey = "SamlResponse";

        private const string SamlRequestQueryKey = "SAMLRequest";

        private const string SamlRelayStateQueryKey = "RelayState";

        private readonly ISignatureProviderFactory _signatureProviderFactory;

        private readonly IConfigurationProvider _configurationProvider;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpRedirectBinding(
            ISignatureProviderFactory signatureProviderFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfigurationProvider configurationProvider)
        {
            _signatureProviderFactory = signatureProviderFactory;
            _httpContextAccessor = httpContextAccessor;
            _configurationProvider = configurationProvider;
        }

        private HttpContext Context => _httpContextAccessor.HttpContext;

        private HttpRequest Request => _httpContextAccessor.HttpContext.Request;

        private Uri Uri => new Uri(Context.Request.GetEncodedUrl());

        public bool IsValid()
        {
            if (Request == null)
            {
                return false;
            }

            if (Request.Method == HttpMethods.Get)
            {
                return Request.Query.ContainsKey(SamlRequestQueryKey) ||
                       Request.Query.ContainsKey(SamlResponseQueryKey);
            }

            if (Request.Method != HttpMethods.Post)
            {
                return false;
            }

            return Request.Form.ContainsKey(SamlResponseQueryKey);
        }

        public bool IsLogoutRequest()
        {
            if (Request.Method == HttpMethods.Get)
            {
                return Request.Query.ContainsKey(SamlRequestQueryKey);
            }

            if (Request.Method != HttpMethods.Post)
            {
                return false;
            }

            return Request.Form.ContainsKey(SamlRequestQueryKey);
        }

        public Saml2Response GetResponse()
        {
            if (Request.Method == HttpMethods.Get)
            {
                return new Saml2Response
                {
                    Response = Request.Query[SamlResponseQueryKey],
                    RelayState = Request.Query[SamlRelayStateQueryKey].ToString()?.DeflateDecompress()
                };
            }

            if (Request.Method != HttpMethods.Post)
            {
                return null;
            }

            var form = Request.Form;
            return new Saml2Response
            {
                Response = form[SamlResponseQueryKey],
                RelayState = form[SamlRelayStateQueryKey].ToString()?.DeflateDecompress()
            };
        }

        public string GetCompressedRelayState()
        {
            if (Request.Method == HttpMethods.Get)
            {
                return Request.Query[SamlRelayStateQueryKey].ToString();
            }

            if (Request.Method != HttpMethods.Post)
            {
                return null;
            }

            return Request.Form?[SamlRelayStateQueryKey].ToString();
        }

        public string BuildAuthnRequestUrl(string providerName, Saml2AuthnRequest saml2AuthnRequest, string relayState)
        {
            var request = saml2AuthnRequest.GetXml().OuterXml;
            return BuildRequestUrl(providerName, relayState, request, saml2AuthnRequest.Destination);
        }

        public string BuildLogoutRequestUrl(string providerName, Saml2LogoutRequest saml2LogoutRequest, string relayState)
        {
            var request = saml2LogoutRequest.GetXml().OuterXml;
            return BuildRequestUrl(providerName, relayState, request, saml2LogoutRequest.Destination);
        }

        public string BuildLogoutResponseUrl(string providerName, Core.Saml2LogoutResponse logoutResponse, string relayState)
        {
            var response = logoutResponse.GetXml().OuterXml;
            return BuildRequestUrl(providerName, relayState, response, logoutResponse.Destination);
        }

        public string GetLogoutResponseMessage(string providerName)
        {
            var signingCertificate = _configurationProvider.GetIdentityProviderSigningCertificate(providerName);
            var key = signingCertificate.PublicKey.GetRSAPublicKey();
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var parser = new HttpRedirectBindingParser(Uri);
            if (!parser.IsSigned)
            {
                throw new InvalidOperationException("Query is not signed, so there is no signature to verify.");
            }

            // Validates the signature using the public part of the asymmetric key given as parameter.
            var signatureProvider = _signatureProviderFactory.CreateFromAlgorithmUri(key.GetType(), parser.SignatureAlgorithm);
            if (!signatureProvider.VerifySignature(
                    key,
                    Encoding.UTF8.GetBytes(parser.SignedQuery),
                    parser.DecodeSignature()))
            {
                throw new InvalidOperationException("Logout request signature verification failed");
            }

            return parser.Message;
        }

        public Saml2LogoutResponse GetLogoutResponse(string providerName)
        {
            var signingCertificate = _configurationProvider.GetIdentityProviderSigningCertificate(providerName);
            var key = signingCertificate.PublicKey.GetRSAPublicKey();
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var response = new Saml2LogoutResponse();
            var parser = new HttpRedirectBindingParser(Uri);
            response.OriginalLogoutRequest = parser.LogoutRequest;

            if (!parser.IsSigned)
            {
                response.StatusCode = Saml2Constants.StatusCodes.RequestDenied;
            }

            // Validates the signature using the public part of the asymmetric key given as parameter.
            var signatureProvider = _signatureProviderFactory.CreateFromAlgorithmUri(key.GetType(), parser.SignatureAlgorithm);
            if (!signatureProvider.VerifySignature(
                    key,
                    Encoding.UTF8.GetBytes(parser.SignedQuery),
                    parser.DecodeSignature()))
            {
                response.StatusCode = Saml2Constants.StatusCodes.RequestDenied;
            }

            response.StatusCode = Saml2Constants.StatusCodes.Success;
            return response;
        }

        /// <summary>
        ///     If an asymmetric key has been specified, sign the request.
        /// </summary>
        private void AddSignature(string providerName, StringBuilder result)
        {
            var signingCertificate = _configurationProvider.ServiceProviderSigningCertificate();

            var signingKey = signingCertificate.GetRSAPrivateKey();

            // Check if the key is of a supported type. [SAMLBind] sect. 3.4.4.1 specifies this.
            /*if (!(signingKey is RSA || signingKey is DSA || signingKey == null))
            {
                throw new ArgumentException("Signing key must be an instance of either RSA or DSA.");
            }*/

            var hashingAlgorithm = _configurationProvider.GetIdentityProviderConfiguration(providerName).HashingAlgorithm;
            if (signingKey == null)
            {
                return;
            }

            result.Append($"&{HttpRedirectBindingConstants.SigAlg}=");

            var shaHashingAlgorithm = _signatureProviderFactory.ValidateShaHashingAlgorithm(hashingAlgorithm);
            var signingProvider = _signatureProviderFactory.CreateFromAlgorithmName(signingKey.GetType(), shaHashingAlgorithm);

            var urlEncoded = signingProvider.SignatureUri.UrlEncode();
            result.Append(urlEncoded.UpperCaseUrlEncode());

            // Calculate the signature of the URL as described in [SAMLBind] section 3.4.4.1.
            var signature = signingProvider.SignData(signingKey, Encoding.UTF8.GetBytes(result.ToString()));

            result.AppendFormat("&{0}=", HttpRedirectBindingConstants.Signature);
            result.Append(HttpUtility.UrlEncode(Convert.ToBase64String(signature)));
        }

        private string BuildRequestUrl(string providerName, string relayState, string message, string destination)
        {
            var result = new StringBuilder();
            result.AddMessageParameter(message, null);
            result.AddRelayState(message, relayState);
            AddSignature(providerName, result);
            return $"{destination}?{result}";
        }
    }
}