namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    using System.Security.Cryptography;
    using System.Security.Cryptography.Xml;

    internal class RsaSha1SignatureProvider : ISignatureProvider
    {
        public string SignatureUri => SignedXml.XmlDsigRSASHA1Url;

        public byte[] SignData(AsymmetricAlgorithm key, byte[] data)
        {
            var rsa = (RSA)key;
            return rsa.SignData(data, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        public bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature)
        {
            using var shaManaged = new SHA1Managed();
            return ((RSA)key).VerifyHash(shaManaged.ComputeHash(data), signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

    }
}