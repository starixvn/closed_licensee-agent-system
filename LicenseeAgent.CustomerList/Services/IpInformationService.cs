// -----------------------------------------------------------------------
// <copyright file="IpInformationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerList.Criteria;
    using LicenseeAgent.CustomerList.Entities;

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