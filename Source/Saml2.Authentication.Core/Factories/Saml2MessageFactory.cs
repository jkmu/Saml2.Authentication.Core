using System;
using System.Collections.Generic;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using Saml2.Authentication.Core.Extensions;
using Saml2.Authentication.Core.Options;

namespace Saml2.Authentication.Core.Factories
{
    internal class Saml2MessageFactory : ISaml2MessageFactory
    {
        private readonly IdentityProviderConfiguration _identityProviderConfiguration;
        private readonly Saml2Configuration _saml2Configuration;
        private readonly ServiceProviderConfiguration _serviceProviderConfiguration;

        public Saml2MessageFactory(Saml2Configuration saml2Configuration)
        {
            _saml2Configuration = saml2Configuration;
            _serviceProviderConfiguration = saml2Configuration.ServiceProviderConfiguration;
            _identityProviderConfiguration = saml2Configuration.IdentityProviderConfiguration;
        }

        public Saml2AuthnRequest CreateAuthnRequest(string authnRequestId, string assertionConsumerServiceUrl)
        {
            var request = new Saml2AuthnRequest
            {
                ID = authnRequestId,
                Issuer = _serviceProviderConfiguration.EntityId,
                ForceAuthn = _saml2Configuration.ForceAuth,
                IsPassive = _saml2Configuration.IsPassive,
                Destination = _identityProviderConfiguration.SingleSignOnService,
                IssuerFormat = _identityProviderConfiguration.IssuerFormat,
                IssueInstant = DateTime.UtcNow,
                ProtocolBinding = _identityProviderConfiguration.ProtocolBinding
            };

            request.Request.AssertionConsumerServiceURL = assertionConsumerServiceUrl;

            var audienceRestrictions = new List<ConditionAbstract>(1);
            var audienceRestriction =
                new AudienceRestriction {Audience = new List<string>(1) {_serviceProviderConfiguration.EntityId}};
            audienceRestrictions.Add(audienceRestriction);
            request.Request.Conditions = new Conditions {Items = audienceRestrictions};

            if (_saml2Configuration.AllowCreate.HasValue &&
                _identityProviderConfiguration.NameIdPolicyFormat.IsNotNullOrEmpty())
                request.Request.NameIDPolicy = new NameIDPolicy
                {
                    AllowCreate = _saml2Configuration.AllowCreate,
                    Format = _identityProviderConfiguration.NameIdPolicyFormat
                };

            if (_saml2Configuration.AuthnContextComparisonType.IsNotNullOrEmpty())
                request.Request.RequestedAuthnContext = new RequestedAuthnContext
                {
                    Comparison = Enum.Parse<AuthnContextComparisonType>(_saml2Configuration.AuthnContextComparisonType),
                    ComparisonSpecified = true,
                    Items = _saml2Configuration.AuthnContextComparisonItems
                };
            return request;
        }

        public Saml2LogoutRequest CreateLogoutRequest(string logoutRequestId, string sessionIndex, string subject)
        {
            var request = new Saml2LogoutRequest
            {
                Issuer = _serviceProviderConfiguration.EntityId,
                Destination = _identityProviderConfiguration.SingleSignOutService,
                Reason = Saml2Constants.Reasons.User,
                SubjectToLogOut = new NameID()
            };

            request.Request.ID = logoutRequestId;

            if (sessionIndex.IsNotNullOrEmpty())
                request.SessionIndex = sessionIndex;

            if (subject.IsNotNullOrEmpty())
                request.SubjectToLogOut.Value = subject;

            return request;
        }

        public Saml2LogoutResponse CreateLogoutResponse(string statusCode, string inResponseTo)
        {
            return new Saml2LogoutResponse
            {
                StatusCode = statusCode,
                Issuer = _serviceProviderConfiguration.EntityId,
                InResponseTo = inResponseTo,
                Destination = _identityProviderConfiguration.SingleSignOutService
            };
        }
    }
}