namespace LicenseeAgent.Home.Criteria.AccountInformation
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMemberProductSettingCriteria class.
    /// </summary>
    public class GetMemberProductSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetProductSettingByCustId";
        }

        public override bool IsValid()
        {
            return CustId > 0 && LicenseeId > 0 && CurrencyId > 0;
        }
    }
}