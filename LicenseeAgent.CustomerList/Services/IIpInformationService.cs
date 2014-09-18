namespace LicenseeAgent.Customer.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Customer.Criteria;
    using LicenseeAgent.Customer.Entities;

    public interface IIpInformationService
    {
        /// <summary>
        /// Authenticates the agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns agent information.</returns>
        IList<IpEntity> GetIpInformation(GetIpInformationCriteria criteria);
    }
}