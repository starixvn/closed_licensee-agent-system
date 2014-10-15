namespace LicenseeAgent.ProductSetting.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.ProductSetting.Criteria.Status;

    /// <summary>
    /// ProductService class.
    /// </summary>
    public class StatusService : QueryService, IStatusService
    {
        public void UpdateAgent(UpdateAgentCriteria criteria)
        {
            Execute(criteria);
        }

        public void UpdateMember(UpdateMemberCriteria criteria)
        {
            Execute(criteria);
        }
    }
}