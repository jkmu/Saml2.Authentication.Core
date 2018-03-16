using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace dk.nita.saml20.Bindings.SignatureProviders
{
    internal class DsaSha1SignatureProvider : ISignatureProvider
    {
        public string SignatureUri => SignedXml.XmlDsigDSAUrl;
        public byte[] SignData(AsymmetricAlgorithm key, byte[] data)
        {
            return ((DSACryptoServiceProvider) key).SignData(data);
        }

        public bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature)
        {
            var hash = new SHA1Managed().ComputeHash(data);
            return ((DSACryptoServiceProvider)key).VerifySignature(hash, signature);
        }
    }
}