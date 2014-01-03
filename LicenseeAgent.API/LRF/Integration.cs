// --------------------------------------------------------------------------
// <copyright file="Integration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.API
{
    using LicenseeAgent.API.LRF.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Integration
    {
        public static string GenerateLink(int userId, int licenseeId)
        {
            var loginInfo = new LoginInfo
            {
                UserId = userId,
                LicenseeId = licenseeId
            };

            return null;
        }
    }
}