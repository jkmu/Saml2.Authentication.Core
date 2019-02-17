using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Saml2.Authentication.Core.Authentication;

namespace Saml2.Authentication.Core.Session
{
    public class AspNetSessionStorage : ISessionStore
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetSessionStorage(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpContext Context => _httpContextAccessor.HttpContext;

        public Task<T> LoadAsync<T>()
        {
            var key = GetSessionKey(typeof(T));
            if (Context.Session.TryGetValue(key, out var session) && session?.Length > 0)
            {
                return Task.FromResult(JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(session)));
            }
            return Task.FromResult(default(T));
        }

        public Task SaveAsync<T>(object session)
        {
            var key = GetSessionKey(typeof(T));
            Context.Session.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(session)));

            return Task.CompletedTask;
        }

        public Task RemoveAsync<T>()
        {
            var key = GetSessionKey(typeof(T));
            Context.Session.Remove(key);
            return Task.CompletedTask;
        }

        public Task RefreshAsync<T>()
        {
            var key = GetSessionKey(typeof(T));
            Context.Session.TryGetValue(key, out var _);
            return Task.CompletedTask;
        }

        private static string GetSessionKey(MemberInfo type) => $"{Saml2Defaults.SessionKeyPrefix}.{type.Name}";
    }
}
