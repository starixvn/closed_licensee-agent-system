namespace LicenseeAgent.Taiwan.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Taiwan.Entities;

    /// <summary>
    /// LogService class.
    /// </summary>
    internal class LogService : QueryService, ILogService
    {
        /// <summary>
        /// Write log when call API.
        /// Wrap exception to avoid causing error on LAS site.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        public void WriteLog(ICriteria criteria)
        {
            Execute(criteria);
        }

        /// <summary>
        /// Get list log information.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        /// <returns>List log information.</returns>
        public IEnumerable<UpdateMemberStatusLogEntity> GetUpdateMemberStatusLogs(ICriteria criteria)
        {
            return GetEntites<UpdateMemberStatusLogEntity>(criteria);
        }
    }
}