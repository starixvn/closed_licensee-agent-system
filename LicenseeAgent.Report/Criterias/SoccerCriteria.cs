// -----------------------------------------------------------------------
// <copyright file="TotalBetsSoccerCriteria.cs" company="">
// TotalBetsSoccerCriteria.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TotalBetsSoccerCriteria.
    /// </summary>
    public class SoccerCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetByAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
