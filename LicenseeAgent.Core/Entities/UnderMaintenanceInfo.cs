// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.3">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.17">add more property UMDate</change>
// </history>

namespace LicenseeAgent.Core.Entities
{
    using System;

    /// <summary>
    /// Under Maintenance Page Model.
    /// </summary>
    public class UnderMaintenanceInfo
    {
        public UnderMaintenanceInfo()
        {
            Status = false;
            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            UmTimeFrom = nowHour.ToShortTimeString();
            UmTimeTo = nowHour.AddHours(1).ToShortTimeString();
        }

        public bool Status { get; set; }

        public string UmTimeFrom { get; set; }

        public string UmTimeTo { get; set; }
    }
}