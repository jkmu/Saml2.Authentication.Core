namespace Saml2.Authentication.Core.Authentication
{
    public class Saml2ClaimTypes
    {
        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static readonly string Subject = $"${Namespace}subject";

        public static readonly string SessionIndex = $"{Namespace}session_index";

        private const string Namespace = "http://saml2.authentication.core/";
    }
}
