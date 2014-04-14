// -----------------------------------------------------------------------
// <copyright file="IIpInformationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerList.Criteria;
    using LicenseeAgent.CustomerList.Entities;

    public interface IIpInformationService
    {
        /// <summary>
        /// Authenticates the agent.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns agent information.</returns>
        IEnumerable<IpEntity> GetIpInformation(GetIpInformationCriteria criteria);
    }
}