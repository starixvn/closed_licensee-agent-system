// ----------------------------------------------------------------------
// <copyright file="ServiceMapper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Taiwan.Filter;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class ServiceMapper
    {
        /// <summary>
        /// Map data of source to destination.
        /// </summary>
        /// <typeparam name="T1">Type source.</typeparam>
        /// <typeparam name="T2">Type destination.</typeparam>
        /// <param name="source">Source entity.</param>
        /// <param name="destination">Destination entity.</param>
        public static void MapObject<T1, T2>(T1 source, T2 destination)
        {
            if (source == null || destination == null)
            {
                return;
            }

            var sourceProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();
            
            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var destinationProperty in destinationProperties)
                {
                    var propertyName = destinationProperty.Name;
                    var attribute = destinationProperty.GetCustomAttributes(typeof(ServiceAttribute), true).FirstOrDefault();

                    if (attribute != null)
                    {
                        propertyName = ((ServiceAttribute)attribute).Name;
                    }

                    if (destinationProperty.CanWrite && propertyName.Equals(sourceProperty.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                    }
                }
            }
        }

        /// <summary>
        /// Map data of sources to compatible destinations.
        /// </summary>
        /// <typeparam name="T1">Type source.</typeparam>
        /// <typeparam name="T2">Type destination.</typeparam>
        /// <param name="sources">List Source entity.</param>
        /// <param name="destinations">List Destination entity.</param>
        /// <param name="serviceFilter">Service filter.</param>
        public static void MapObjects<T1, T2>(IEnumerable<T1> sources, IEnumerable<T2> destinations, IServiceFilter serviceFilter)
        {
            foreach (var destination in destinations)
            {
                var source = (from s in sources
                              where serviceFilter.IsEquals<T1, T2>(s, destination) 
                              select s).FirstOrDefault();

                ServiceMapper.MapObject<T1, T2>(source, destination);
            }
        }
    }
}
