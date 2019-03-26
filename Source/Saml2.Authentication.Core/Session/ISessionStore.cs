namespace Saml2.Authentication.Core.Session
{
    using System.Threading.Tasks;

    public interface ISessionStore
    {
        Task<T> LoadAsync<T>();

        Task SaveAsync<T>(object session);

        Task RemoveAsync<T>();

        Task RefreshAsync<T>();
    }
}
