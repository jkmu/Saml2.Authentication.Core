using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Saml2.Authentication.Core.Session
{
    public interface ISessionStore
    {
        Task<T> LoadAsync<T>();

        Task SaveAsync<T>(object session);

        Task RemoveAsync<T>();

        Task RefreshAsync<T>();
    }
}
