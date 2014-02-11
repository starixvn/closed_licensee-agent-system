// ----------------------------------------------------------------------
// <copyright file="ServiceExtension.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Check is updated multiple status member.
        /// </summary>
        /// <param name="IBCCustId">List member ID.</param>
        /// <param name="LASAgentId">List Agent ID.</param>
        /// <returns>If is multiple return true, elsereturn false.</returns>
        public static bool IsUpdateMultipleStatus(string IBCCustId)
        {
            if (!string.IsNullOrEmpty(IBCCustId) && IBCCustId.Split(',').Count() <= 1)
            {
                return false;
            }

            return true;
        }
    }
}
