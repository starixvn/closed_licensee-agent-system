// --------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Extensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            return string.IsNullOrEmpty(toCheck) ? true : source.IndexOf(toCheck, comparison) >= 0;
        }

        public static string FormatCustomerName(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return string.Format("{0} {1}", firstName, lastName);
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                return firstName;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                return lastName;
            }

            return Constants.EmptyString;
        }
    }
}