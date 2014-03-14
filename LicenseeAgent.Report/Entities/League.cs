// ----------------------------------------------------------------------
// <copyright file="League.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/16">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Entities
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class League
    {
        public string LeagueName { get; set; }

        public int LeagueID { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }
    }
}
