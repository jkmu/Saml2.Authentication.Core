using System.Security.Cryptography;

namespace dk.nita.saml20.Bindings.SignatureProviders
{
    internal class RsaSha512SignatureProvider : ISignatureProvider
    {
        public string SignatureUri => Saml20Constants.XmlDsigRSASHA512Url;
        public byte[] SignData(AsymmetricAlgorithm key, byte[] data)
        {
            var rsa = (RSACryptoServiceProvider)key;
            return rsa.SignData(data, new SHA512CryptoServiceProvider());
        }

        public bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature)
        {
            var hash = new SHA512Managed().ComputeHash(data);
            return ((RSACryptoServiceProvider)key).VerifyHash(hash, "SHA512", signature);
        }
    }
}