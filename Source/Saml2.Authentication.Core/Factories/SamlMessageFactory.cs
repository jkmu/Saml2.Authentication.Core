namespace Saml2.Authentication.Core.Factories
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using dk.nita.saml20;
    using dk.nita.saml20.Schema.Core;
    using dk.nita.saml20.Schema.Protocol;
    using Extensions;
    using Providers;

    internal class SamlMessageFactory : ISamlMessageFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        public SamlMessageFactory(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        private ServiceProviderConfiguration ServiceProviderConfiguration => _configurationProvider.ServiceProviderConfiguration;

        public Saml2AuthnRequest CreateAuthnRequest(string providerName, string authnRequestId, string assertionConsumerServiceUrl)
        {
            var identityProviderConfiguration = _configurationProvider.GetIdentityProviderConfiguration(providerName);
            var request = new Saml2AuthnRequest
            {
                ID = authnRequestId,
                Issuer = ServiceProviderConfiguration.EntityId,
                ForceAuthn = identityProviderConfiguration.ForceAuth,
                IsPassive = identityProviderConfiguration.IsPassive,
                Destination = identityProviderConfiguration.SingleSignOnService,
                IssuerFormat = identityProviderConfiguration.IssuerFormat,
                IssueInstant = DateTime.UtcNow,
                ProtocolBinding = identityProviderConfiguration.ProtocolBinding
            };

            request.Request.AssertionConsumerServiceURL = assertionConsumerServiceUrl;

            var audienceRestrictions = new List<ConditionAbstract>(1);
            var audienceRestriction =
                new AudienceRestriction { Audience = new List<string>(1) { ServiceProviderConfiguration.EntityId } };
            audienceRestrictions.Add(audienceRestriction);
            request.Request.Conditions = new Conditions { Items = audienceRestrictions };

            if (identityProviderConfiguration.AllowCreate.HasValue &&
                identityProviderConfiguration.NameIdPolicyFormat.IsNotNullOrEmpty())
            {
                request.Request.NameIDPolicy = new NameIDPolicy
                {
                    AllowCreate = identityProviderConfiguration.AllowCreate,
                    Format = identityProviderConfiguration.NameIdPolicyFormat
                };
            }

            if (identityProviderConfiguration.AuthnContextComparisonType.IsNotNullOrEmpty())
            {
                request.Request.RequestedAuthnContext = new RequestedAuthnContext
                {
                    Comparison = Enum.Parse<AuthnContextComparisonType>(identityProviderConfiguration.AuthnContextComparisonType),
                    ComparisonSpecified = true,
                    Items = identityProviderConfiguration.AuthnContextComparisonItems
                };
            }

            return request;
        }

        public Saml2LogoutRequest CreateLogoutRequest(string providerName, string logoutRequestId, string sessionIndex, string subject)
        {
            var identityProviderConfiguration = _configurationProvider.GetIdentityProviderConfiguration(providerName);
            var request = new Saml2LogoutRequest
            {
                Issuer = ServiceProviderConfiguration.EntityId,
                Destination = identityProviderConfiguration.SingleSignOutService,
                Reason = Saml2Constants.Reasons.User,
                SubjectToLogOut = new NameID()
            };

            request.Request.ID = logoutRequestId;

            if (sessionIndex.IsNotNullOrEmpty())
            {
                request.SessionIndex = sessionIndex;
            }

            if (subject.IsNotNullOrEmpty())
            {
                request.SubjectToLogOut.Value = subject;
            }

            return request;
        }

        public Saml2LogoutResponse CreateLogoutResponse(string providerName, string statusCode, string inResponseTo)
        {
            var identityProviderConfiguration = _configurationProvider.GetIdentityProviderConfiguration(providerName);
            return new Saml2LogoutResponse
            {
                StatusCode = statusCode,
                Issuer = ServiceProviderConfiguration.EntityId,
                InResponseTo = inResponseTo,
                Destination = identityProviderConfiguration.SingleSignOutService
            };
        }
    }
}