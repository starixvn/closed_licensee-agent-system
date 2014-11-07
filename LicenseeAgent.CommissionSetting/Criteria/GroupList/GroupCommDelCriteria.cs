namespace LicenseeAgent.CommissionSetting.Criteria.GroupList
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Commission Delete Criteria.
    /// </summary>
    public class GroupCommDelCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int Currency { get; set; }

        public int GroupCommId { get; set; }

        public int AdminId { get; set; }

        public string IP { get; set; }

        public bool IsRemoveMemberComm { get; set; }

        public bool IsResetDefault { get; set; }

        public int ErrCode { get; set; }

        public string StrCustID { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeGroupCommDel";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0
                && Currency > 0
                && GroupCommId > 0
                && AdminId > 0
                && !string.IsNullOrEmpty(IP);
        }
    }
}