namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    using System;

    internal interface ISignatureProviderFactory
    {
        ISignatureProvider CreateFromAlgorithmName(Type signingKeyType, ShaHashingAlgorithm hashingAlgorithm);

        ISignatureProvider CreateFromAlgorithmUri(Type signingKeyType, string algorithmUri);

        ShaHashingAlgorithm ValidateShaHashingAlgorithm(string shaHashingAlgorithm);
    }
}