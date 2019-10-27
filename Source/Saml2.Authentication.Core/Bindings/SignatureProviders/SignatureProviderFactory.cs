namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Xml;
    using dk.nita.saml20;

    internal class SignatureProviderFactory : ISignatureProviderFactory
    {
        /// <summary>
        /// returns the validated <see cref="ShaHashingAlgorithm"/>
        /// </summary>
        /// <returns>ShaHashingAlgorithm</returns>
        public ShaHashingAlgorithm ValidateShaHashingAlgorithm(string shaHashingAlgorithm)
        {
            if (Enum.TryParse(shaHashingAlgorithm, out ShaHashingAlgorithm val) && Enum.IsDefined(typeof(ShaHashingAlgorithm), val))
            {
                return val;
            }

            throw new InvalidOperationException($"The value of the configuration element 'ShaHashingAlgorithm' is not valid: '{shaHashingAlgorithm}'. Value must be either SHA1, SHA256 or SHA512");
        }

        public ISignatureProvider CreateFromAlgorithmUri(Type signingKeyType, string algorithmUri)
        {
            if (signingKeyType.IsSubclassOf(typeof(RSA)))
            {
                return algorithmUri switch
                {
                    SignedXml.XmlDsigRSASHA1Url => new RsaSha1SignatureProvider(),
                    Saml2Constants.XmlDsigRSASHA256Url => new RsaSha256SignatureProvider(),
                    Saml2Constants.XmlDsigRSASHA512Url => new RsaSha512SignatureProvider(),
                    _ => throw new InvalidOperationException($"Unsupported hashing algorithm uri '{algorithmUri}' provided while using RSA signing key"),
                };
            }

            if (signingKeyType.IsSubclassOf(typeof(DSA)))
            {
                return new DsaSha1SignatureProvider();
            }

            throw new InvalidOperationException($"The signing key type {signingKeyType.FullName} is not supported by OIOSAML.NET. It must be either a DSA or RSA key.");
        }

        public ISignatureProvider CreateFromAlgorithmName(Type signingKeyType, ShaHashingAlgorithm hashingAlgorithm)
        {
            if (signingKeyType.IsSubclassOf(typeof(RSA)))
            {
                return hashingAlgorithm switch
                {
                    ShaHashingAlgorithm.SHA1 => new RsaSha1SignatureProvider(),
                    ShaHashingAlgorithm.SHA256 => new RsaSha256SignatureProvider(),
                    ShaHashingAlgorithm.SHA512 => new RsaSha512SignatureProvider(),
                    _ => throw new InvalidOperationException($"Unsupported hashing algorithm '{hashingAlgorithm}' provideded while using RSA signing key"),
                };
            }

            if (signingKeyType.IsSubclassOf(typeof(DSA)))
            {
                return new DsaSha1SignatureProvider();
            }

            throw new InvalidOperationException($"The signing key type {signingKeyType.FullName} is not supported by OIOSAML.NET. It must be either a DSA or RSA key.");
        }
    }
}