namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    using System.Security.Cryptography;
    using System.Security.Cryptography.Xml;

    internal class DsaSha1SignatureProvider : ISignatureProvider
    {
        public string SignatureUri => SignedXml.XmlDsigDSAUrl;

        public byte[] SignData(AsymmetricAlgorithm key, byte[] data)
        {
            return ((DSA)key).SignData(data, HashAlgorithmName.SHA1);
        }

        public bool VerifySignature(AsymmetricAlgorithm key, byte[] data, byte[] signature)
        {
            using var sha1Managed = new SHA1Managed();
            return ((DSA)key).VerifySignature(sha1Managed.ComputeHash(data), signature);
        }
    }
}