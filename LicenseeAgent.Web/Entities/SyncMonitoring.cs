// ----------------------------------------------------------------------
// <copyright file="SyncMonitoring.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/04/22">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Entities
{
    using System;

    /// <summary>
    /// Monitoring Data's synchronization.
    /// </summary>
    public class SyncMonitoring
    {
        public int KeyId { get; set; }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string ActionName { get; set; }

        public int ActionId { get; set; }

        public string Command { get; set; }

        public DateTime LogDate { get; set; }

        public string InputParams { get; set; }

        public string Remark { get; set; }

        public string Url { get; set; }

        public int RunCount { get; set; }
    }
}