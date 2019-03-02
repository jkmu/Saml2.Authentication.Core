namespace Saml2.Authentication.Core.Bindings
{
    using dk.nita.saml20.Schema.Protocol;

    public class Saml2LogoutResponse
    {
        public LogoutRequest OriginalLogoutRequest { get; set; }

        public string StatusCode { get; set; }
    }
}