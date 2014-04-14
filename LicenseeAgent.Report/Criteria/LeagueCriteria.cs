// ----------------------------------------------------------------------
// <copyright file="LeagueCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/01/16">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// League Criteria.
    /// </summary>
    public class LeagueCriteria : CriteriaBase
    {
        public bool IsShowAll { get; set; }

        public DateTime EventDate { get; set; }

        public int SportType { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Result_LeagueSelByDate";
        }

        public override bool IsValid()
        {
            return SportType > 0;
        }
    }
}
