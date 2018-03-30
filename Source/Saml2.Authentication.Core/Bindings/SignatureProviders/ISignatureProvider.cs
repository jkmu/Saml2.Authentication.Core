using System.Security.Cryptography;

namespace dk.nita.saml20.Bindings.SignatureProviders
{
    internal interface ISignatureProvider
    {
        string SignatureUri { get; }

        byte[] SignData(AsymmetricAlgorithm key, byte[] data);

        bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature);
    }
}