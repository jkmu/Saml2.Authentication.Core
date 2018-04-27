using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;

namespace Saml2.Authentication.Core.Bindings
{
    public interface IHttpArtifactBinding
    {
        bool IsValid(HttpRequest request);

        string GetArtifact(HttpRequest request);

        /// <summary>
        ///     Resolves an artifact.
        /// </summary>
        /// <returns>A stream containing the artifact response from the IdP</returns>
        Stream ResolveArtifact(string artifact, string artifactResolveEndpoint, string serviceProviderId,
            X509Certificate2 cert);

        string GetRelayState(HttpRequest contextRequest);
    }
}