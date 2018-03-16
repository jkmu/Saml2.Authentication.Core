using System;
using dk.nita.saml20.Bindings.SignatureProviders;
using dk.nita.saml20.config;

namespace Saml2.Authentication.Core.Bindings.SignatureProviders
{
    internal interface ISignatureProviderFactory
    {
        ISignatureProvider CreateFromAlgorithmName(Type signingKeyType, ShaHashingAlgorithm hashingAlgorithm);

        ISignatureProvider CreateFromAlgorithmUri(Type signingKeyType, string algorithmUri);

        ShaHashingAlgorithm ValidateShaHashingAlgorithm(string shaHashingAlgorithm);
    }
}