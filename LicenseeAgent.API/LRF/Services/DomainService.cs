namespace LicenseeAgent.API.LRF.Services
{
    using System;
    using System.Data;
    using Fanex.Data;
    using LicenseeAgent.API.LRF.Entities;

    /// <summary>
    /// DomainService class.
    /// </summary>
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
            var environment = Configuration.LegendEnvironment;
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