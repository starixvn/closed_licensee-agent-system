namespace LicenseeAgent.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// ObjectSerializer class.
    /// </summary>
    public static class ObjectSerializer
    {
        private static XmlSerializerNamespaces xmlNameSpace = XmlSerializerNamespaces();

        private static XmlWriterSettings xmlSettings = new XmlWriterSettings
        {
            Indent = false,
            OmitXmlDeclaration = true,
            NewLineHandling = NewLineHandling.None
        };

        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="list">The object list.</param>
        /// <returns>
        /// Returns XML string.
        /// </returns>
        public static string SerializeList<T>(IList<T> list)
        {
            if (list == null)
            {
                return string.Empty;
            }

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
                {
                    var serializer = new XmlSerializer(list.GetType(), new XmlRootAttribute("root"));
                    serializer.Serialize(xmlWriter, list, xmlNameSpace);
                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="xml">The XML string.</param>
        /// <returns>
        /// Return list.
        /// </returns>
        public static IList<T> DeserializeList<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new List<T>();
            }

            using (var stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute("root"));
                return (List<T>)serializer.Deserialize(stringReader);
            }
        }

        /// <summary>
        /// Serializes the specified object to Xml string.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">The object.</param>
        /// <returns>
        /// Returns a XML string.
        /// </returns>
        public static string Serialize<T>(T value)
        {
            var xmlSerializer = new XmlSerializer(value.GetType());
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
                {
                    xmlSerializer.Serialize(xmlWriter, value, xmlNameSpace);
                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="xml">The XML string.</param>
        /// <returns>
        /// Return list.
        /// </returns>
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            using (var stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <param name="type">The object type.</param>
        /// <returns>Returns XML object.</returns>
        public static object Deserialize(string xml, Type type)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            using (var stringReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(stringReader);
            }
        }

        private static XmlSerializerNamespaces XmlSerializerNamespaces()
        {
            var xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add(string.Empty, string.Empty);
            return xmlNameSpace;
        }
    }
}