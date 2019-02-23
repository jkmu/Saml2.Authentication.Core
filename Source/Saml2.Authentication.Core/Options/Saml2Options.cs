using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Internal;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Saml2.Authentication.Core.Authentication;

namespace Saml2.Authentication.Core.Options
{
    public class Saml2Options : AuthenticationSchemeOptions
    {
        private CookieBuilder _sessionCookie;

        public Saml2Options()
        {
            AssertionConsumerServiceUrl = "Saml2/AssertionConsumerService";
            SingleLogoutServiceUrl = "Saml2/SingleLogoutService";
            DefaultRedirectUrl = "/";
            SignInScheme = Saml2Defaults.SignInScheme;
            AuthenticationScheme = Saml2Defaults.AuthenticationScheme;
            SessionCookieLifetime = TimeSpan.FromMinutes(10);

            _sessionCookie = new SessionCookieBuilder(this)
            {
                Name = $"{Saml2Defaults.SessionKeyPrefix}.{Guid.NewGuid():N}",
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                SecurePolicy = CookieSecurePolicy.SameAsRequest
            };
        }

        public string AssertionConsumerServiceUrl { get; set; }

        public string SingleLogoutServiceUrl { get; set; }

        public string DefaultRedirectUrl { get; set; }

        public string SignInScheme { get; set; }

        public string SignOutScheme { get; set; }

        public string AuthenticationScheme { get; set; }
        
        public IDataProtectionProvider DataProtectionProvider { get; set; }

        public TimeSpan SessionCookieLifetime { get; set; }

        public CookieBuilder SessionCookie
        {
            get => _sessionCookie;
            set => _sessionCookie = value ?? throw new AuthenticationException(nameof(value));
        }

        public ISecureDataFormat<object> ObjectDataFormat { get; set; }
    }

    internal class SessionCookieBuilder : RequestPathBaseCookieBuilder
    {
        private readonly Saml2Options _options;

        public SessionCookieBuilder(Saml2Options options)
        {
            _options = options;
        }

        public override CookieOptions Build(HttpContext context, DateTimeOffset expiresFrom)
        {
            var cookieOptions = base.Build(context, expiresFrom);

            if (!Expiration.HasValue || !cookieOptions.Expires.HasValue)
            {
                cookieOptions.Expires = expiresFrom.Add(_options.SessionCookieLifetime);
            }

            return cookieOptions;
        }
    }
}