namespace LicenseeAgent.ProductSetting.Services
{
    using LicenseeAgent.ProductSetting.Criteria.Status;

    /// <summary>
    /// IProductService class.
    /// </summary>
    public interface IStatusService
    {
        void UpdateAgent(UpdateAgentCriteria criteria);

        void UpdateMember(UpdateMemberCriteria criteria);
    }
}