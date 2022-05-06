namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    using System.Security.Cryptography;
    using dk.nita.saml20;

    internal class RsaSha512SignatureProvider : ISignatureProvider
    {
        public string SignatureUri => Saml2Constants.XmlDsigRSASHA512Url;

        public byte[] SignData(AsymmetricAlgorithm key, byte[] data)
        {
            var rsa = (RSA)key;
            return rsa.SignData(data, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        }

        public bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature)
        {
            var hash = SHA512.Create().ComputeHash(data);
            return ((RSA)key).VerifyHash(hash, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        }
    }
}