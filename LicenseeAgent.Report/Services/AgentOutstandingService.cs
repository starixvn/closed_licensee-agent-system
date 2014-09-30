namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Criteria.AgentOutstanding;
    using LicenseeAgent.Report.Entities.AgentOutstanding;

    /// <summary>
    /// AgentOutstandingService class.
    /// </summary>
    public class AgentOutstandingService : QueryService, IAgentOutstandingService
    {
        /// <summary>
        /// Get outstanding results.
        /// </summary>
        /// <param name="criteria">Outstanding Criteria.</param>
        /// <returns>List of agent outstanding results.</returns>
        public IList<AgentOutstandingEntity> GetData(OutstandingReportCriteria criteria)
        {
            return GetEntites<AgentOutstandingEntity>(criteria);
        }
    }
}