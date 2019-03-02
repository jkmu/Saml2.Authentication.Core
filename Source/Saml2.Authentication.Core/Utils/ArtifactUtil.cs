namespace dk.nita.saml20.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Contains functions to generate and parse artifacts, as defined in "Bindings for the OASIS 
    /// Security Assertion Markup Language (SAML) v. 2.0" specification.
    /// </summary>
    public class ArtifactUtil
    {
        /// <summary>
        /// Length of source id
        /// </summary>
        public const int SourceIdLength = 20;

        /// <summary>
        /// Length of message handle
        /// </summary>
        public const int MessageHandleLength = 20;

        private const string ArgumentLengthErrorFmt = "Unexpected length of byte[] parameter: {0}. Should be {1}";
        private const int ArtifactLength = 44;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeCodeValue"></param>
        /// <param name="endpointIndexValue"></param>
        /// <param name="sourceIdHash"></param>
        /// <param name="messageHandle"></param>
        /// <returns>A Base64 encoded string containing the artifact</returns>
        public static string CreateArtifact(short typeCodeValue, short endpointIndexValue, byte[] sourceIdHash, byte[] messageHandle)
        {
            if (sourceIdHash.Length != SourceIdLength)
            {
                throw new ArgumentException(
                    string.Format(ArgumentLengthErrorFmt, sourceIdHash.Length, SourceIdLength), nameof(sourceIdHash));
            }

            if (messageHandle.Length != MessageHandleLength)
            {
                throw new ArgumentException(
                    string.Format(ArgumentLengthErrorFmt, messageHandle.Length, MessageHandleLength), nameof(messageHandle));
            }

            byte[] typeCode = new byte[2];
            typeCode[0] = (byte)(typeCodeValue >> 8);
            typeCode[1] = (byte)typeCodeValue;

            byte[] endpointIndex = new byte[2];
            endpointIndex[0] = (byte)(endpointIndexValue >> 8);
            endpointIndex[1] = (byte)endpointIndexValue;

            byte[] result = new byte[2 + 2 + SourceIdLength + MessageHandleLength];

            typeCode.CopyTo(result, 0);
            endpointIndex.CopyTo(result, 2);
            sourceIdHash.CopyTo(result, 4);
            messageHandle.CopyTo(result, 4 + SourceIdLength);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artifact"></param>
        /// <param name="typeCodeValue"></param>
        /// <param name="endpointIndex"></param>
        /// <param name="sourceIdHash"></param>
        /// <param name="messageHandle"></param>
        public static void ParseArtifact(string artifact, ref short typeCodeValue, ref short endpointIndex, ref byte[] sourceIdHash, ref byte[] messageHandle)
        {
            if (sourceIdHash.Length != SourceIdLength)
            {
                throw new ArgumentException(
                    string.Format(ArgumentLengthErrorFmt, sourceIdHash.Length, SourceIdLength), nameof(sourceIdHash));
            }

            if (messageHandle.Length != MessageHandleLength)
            {
                throw new ArgumentException(
                    string.Format(ArgumentLengthErrorFmt, messageHandle.Length, MessageHandleLength), nameof(messageHandle));
            }

            byte[] bytes = Convert.FromBase64String(artifact);

            if (bytes.Length != ArtifactLength)
            {
                throw new ArgumentException("Unexpected artifact length", nameof(artifact));
            }

            typeCodeValue = (short)(bytes[0] << 8 | bytes[1]);

            endpointIndex = (short)(bytes[2] << 8 | bytes[3]);

            var index = 4;

            for (var i = 0; i < SourceIdLength; i++)
            {
                sourceIdHash[i] = bytes[i + index];
            }

            index += SourceIdLength;

            for (var i = 0; i < MessageHandleLength; i++)
            {
                messageHandle[i] = bytes[i + index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artifact"></param>
        /// <param name="typeCodeValue"></param>
        /// <param name="endpointIndex"></param>
        /// <param name="sourceIdHash"></param>
        /// <param name="messageHandle"></param>
        /// <returns>bool</returns>
        public static bool TryParseArtifact(string artifact, ref short typeCodeValue, ref short endpointIndex, ref byte[] sourceIdHash, ref byte[] messageHandle)
        {
            try
            {
                ParseArtifact(artifact, ref typeCodeValue, ref endpointIndex, ref sourceIdHash, ref messageHandle);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieves the endpoint index from an artifact
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        /// <returns>ushort</returns>
        public static ushort GetEndpointIndex(string artifact)
        {
            short parsedTypeCode = -1;
            short parsedEndpointIndex = -1;
            byte[] parsedSourceIdHash = new byte[20];
            byte[] parsedMessageHandle = new byte[20];

            if (TryParseArtifact(artifact, ref parsedTypeCode, ref parsedEndpointIndex, ref parsedSourceIdHash, ref parsedMessageHandle))
            {
                return (ushort)parsedEndpointIndex;
            }

            throw new ArgumentException("Malformed artifact", nameof(artifact));
        }

        public static byte[] GenerateMessageHandle()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] messageHandle = new byte[MessageHandleLength];
            rng.GetNonZeroBytes(messageHandle);

            return messageHandle;
        }

        public static byte[] GenerateSourceIdHash(string sourceIdUrl)
        {
            var sha = SHA1.Create();

            var sourceId = sha.ComputeHash(Encoding.ASCII.GetBytes(sourceIdUrl));

            return sourceId;
        }
    }
}