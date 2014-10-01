namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criteria.Common;
    using LicenseeAgent.Web.Entities.Common;

    /// <summary>
    /// IP InformationService class.
    /// </summary>
    public class CommonService : QueryService, ICommonService
    {
        /// <summary>
        /// Authenticates the agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns agent information.</returns>
        public IpEntity GetIpInformation(GetIpInformationCriteria criteria)
        {
            return GetEntity<IpEntity>(criteria);
        }
    }
}