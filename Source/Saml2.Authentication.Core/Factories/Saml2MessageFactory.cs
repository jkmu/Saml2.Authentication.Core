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
        private readonly ServiceProviderConfiguration _serviceProviderConfiguration;
        private readonly IdentityProviderConfiguration _identityProviderConfiguration;

        public Saml2MessageFactory(Saml2Configuration saml2Configuration)
        {
            _serviceProviderConfiguration = saml2Configuration.ServiceProviderConfiguration;
            _identityProviderConfiguration = saml2Configuration.IdentityProviderConfiguration;
        }

        public Saml20AuthnRequest CreateAuthnRequest(string authnRequestId, string assertionConsumerServiceUrl)
        {
            var request = new Saml20AuthnRequest
            {
                ID = authnRequestId,
                Issuer = _serviceProviderConfiguration.Id,
                ForceAuthn = _serviceProviderConfiguration.ForceAuth,
                IsPassive = _serviceProviderConfiguration.IsPassive,
                Destination = _identityProviderConfiguration.SingleSignOnService,
                IssuerFormat = _identityProviderConfiguration.IssuerFormat,
                IssueInstant = DateTime.UtcNow,
                ProtocolBinding = _identityProviderConfiguration.ProtocolBinding,
            };

            request.Request.AssertionConsumerServiceURL = assertionConsumerServiceUrl;

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

        public Saml20LogoutRequest CreateLogoutRequest(string logoutRequestId, string sessionIndex, string subject)
        {
            var request = new Saml20LogoutRequest
            {
                Issuer = _serviceProviderConfiguration.Id,
                Destination = _identityProviderConfiguration.SingleSignOutService,
                Reason = Saml20Constants.Reasons.User,
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
    }
}
