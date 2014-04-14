// --------------------------------------------------------------------------
// <copyright file="DomainService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.API.LRF.Services
{
    using System;
    using System.Configuration;
    using System.Data;
    using Fanex.Data;
    using LicenseeAgent.API.LRF.Entities;

    public class DomainService : IDomainService
    {
        /// <summary>
        /// Gets the licensee domain.
        /// </summary>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <returns>
        /// Returns domain name by licensee ID.
        /// </returns>
        public LicenseeDomain GetLicenseeDomain(int licenseeId)
        {
            var environment = ConfigurationManager.AppSettings["Las_Environment"];
            using (var objectDb = new ObjectDb("Las_GetLicenseeDomain"))
            {
                objectDb.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                objectDb.AddInParameter("environment", DbType.String, environment);
                var reader = objectDb.ExecuteReader();
                while (reader.Read())
                {
                    var currentLicenseeId = Convert.ToInt32(reader["licenseeid"]);
                    var currentDomainName = Convert.ToString(reader["domainname"]);
                    if (currentLicenseeId == licenseeId)
                    {
                        return new LicenseeDomain
                        {
                            LicenseeId = currentLicenseeId,
                            DomainName = currentDomainName
                        };
                    }
                }
            }

            return null;
        }
    }
}