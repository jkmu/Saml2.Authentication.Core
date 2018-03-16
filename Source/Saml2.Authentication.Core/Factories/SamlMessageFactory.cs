using System;
using System.Collections.Generic;
using dk.nita.saml20;
using dk.nita.saml20.Schema.Core;
using dk.nita.saml20.Schema.Protocol;
using Saml2.Authentication.Core.Authentication;
using Saml2.Authentication.Core.Extensions;
using Saml2.Authentication.Core.Options;

namespace Saml2.Authentication.Core.Factories
{
    public class SamlMessageFactory : ISamlMessageFactory
    {
        private readonly ServiceProviderOptions _serviceProviderOptions;
        private readonly IdentityProviderOptions _identityProviderOptions;
        private readonly Saml2Options _saml2Options;

        public SamlMessageFactory(
            ServiceProviderOptions serviceProviderOptions,
            IdentityProviderOptions identityProviderOptions,
            Saml2Options saml2Options)
        {
            _serviceProviderOptions = serviceProviderOptions;
            _identityProviderOptions = identityProviderOptions;
            _saml2Options = saml2Options;
        }

        public Saml20AuthnRequest CreateAuthnRequest()
        {
            var request = new Saml20AuthnRequest
            {
                Issuer = _serviceProviderOptions.Id,
                ForceAuthn = _serviceProviderOptions.ForceAuth,
                IsPassive = _serviceProviderOptions.IsPassive,
                Destination = _identityProviderOptions.SingleSignOnEndpoint,
                IssuerFormat = _identityProviderOptions.IssuerFormat,
                IssueInstant = DateTime.UtcNow,
                ProtocolBinding = _identityProviderOptions.ProtocolBinding
            };
            request.Request.AssertionConsumerServiceURL = _saml2Options.AssertionConsumerServiceUrl;

            var audienceRestrictions = new List<ConditionAbstract>(1);
            var audienceRestriction = new AudienceRestriction { Audience = new List<string>(1) { _serviceProviderOptions.Id } };
            audienceRestrictions.Add(audienceRestriction);
            request.Request.Conditions = new Conditions { Items = audienceRestrictions };

            if (_identityProviderOptions.AllowCreate.HasValue && _identityProviderOptions.NameIdPolicyFormat.IsNotNullOrEmpty())
            {
                request.Request.NameIDPolicy.AllowCreate = _identityProviderOptions.AllowCreate;
                request.Request.NameIDPolicy.Format = _identityProviderOptions.NameIdPolicyFormat;
            }

            if (_identityProviderOptions.AuthnContextComparisonType.IsNotNullOrEmpty())
            {
                request.Request.RequestedAuthnContext = new RequestedAuthnContext
                {
                    Comparison = Enum.Parse<AuthnContextComparisonType>(_identityProviderOptions.AuthnContextComparisonType),
                    ComparisonSpecified = true,
                    Items = _identityProviderOptions.AuthnContextComparisonItems
                };
            }
            return request;
        }
    }
}
