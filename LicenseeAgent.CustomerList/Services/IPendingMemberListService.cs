namespace LicenseeAgent.CustomerList.Services
{
    using LicenseeAgent.CustomerList.Criteria.PendingMember;

    /// <summary>
    /// IPendingMemberService class.
    /// </summary>
    public interface IPendingMemberListService
    {
        int GetAgentIdByUserName(GetAgentIdByUsernameCriteria criteria);
    }
}