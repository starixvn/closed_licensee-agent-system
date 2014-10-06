namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
        public IList<AgentOutstandingEntity> GetAgentList(AgentOutstandingReportCriteria criteria)
        {
            return GetEntites<AgentOutstandingEntity>(criteria);
        }

        public IList<AgentOutstandingEntity> GetMemberList(MemberOutstandingReportCriteria criteria)
        {
            return GetEntites<AgentOutstandingEntity>(criteria);
        }

        public AgentOutstandingEntity GetMemberSum(MemberOutstandingReportCriteria criteria)
        {
            var list = GetEntites<AgentOutstandingEntity>(criteria);
            return list.Count == 0 ? null : new AgentOutstandingEntity
            {
                Id = 0,
                Username = "Direct Member",
                Outstanding = list.Sum(x => x.Outstanding),
                Positiontaking = list.Sum(x => x.Positiontaking)
            };
        }
    }
}