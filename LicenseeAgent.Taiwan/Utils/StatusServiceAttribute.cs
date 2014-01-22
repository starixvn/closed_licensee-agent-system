// ----------------------------------------------------------------------
// <copyright file="StatusServiceAttribute.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utils
{
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StatusServiceAttribute : ServiceAttribute, IDataParser
    {
        public object Parse(object value)
        {
            if (value is bool)
            {
                var isClosed = (bool)value;
                return isClosed ? AccountStatus.Closed : AccountStatus.Open;
            }

            return null;
        }
    }
}