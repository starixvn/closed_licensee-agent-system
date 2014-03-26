// ----------------------------------------------------------------------
// <copyright file="BetListResultCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Forrest.Nguyen" date="2014/03/26">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.Criteria
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListResultCriteria
    {
        public bool IsSportsbook { get; set; }

        public int MatchId { get; set; }

        public long RefNo { get; set; }

        public int SportType { get; set; }
    }
}
