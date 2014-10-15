namespace LicenseeAgent.Web.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Web.Criteria.Common;
    using LicenseeAgent.Web.Entities.Common;

    public interface ICommonService
    {
        /// <summary>
        /// Authenticates the agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns agent information.</returns>
        IpEntity GetIpInformation(GetIpInformationCriteria criteria);
    }
}