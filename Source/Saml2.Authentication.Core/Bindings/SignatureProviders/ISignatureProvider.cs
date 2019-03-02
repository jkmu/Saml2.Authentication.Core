namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    using System.Security.Cryptography;

    internal interface ISignatureProvider
    {
        string SignatureUri { get; }

        byte[] SignData(AsymmetricAlgorithm key, byte[] data);

        bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature);
    }
}