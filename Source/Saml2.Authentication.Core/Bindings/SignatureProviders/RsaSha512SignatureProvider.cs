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
            using var shaManaged = new SHA512Managed();
            return ((RSA)key).VerifyHash(shaManaged.ComputeHash(data), signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        }
    }
}