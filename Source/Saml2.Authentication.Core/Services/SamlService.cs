namespace Saml2.Authentication.Core.Services
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Bindings;
    using Configuration;
    using dk.nita.saml20;
    using Extensions;
    using Factories;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Validation;

    public class SamlService : ISamlService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ISamlMessageFactory _samlMessageFactory;

        private readonly IHttpArtifactBinding _httpArtifactBinding;

        private readonly IHttpRedirectBinding _httpRedirectBinding;

        private readonly ISamlXmlProvider _xmlProvider;

        private readonly ISamlValidator _validator;

        private readonly ISamlClaimFactory _claimFactory;

        private readonly ILogger<SamlService> _logger;
        private readonly IConfigurationProvider _configurationProvider;

        public SamlService(
            IHttpContextAccessor contextAccessor,
            ISamlMessageFactory samlMessageFactory,
            IHttpArtifactBinding httpArtifactBinding,
            IHttpRedirectBinding httpRedirectBinding,
            ISamlXmlProvider xmlProvider,
            ISamlValidator validator,
            ISamlClaimFactory claimFactory,
            ILogger<SamlService> logger,
            IConfigurationProvider configurationProvider)
        {
            _contextAccessor = contextAccessor;
            _samlMessageFactory = samlMessageFactory;
            _httpArtifactBinding = httpArtifactBinding;
            _httpRedirectBinding = httpRedirectBinding;
            _xmlProvider = xmlProvider;
            _validator = validator;
            _claimFactory = claimFactory;
            _logger = logger;
            _configurationProvider = configurationProvider;
        }

        private ServiceProviderConfiguration ServiceProviderConfiguration => _configurationProvider.ServiceProviderConfiguration;

        private HttpContext Context => _contextAccessor.HttpContext;

        private HttpRequest Request => Context.Request;

        public Task InitiateSsoAsync(string providerName, string requestId, string relayState = null)
        {
            var assertionConsumerServiceUrl = $"{Request.GetBaseUrl()}/{ServiceProviderConfiguration.AssertionConsumerServiceUrl}";
            var saml20AuthnRequest = _samlMessageFactory.CreateAuthnRequest(providerName, requestId, assertionConsumerServiceUrl);

            var authnRequestUrl = _httpRedirectBinding.BuildAuthnRequestUrl(providerName, saml20AuthnRequest, relayState);

            _logger.LogDebug($"Method={nameof(InitiateSsoAsync)}. Redirecting to saml identity provider for SSO. Url={authnRequestUrl}");
            Context.Response.Redirect(authnRequestUrl);

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

        public Task<Saml2Assertion> ReceiveHttpArtifactAuthnResponseAsync(string providerName, string initialRequestId)
        {
            var result = _httpArtifactBinding.ResolveArtifact(providerName);
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
            var identity = new ClaimsIdentity(claims, signinScheme);
            var principal = new ClaimsPrincipal(identity);
            await Context.SignInAsync(signinScheme, principal, authenticationProperties);
        }

        public Task InitiateSloAsync(string providerName, string requestId, string relayState = null)
        {
            var sessionIndex = Context.User.GetSessionIndex();
            var subject = Context.User.GetSubject();
            var logoutRequest = _samlMessageFactory.CreateLogoutRequest(providerName, requestId, sessionIndex, subject);

            var url = _httpRedirectBinding.BuildLogoutRequestUrl(providerName, logoutRequest, relayState);

            Context.Response.Redirect(url);

            _logger.LogDebug($"Method={nameof(InitiateSloAsync)}. Redirecting to saml identity provider for SLO. Url={url}");
            return Task.CompletedTask;
        }

        public Task<string> ReceiveIdpInitiatedLogoutRequest(string providerName)
        {
            var (samlLogoutResponse, isSuccess) = IsLogoutResponseSuccess(providerName);
            if (!isSuccess)
            {
                return null;
            }

            var relayState = _httpRedirectBinding.GetCompressedRelayState();

            var response = _samlMessageFactory.CreateLogoutResponse(providerName, samlLogoutResponse.StatusCode, samlLogoutResponse.OriginalLogoutRequest.ID);

            return Task.FromResult(_httpRedirectBinding.BuildLogoutResponseUrl(providerName, response, relayState));
        }

        public Task<bool> ReceiveSpInitiatedLogoutResponse(string providerName, string logoutRequestId)
        {
            var logoutMessage = _httpRedirectBinding.GetLogoutResponseMessage(providerName);
            var logoutRequest = _xmlProvider.GetLogoutResponse(logoutMessage);

            return Task.FromResult(_validator.ValidateLogoutResponse(logoutRequest, logoutRequestId));
        }

        private(Saml2LogoutResponse response, bool isSuccess) IsLogoutResponseSuccess(string providerName)
        {
            var logoutResponse = _httpRedirectBinding.GetLogoutResponse(providerName);
            if (!_validator.ValidateLogoutRequestIssuer(providerName, logoutResponse.OriginalLogoutRequest.Issuer.Value))
            {
                logoutResponse.StatusCode = Saml2Constants.StatusCodes.RequestDenied;
            }

            return (logoutResponse, logoutResponse.StatusCode == Saml2Constants.StatusCodes.Success);
        }
    }
}
