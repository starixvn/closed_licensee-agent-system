// ----------------------------------------------------------------------
// <copyright file="MemberFilter.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Filter
{
    using System;
    using System.Linq;
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// MemberFilter class.
    /// </summary>
    public class MemberFilter : IServiceFilter
    {
        /// <summary>
        /// Check two object same key property.
        /// </summary>
        /// <typeparam name="T1">Type object source.</typeparam>
        /// <typeparam name="T2">Type object destination.</typeparam>
        /// <param name="source">Object source.</param>
        /// <param name="destination">Object destination.</param>
        /// <returns>They are same or different.</returns>
        public bool IsEquals<T1, T2>(T1 source, T2 destination)
        {
            var sourceProperty = source.GetType().GetProperty("IBCCustId");
            var destinationProperties = destination.GetType().GetProperties();

            if (sourceProperty == null)
            {
                return false;
            }

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
                    return Convert.ToInt32(destinationProperty.GetValue(destination, null)) == Convert.ToInt32(sourceProperty.GetValue(source, null));
                }
            }

            return false;
        }
    }
}
