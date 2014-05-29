// -----------------------------------------------------------------------
// <copyright file="RegLib.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class RegLib
    {
        /// <summary>
        /// Validates the list of integer.
        /// Valid: "1", "1,2", "*", ...
        /// Invalid: "1,", "1a,2", ...
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns true if match.
        /// </returns>
        public static bool ValidateListOfInt(string value)
        {
            var regex = new Regex(@"^((\*)|([0-9]+(,[0-9]+)*))$");
            return regex.IsMatch(value);
        }

        /// <summary>
        /// Validates the list of string.
        /// Valid: "1", "1,2", "*", ...
        /// Invalid: "1,", "1a,2", ...
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns true if match.</returns>
        public static bool ValidateListOfString(string value)
        {
            var regex = new Regex(@"^((\*)|([0-9a-zA-Z]+(,[0-9a-zA-Z]+)*))$");
            return regex.IsMatch(value);
        }
    }
}
