namespace LicenseeAgent.CommissionSetting.Criteria.MemberList
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MemberRemoveGroupCommissionCriteria class.
    /// </summary>
    public class MemberRemoveGroupCommissionCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        [SpParam(SqlParamName = "strCustIDOriginal")]
        public string OrginalCustomer { get; set; }

        public int AdminId { get; set; }

        public int Currency { get; set; }

        public bool IsRemoveMemberComm { get; set; }

        public string Ip { get; set; }

        [SpParam(SqlParamName = "strCustID")]
        public string EffectedCustomers { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Las_MemberRemoveGroupComm";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0
                && Currency > 0
                && AdminId > 0
                && !string.IsNullOrEmpty(OrginalCustomer)
                && !string.IsNullOrEmpty(Ip);
        }
    }
}