namespace LicenseeAgent.CustomerList.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria.PendingMember;

    /// <summary>
    /// PendingMemberService class.
    /// </summary>
    public class PendingMemberListService : QueryService, IPendingMemberListService
    {
        public int GetAgentIdByUserName(GetAgentIdByUsernameCriteria criteria)
        {
            Execute(criteria);
            return criteria.AgentId;
        }
    }
}