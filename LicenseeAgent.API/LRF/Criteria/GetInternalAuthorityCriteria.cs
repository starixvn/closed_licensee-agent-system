namespace LicenseeAgent.API.LRF.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetInternalAuthorityCriteria class.
    /// </summary>
    public class GetInternalAuthorityCriteria : CriteriaBase
    {
        public string Guid { get; set; }

        public string Browser { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_GetInternalAuthority";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Guid)
                && !string.IsNullOrEmpty(Browser)
                && !string.IsNullOrEmpty(Ip);
        }
    }
}