namespace LicenseeAgent.Report.Criteria.AgentWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// WinLossMemberCriteria class.
    /// </summary>
    public class WinLossMemberViewCriteria : WinlossBase
    {
        public WinLossMemberViewCriteria(
            int agentId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            DateTime balanceUpDay,
            int drawType,
            string productIds)
            : base(agentId, fromDate, toDate, moveBetsDay, balanceUpDay, drawType, productIds)
        {
        }

        public override string GetSettingKey()
        {
            return "LAS_Rpt_Winloss_DirectMem";
        }
    }
}
