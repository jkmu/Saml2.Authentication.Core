namespace Saml2.Authentication.Core.Providers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Bindings;
    using dk.nita.saml20;
    using Extensions;
    using Factories;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Options;
    using Validation;

    public class Saml2AuthenticationProvider : ISaml2AuthenticationProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly Saml2Options _options;

        private readonly ISaml2MessageFactory _saml2MessageFactory;

        private readonly IHttpArtifactBinding _httpArtifactBinding;

        private readonly IHttpRedirectBinding _httpRedirectBinding;

        private readonly ISaml2XmlProvider _xmlProvider;

        private readonly ISaml2Validator _validator;

        private readonly ISaml2ClaimFactory _claimFactory;

        private readonly ILogger<Saml2AuthenticationProvider> _logger;

        public Saml2AuthenticationProvider(
            IHttpContextAccessor contextAccessor,
            IOptions<Saml2Options> options,
            ISaml2MessageFactory saml2MessageFactory,
            IHttpArtifactBinding httpArtifactBinding,
            IHttpRedirectBinding httpRedirectBinding,
            ISaml2XmlProvider xmlProvider,
            ISaml2Validator validator,
            ISaml2ClaimFactory claimFactory,
            ILogger<Saml2AuthenticationProvider> logger)
        {
            _contextAccessor = contextAccessor;
            _saml2MessageFactory = saml2MessageFactory;
            _httpArtifactBinding = httpArtifactBinding;
            _httpRedirectBinding = httpRedirectBinding;
            _xmlProvider = xmlProvider;
            _validator = validator;
            _claimFactory = claimFactory;
            _logger = logger;
            _options = options.Value;
        }

        private HttpContext Context => _contextAccessor.HttpContext;

        private HttpRequest Request => Context.Request;

        public Task InitiateSsoAsync(string requestId, string relayState = null)
        {
            var assertionConsumerServiceUrl = $"{Request.GetBaseUrl()}/{_options.AssertionConsumerServiceUrl}";
            var saml20AuthnRequest = _saml2MessageFactory.CreateAuthnRequest(requestId, assertionConsumerServiceUrl);

            var authnRequestUrl = _httpRedirectBinding.BuildAuthnRequestUrl(saml20AuthnRequest, relayState);

            _logger.LogDebug($"Method={nameof(InitiateSsoAsync)}. Redirecting to saml identity provider for SSO. Url={authnRequestUrl}");
            Context.Response.Redirect(authnRequestUrl, true);

            return Task.CompletedTask;
        }

        public Task<Saml2Assertion> ReceiveHttpRedirectAuthnResponseAsync(string initialRequestId)
        {
            var result = _httpRedirectBinding.GetResponse();
            var samlResponseDocument = _xmlProvider.GetDecodedSamlResponse(result);
            var response = samlResponseDocument.DocumentElement;
            var isValid = _validator.Validate(response, initialRequestId);
            if (isValid)
            {
                return Task.FromResult(_validator.GetValidatedAssertion(response));
            }

            throw new InvalidOperationException("The received samlAssertion is invalid");
        }

        public Task<Saml2Assertion> ReceiveHttpArtifactAuthnResponseAsync(string initialRequestId)
        {
            var result = _httpArtifactBinding.ResolveArtifact();
            var assertionElement = _xmlProvider.GetArtifactResponse(result);
            var isValid = _validator.Validate(assertionElement, initialRequestId);
            if (isValid)
            {
                return Task.FromResult(_validator.GetValidatedAssertion(assertionElement));
            }

            throw new InvalidOperationException("The received samlAssertion is invalid");
        }

        public async Task SignInAsync(
            string signinScheme,
            Saml2Assertion assertion,
            AuthenticationProperties authenticationProperties)
        {
            var claims = _claimFactory.Create(assertion);
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            await Context.SignInAsync(signinScheme, principal, authenticationProperties);
        }

        public Task InitiateSloAsync(string requestId, string relayState = null)
        {
            var sessionIndex = Context.User.GetSessionIndex();
            var subject = Context.User.GetSubject();
            var logoutRequest = _saml2MessageFactory.CreateLogoutRequest(requestId, sessionIndex, subject);

            var url = _httpRedirectBinding.BuildLogoutRequestUrl(logoutRequest, relayState);

            Context.Response.Redirect(url, true);

            _logger.LogDebug($"Method={nameof(InitiateSloAsync)}. Redirecting to saml identity provider for SLO. Url={url}");
            return Task.CompletedTask;
        }

        public Task<string> ReceiveIdpInitiatedLogoutRequest()
        {
            var (samlLogoutResponse, isSuccess) = IsLogoutResponseSuccess();
            if (!isSuccess)
            {
                return null;
            }

            var relayState = _httpRedirectBinding.GetCompressedRelayState();

            var response = _saml2MessageFactory.CreateLogoutResponse(
                samlLogoutResponse.StatusCode,
                samlLogoutResponse.OriginalLogoutRequest.ID);

            return Task.FromResult(_httpRedirectBinding.BuildLogoutResponseUrl(response, relayState));
        }

        public Task<bool> ReceiveSpInitiatedLogoutResponse(string logoutRequestId)
        {
            var logoutMessage = _httpRedirectBinding.GetLogoutResponseMessage();
            var logoutRequest = _xmlProvider.GetLogoutResponse(logoutMessage);

            return Task.FromResult(_validator.ValidateLogoutResponse(logoutRequest, logoutRequestId));
        }

        private(Saml2LogoutResponse response, bool isSuccess) IsLogoutResponseSuccess()
        {
            var logoutResponse = _httpRedirectBinding.GetLogoutReponse();
            if (!_validator.ValidateLogoutRequestIssuer(logoutResponse.OriginalLogoutRequest.Issuer.Value))
            {
                logoutResponse.StatusCode = Saml2Constants.StatusCodes.RequestDenied;
            }

            // return logoutResponse.StatusCode == Saml2Constants.StatusCodes.Success && !this.Context.User.Identity.IsAuthenticated;
            return (logoutResponse, logoutResponse.StatusCode == Saml2Constants.StatusCodes.Success);
        }
    }
}
