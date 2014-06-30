namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get IP InformationCriteria class.
    /// </summary>
    public class GetIpInformationCriteria : CriteriaBase
    {
        public string Ip { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_GetIPInfo_Full";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Ip);
        }
    }
}