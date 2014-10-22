namespace LicenseeAgent.Core.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetLicenseeDomainCriteria class.
    /// </summary>
    public class GetLicenseeDomainCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public string Environment { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetLicenseeDomain";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}