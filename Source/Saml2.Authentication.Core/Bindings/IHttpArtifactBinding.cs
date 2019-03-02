namespace Saml2.Authentication.Core.Bindings
{
    using System.IO;

    public interface IHttpArtifactBinding
    {
        bool IsValid();

        /// <summary>
        ///     Resolves an artifact.
        /// </summary>
        /// <returns>A stream containing the artifact response from the IdP</returns>
        Stream ResolveArtifact();
    }
}