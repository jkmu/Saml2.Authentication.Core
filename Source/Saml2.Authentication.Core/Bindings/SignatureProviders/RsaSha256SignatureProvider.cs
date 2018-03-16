using System.Security.Cryptography;

namespace dk.nita.saml20.Bindings.SignatureProviders
{
    internal class RsaSha256SignatureProvider : ISignatureProvider
    {
        public string SignatureUri => Saml20Constants.XmlDsigRSASHA256Url;
        public byte[] SignData(AsymmetricAlgorithm key, byte[] data)
        {
            var rsa = (RSACryptoServiceProvider)key;
            return rsa.SignData(data, new SHA256CryptoServiceProvider());
        }

        public bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature)
        {
            var hash = new SHA256Managed().ComputeHash(data);
            return ((RSACryptoServiceProvider)key).VerifyHash(hash, "SHA256", signature);
        }
    }
}