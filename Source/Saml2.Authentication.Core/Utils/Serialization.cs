using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace dk.nita.saml20.Utils
{
    /// <summary>
    /// Functions for typed serialization and deserialization of objects.
    /// </summary>
    public static class Serialization
    {
        private static readonly XmlSerializerNamespaces _xmlNamespaces = null;

        static Serialization()
        {
            _xmlNamespaces = new XmlSerializerNamespaces();
            _xmlNamespaces.Add("", "");
        }


        /// <summary>
        /// Gets the instance of XmlSerializerNamespaces that is used by this class.
        /// </summary>
        /// <value>The XmlSerializerNamespaces instance.</value>
        public static XmlSerializerNamespaces XmlNamespaces
        {
            get { return _xmlNamespaces; }
        }

        /// <summary>
        /// Serializes the specified item to a stream.
        /// </summary>
        /// <typeparam name="T">The items type</typeparam>
        /// <param name="item">The item to serialize.</param>
        /// <param name="stream">The stream to serialize to.</param>
        public static void Serialize<T>(T item, Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, item, _xmlNamespaces);
            stream.Flush();
        }

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <typeparam name="T">The items type</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>An XmlDocument containing the serialized form of the item</returns>
        public static XmlDocument Serialize<T>(T item)
        {
            MemoryStream stream = new MemoryStream();
            Serialize(item, stream);

            // create the XmlDocument to return
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            stream.Seek(0, SeekOrigin.Begin);
            doc.Load(stream);

            stream.Close();

            return doc;
        }

        /// <summary>
        /// Serializes an item to an XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static string SerializeToXmlString<T>(T item)
        {
            MemoryStream stream = new MemoryStream();
            Serialize(item, stream);

            StreamReader reader = new StreamReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Deserializes an item from an XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static T DeserializeFromXmlString<T>(string xml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(xml));
            reader.XmlResolver = null;
            return Deserialize<T>(reader);
        }

        /// <summary>
        /// Reads and deserializes an item from the reader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static T Deserialize<T>(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T item = (T)serializer.Deserialize(reader);

            return item;
        }
    }
}