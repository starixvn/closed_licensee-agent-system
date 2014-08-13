namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetLicenseeGroupCommissionCriteria class.
    /// </summary>
    public class GroupCommissionSelCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int? GroupCommId { get; set; }

        public string GroupName { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGroupCommSel";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0;
        } 
    }
}
