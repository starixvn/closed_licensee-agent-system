namespace LicenseeAgent.Home.Criteria.TopMenu
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetLicenseeListCriteria class.
    /// </summary>
    public class GetLicenseeListCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetLicenseeList";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}