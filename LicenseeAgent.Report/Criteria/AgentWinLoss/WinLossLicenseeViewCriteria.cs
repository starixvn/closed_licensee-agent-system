namespace LicenseeAgent.Report.Criteria.AgentWinLoss
{
    using System;

    /// <summary>
    /// Win Loss Super View Agent Criteria.
    /// </summary>
    public class WinLossLicenseeViewCriteria : WinlossBase
    {
        public WinLossLicenseeViewCriteria(
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
            return "LAS_Rpt_Winloss_LicenseeWL";
        }
    }
}