namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Report.Entities.AgentWinLoss;

    /// <summary>
    /// IAgentWinLossService class.
    /// </summary>
    public interface IAgentWinLossService
    {
        IList<AgentWinLossEntity> GetData(ICriteria criteria);
    }
}