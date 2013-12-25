// --------------------------------------------------------------------------
// <copyright file="TotalBet_Soccer_MajorBetTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.12.25">Created</change>
// </history>

namespace LicenseeAgent.TotalBets.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class TotalBet_Soccer_MajorBetTypeCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public string LeagueList { get; set; }

        public string MatchList { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_TotalBet_Basketball_MajorBetType";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
