namespace LicenseeAgent.Account.Criteria.Internal
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetCurrencyOfLicenseeCriteria class.
    /// </summary>
    public class GetCurrencyOfLicenseeCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetCurrencyOfLicensee";
        }

        public override bool IsValid()
        {
            return UserId > 0
                && LicenseeId > 0;
        }
    }
}