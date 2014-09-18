namespace LicenseeAgent.Customer.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Customer.Criteria;
    using LicenseeAgent.Customer.Entities;

    /// <summary>
    /// IP InformationService class.
    /// </summary>
    public class IpInformationService : QueryService, IIpInformationService
    {
        /// <summary>
        /// Authenticates the agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns agent information.</returns>
        public IList<IpEntity> GetIpInformation(GetIpInformationCriteria criteria)
        {
            return GetEntites<IpEntity>(criteria);
        }
    }
}