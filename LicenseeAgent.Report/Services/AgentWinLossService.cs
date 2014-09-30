namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Entities.AgentWinLoss;

    /// <summary>
    /// AgentWinLossService class.
    /// </summary>
    public class AgentWinLossService : QueryService, IAgentWinLossService
    {
        public IList<AgentWinLossEntity> GetData(ICriteria criteria)
        {
            return GetEntites<AgentWinLossEntity>(criteria);
        }
    }
}