namespace LicenseeAgent.API.LRF.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// InsertInternalAuthorityCriteria class.
    /// </summary>
    public class InsertInternalAuthorityCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int OnUserId { get; set; }

        public string Ip { get; set; }

        public string Browser { get; set; }

        public string Language { get; set; }

        public string Environment { get; set; }

        public string DomainName { get; set; }

        public string Guid { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_InsertInternalAuthority";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0
                && UserId > 0
                && OnUserId > 0
                && !string.IsNullOrEmpty(Environment)
                && !string.IsNullOrEmpty(Ip)
                && !string.IsNullOrEmpty(Browser)
                && !string.IsNullOrEmpty(Language);
        }
    }
}