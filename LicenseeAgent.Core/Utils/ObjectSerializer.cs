// -----------------------------------------------------------------------
// <copyright file="ObjectSerializer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// <typeparam name="T">T is a class used to serialize.</typeparam>
    public static class ObjectSerializer<T> where T : class
    {
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
                var serializer = new XmlSerializer(list.GetType(), new XmlRootAttribute("root"));
                serializer.Serialize(stringWriter, list);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>Return list.</returns>
        public static List<T> DeserializeList(string xml)
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
    }
}