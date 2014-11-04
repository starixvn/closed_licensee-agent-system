namespace LicenseeAgent.MemberReArrangement.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Legend Agent Id Has Deposit Criteria.
    /// </summary>
    public class GetLegendAgentIdHasDepositCriteria : CriteriaBase
    {
        public string AgentName { get; set; }

        public string MemberName { get; set; }

        public int LicenseeId { get; set; }

        public int LicenseeCurrencyId { get; set; }

        public string SiteName { get; set; }

        public int LegendAgentId { get; set; }

        public int IBCAgentId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Cash_GetLegendAgentIdHasDeposit";
        }

        public override bool IsValid()
        {            
            return !string.IsNullOrEmpty(MemberName);
        }
    }
}
