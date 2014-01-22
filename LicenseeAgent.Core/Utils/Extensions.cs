// --------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utils
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Extensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            return string.IsNullOrEmpty(toCheck) ? true : source.IndexOf(toCheck, comparison) >= 0;
        }
    }
}