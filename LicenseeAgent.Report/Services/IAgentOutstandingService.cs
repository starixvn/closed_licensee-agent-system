namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Report.Criteria.AgentOutstanding;
    using LicenseeAgent.Report.Entities.AgentOutstanding;

    /// <summary>
    /// IAgentOutstandingService class.
    /// </summary>
    public interface IAgentOutstandingService
    {
        /// <summary>
        /// Get outstanding results.
        /// </summary>
        /// <param name="criteria">Outstanding Criteria.</param>
        /// <returns>List of agent outstanding results.</returns>
        IList<AgentOutstandingEntity> GetData(OutstandingReportCriteria criteria);
    }
}