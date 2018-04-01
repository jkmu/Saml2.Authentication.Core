namespace Saml2.Authentication.Core.Providers
{
    public interface ICertificateProvider
    {
        SigningCertificate GetCertificate();
    }
}