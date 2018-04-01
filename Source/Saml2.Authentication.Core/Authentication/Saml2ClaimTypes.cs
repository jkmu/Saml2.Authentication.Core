namespace Saml2.Authentication.Core.Authentication
{
    public static class Saml2ClaimTypes
    {
        internal static string Namespace = "http://saml2.authentication.core/";

        public static string Subject = $"${Namespace}subject";

        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static string SessionIndex = $"{Namespace}session_index";
    }
}
