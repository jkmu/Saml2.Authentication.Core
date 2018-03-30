using System;
using System.Collections.Generic;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using Saml2.Authentication.Core.Extensions;
using Saml2.Authentication.Core.Options;

namespace Saml2.Authentication.Core.Factories
{
    public class Saml2MessageFactory : ISaml2MessageFactory
    {
        private readonly ServiceProviderConfiguration _serviceProviderConfiguration;
        private readonly IdentityProviderConfiguration _identityProviderConfiguration;
        private readonly Saml2Options _saml2Options;

        public Saml2MessageFactory(
            ServiceProviderConfiguration serviceProviderConfiguration,
            IdentityProviderConfiguration identityProviderConfiguration,
            Saml2Options saml2Options)
        {
            _serviceProviderConfiguration = serviceProviderConfiguration;
            _identityProviderConfiguration = identityProviderConfiguration;
            _saml2Options = saml2Options;
        }

        public Saml20AuthnRequest CreateAuthnRequest(string authnRequestId)
        {
            var request = new Saml20AuthnRequest
            {
                ID = authnRequestId,
                Issuer = _serviceProviderConfiguration.Id,
                ForceAuthn = _serviceProviderConfiguration.ForceAuth,
                IsPassive = _serviceProviderConfiguration.IsPassive,
                Destination = _identityProviderConfiguration.SingleSignOnEndpoint,
                IssuerFormat = _identityProviderConfiguration.IssuerFormat,
                IssueInstant = DateTime.UtcNow,
                ProtocolBinding = _identityProviderConfiguration.ProtocolBinding
            };
            request.Request.AssertionConsumerServiceURL = _saml2Options.AssertionConsumerServiceUrl;

            var audienceRestrictions = new List<ConditionAbstract>(1);
            var audienceRestriction = new AudienceRestriction { Audience = new List<string>(1) { _serviceProviderConfiguration.Id } };
            audienceRestrictions.Add(audienceRestriction);
            request.Request.Conditions = new Conditions { Items = audienceRestrictions };

            if (_identityProviderConfiguration.AllowCreate.HasValue && _identityProviderConfiguration.NameIdPolicyFormat.IsNotNullOrEmpty())
            {
                request.Request.NameIDPolicy.AllowCreate = _identityProviderConfiguration.AllowCreate;
                request.Request.NameIDPolicy.Format = _identityProviderConfiguration.NameIdPolicyFormat;
            }

            if (_identityProviderConfiguration.AuthnContextComparisonType.IsNotNullOrEmpty())
            {
                request.Request.RequestedAuthnContext = new RequestedAuthnContext
                {
                    Comparison = Enum.Parse<AuthnContextComparisonType>(_identityProviderConfiguration.AuthnContextComparisonType),
                    ComparisonSpecified = true,
                    Items = _identityProviderConfiguration.AuthnContextComparisonItems
                };
            }
            return request;
        }

        public Saml20LogoutRequest CreateLogoutRequest(string logoutRequestId, string sessionIndex)
        {
            return new Saml20LogoutRequest
            {
                Issuer = _serviceProviderConfiguration.Id,
                Destination = _identityProviderConfiguration.SingleSignOutEndpoint,
                Reason = Saml20Constants.Reasons.User,
                SubjectToLogOut = new NameID(),
                SessionIndex = sessionIndex
            };
        }
    }
}
