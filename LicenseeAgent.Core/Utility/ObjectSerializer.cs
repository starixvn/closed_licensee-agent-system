// -----------------------------------------------------------------------
// <copyright file="ObjectSerializer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// ObjectSerializer class.
    /// </summary>
    /// <typeparam name="T">T is a class used to serialize.</typeparam>
    public static class ObjectSerializer<T> where T : class
    {
        private static XmlSerializerNamespaces xmlNameSpace;
        private static XmlWriterSettings xmlSettings;

        static ObjectSerializer()
        {
            xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add(string.Empty, string.Empty);

            xmlSettings = new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineHandling = NewLineHandling.None
            };
        }

        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">The object list.</param>
        /// <returns>Returns XML string.</returns>
        public static string SerializeList(IList<T> list)
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
        /// <param name="xml">The XML string.</param>
        /// <returns>Return list.</returns>
        public static IList<T> DeserializeList(string xml)
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
        /// <param name="obj">The object.</param>
        /// <returns>
        /// Returns a XML string.
        /// </returns>
        public static string Serialize(T obj)
        {
            var xmlSerializer = new XmlSerializer(obj.GetType());
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
                {
                    xmlSerializer.Serialize(xmlWriter, obj, xmlNameSpace);
                    return stringWriter.ToString();
                }
            }
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>Return list.</returns>
        public static T Deserialize(string xml)
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
    }
}