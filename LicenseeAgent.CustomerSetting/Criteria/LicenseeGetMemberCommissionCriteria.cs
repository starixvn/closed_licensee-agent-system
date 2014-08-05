namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LicenseeGetMemberCommissionCriteria class.
    /// </summary>
    public class LicenseeGetMemberCommissionCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int AdminId { get; set; }

        public int CurrencyId { get; set; }

        public string UserName { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int GroupCommId { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeSelMember";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 && AdminId > 0;
        } 
    }
}
