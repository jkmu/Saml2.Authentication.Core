namespace Saml2.Authentication.Core.Configuration
{
    using Authentication;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Security.Authentication;

    public class Saml2Options : AuthenticationSchemeOptions
    {
        private CookieBuilder _sessionCookie;

        public Saml2Options()
        {
            DefaultRedirectUrl = "/";
            SessionCookieLifetime = TimeSpan.FromMinutes(10);

            _sessionCookie = new SessionCookieBuilder(this)
            {
                Name = $"{Saml2Defaults.SessionKeyPrefix}.{Guid.NewGuid():N}",
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                SecurePolicy = CookieSecurePolicy.SameAsRequest,
                IsEssential = true
            };
        }

        public string DefaultRedirectUrl { get; set; }

        public string SignInScheme { get; set; }

        public string SignOutScheme { get; set; }

        public IDataProtectionProvider DataProtectionProvider { get; set; }

        public TimeSpan SessionCookieLifetime { get; set; }

        public CookieBuilder SessionCookie
        {
            get => _sessionCookie;
            set => _sessionCookie = value ?? throw new AuthenticationException(nameof(value));
        }

        public ISecureDataFormat<object> ObjectDataFormat { get; set; }

        public string IdentityProviderName { get; set; }
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
