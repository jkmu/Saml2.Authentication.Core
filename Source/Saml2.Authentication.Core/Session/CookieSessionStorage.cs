using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Saml2.Authentication.Core.Extensions;
using Saml2.Authentication.Core.Options;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Saml2.Authentication.Core.Session
{
    public class CookieSessionStorage : ISessionStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptionsMonitor<Saml2Options> _options;
        private readonly ISystemClock _clock;

        public CookieSessionStorage(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<Saml2Options> options,
            ISystemClock clock)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options;
            _clock = clock;
        }

        public HttpContext Context => _httpContextAccessor.HttpContext;

        public Saml2Options Options => _options.CurrentValue;

        public HttpResponse Response => Context.Response;

        public IRequestCookieCollection Cookies => Context.Request.Cookies;

        public Task<T> LoadAsync<T>()
        {
            if (!Cookies.TryGetValue(SessionCookieName, out var value))
            {
                return Task.FromResult(default(T));
            }
            
            if (Options.ObjectDataFormat.Unprotect(value) is JObject some)
            {
                return Task.FromResult(some.ToObject<T>());
            }

            return Task.FromResult(default(T));
        }

        public async Task SaveAsync<T>(object session)
        {
            await RemoveAsync<T>();
            Response.Cookies.Append(SessionCookieName, Options.ObjectDataFormat.Protect(session),
                Options.SessionCookie.Build(Context, _clock.UtcNow));
        }

        public Task RemoveAsync<T>()
        {
            Response.DeleteAllSessionCookies(Context.Request,
                 Options.SessionCookie.Build(Context, _clock.UtcNow.AddDays(-1)));
            return Task.CompletedTask;
        }

        public Task RefreshAsync<T>()
        {
            return Task.CompletedTask;
        }

        private string SessionCookieName => Options.SessionCookie.Name;
    }
}
