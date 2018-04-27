using dk.nita.saml20.Schema.Protocol;

namespace Saml2.Authentication.Core.Bindings
{
    public class Saml2LogoutResponse
    {
        public LogoutRequest OriginalLogoutRequest { get; set; }

        public string StatusCode { get; set; }
    }
}