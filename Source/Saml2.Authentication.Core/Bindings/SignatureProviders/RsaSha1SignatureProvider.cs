using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using dk.nita.saml20.Bindings.SignatureProviders;

namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    internal class RsaSha1SignatureProvider : ISignatureProvider
    {
        public string SignatureUri => SignedXml.XmlDsigRSASHA1Url;
        public byte[] SignData(AsymmetricAlgorithm key, byte[] data)
        {
            var rsa = (RSA) key;
            return rsa.SignData(data, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        public bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature)
        {
            var hash = new SHA1Managed().ComputeHash(data);
            return ((RSA) key).VerifyHash(hash, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

    }
}