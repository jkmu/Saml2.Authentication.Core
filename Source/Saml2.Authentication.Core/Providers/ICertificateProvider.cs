namespace Saml2.Authentication.Core.Providers
{
    internal interface ICertificateProvider
    {
        SigningCertificate GetCertificate();
    }
}