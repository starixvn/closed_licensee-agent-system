namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Report.Criteria.AgentWinLoss;
    using LicenseeAgent.Report.Entities.AgentWinLoss;

    /// <summary>
    /// IAgentWinLossService class.
    /// </summary>
    public interface IAgentWinLossService
    {
        IList<AgentWinLossEntity> GetAgentList(ICriteria criteria);

        IList<AgentWinLossEntity> GetMemberList(ICriteria criteria);

        AgentWinLossEntity GetMemberSum(WinLossMemberViewCriteria criteria);
    }
}