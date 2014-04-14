// ----------------------------------------------------------------------
// <copyright file="IServiceFilter.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Filter
{
    public interface IServiceFilter
    {
        /// <summary>
        /// Check two object same key property.
        /// </summary>
        /// <typeparam name="T1">Type object source.</typeparam>
        /// <typeparam name="T2">Type object destination.</typeparam>
        /// <param name="source">Object source.</param>
        /// <param name="destination">Object destination.</param>
        /// <returns>They are same or different.</returns>
        bool IsEquals<T1, T2>(T1 source, T2 destination);
    }
}
