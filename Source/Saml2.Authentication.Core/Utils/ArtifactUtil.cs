using System;
using System.Security.Cryptography;
using System.Text;

namespace dk.nita.saml20.Utils
{
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

        private const string argumentLengthErrorFmt = "Unexpected length of byte[] parameter: {0}. Should be {1}";
        private const int artifactLength = 44;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeCodeValue"></param>
        /// <param name="endpointIndexValue"></param>
        /// <param name="sourceIdHash"></param>
        /// <param name="messageHandle"></param>
        /// <returns>A Base64 encoded string containing the artifact</returns>
        public static string CreateArtifact(Int16 typeCodeValue, Int16 endpointIndexValue, byte[] sourceIdHash, byte[] messageHandle)
        {
            if (sourceIdHash.Length != SourceIdLength)
                throw new ArgumentException(
                    string.Format(argumentLengthErrorFmt, sourceIdHash.Length,SourceIdLength), "sourceIdHash");

            if (messageHandle.Length != MessageHandleLength)
                throw new ArgumentException(
                    string.Format(argumentLengthErrorFmt, messageHandle.Length, MessageHandleLength), "messageHandle");

            byte[] typeCode = new byte[2];
            typeCode[0] = (byte)(typeCodeValue >> 8);
            typeCode[1] = (byte)(typeCodeValue);
            
            
            byte[] endpointIndex = new byte[2];
            endpointIndex[0] = (byte)(endpointIndexValue >> 8);
            endpointIndex[1] = (byte)(endpointIndexValue);

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
        public static void ParseArtifact(string artifact, ref Int16 typeCodeValue, ref Int16 endpointIndex, ref byte[] sourceIdHash, ref byte[] messageHandle)
        {
            if (sourceIdHash.Length != SourceIdLength)
                throw new ArgumentException(
                    string.Format(argumentLengthErrorFmt, sourceIdHash.Length, SourceIdLength), "sourceIdHash");

            if (messageHandle.Length != MessageHandleLength)
                throw new ArgumentException(
                    string.Format(argumentLengthErrorFmt, messageHandle.Length, MessageHandleLength), "messageHandle");

            byte[] bytes = Convert.FromBase64String(artifact);

            if (bytes.Length != artifactLength)
                throw new ArgumentException("Unexpected artifact length", "artifact");

            typeCodeValue = (Int16)(bytes[0] << 8 | bytes[1]);

            endpointIndex = (Int16)(bytes[2] << 8 | bytes[3]);

            int index = 4;

            for(int i = 0; i < SourceIdLength; i++)
            {
                sourceIdHash[i] = bytes[i + index];
            }

            index += SourceIdLength;

            for (int i = 0; i < MessageHandleLength; i++)
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
        /// <returns></returns>
        public static bool TryParseArtifact(string artifact, ref Int16 typeCodeValue, ref Int16 endpointIndex, ref byte[] sourceIdHash, ref byte[] messageHandle)
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
        /// <returns></returns>
        public static ushort GetEndpointIndex(string artifact)
        {
            Int16 parsedTypeCode = -1;
            Int16 parsedEndpointIndex = -1;
            byte[] parsedSourceIdHash = new byte[20];
            byte[] parsedMessageHandle = new byte[20];

            if(TryParseArtifact(artifact, ref parsedTypeCode, ref parsedEndpointIndex, ref parsedSourceIdHash, ref parsedMessageHandle))
            {
                return (ushort) parsedEndpointIndex;   
            }

            throw new ArgumentException("Malformed artifact", "artifact");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateMessageHandle()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] messageHandle = new byte[MessageHandleLength];
            rng.GetNonZeroBytes(messageHandle);

            return messageHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceIdUrl"></param>
        /// <returns></returns>
        public static byte[] GenerateSourceIdHash(string sourceIdUrl)
        {
            SHA1 sha = SHA1Managed.Create();

            byte[] sourceId = sha.ComputeHash(Encoding.ASCII.GetBytes(sourceIdUrl));

            return sourceId;
        }
    }
}