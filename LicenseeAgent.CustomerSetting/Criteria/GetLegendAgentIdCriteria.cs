namespace LicenseeAgent.CustomerSetting.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Legend Agent Id Criteria.
    /// </summary>
    public class GetLegendAgentIdCriteria : CriteriaBase
    {
        public string AgentName { get; set; }

        public int LicenseeId { get; set; }

        public int LicenseeCurrencyId { get; set; }

        public int MemberCurrencyId { get; set; }

        public string SiteName { get; set; }

        public int LegendAgentId { get; set; }

        public int IBCAgentId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Cash_GetLegendAgentId";
        }

        public override bool IsValid()
        {
            return MemberCurrencyId != 0;
        }
    }
}
