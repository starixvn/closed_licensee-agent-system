namespace LicenseeAgent.Taiwan.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Taiwan.Entities;

    internal interface ILogService
    {
        /// <summary>
        /// Write log when call API.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        void WriteLog(ICriteria criteria);

        /// <summary>
        /// Get list log information.
        /// </summary>
        /// <param name="criteria">Criteria store procedure.</param>
        /// <returns>List log information.</returns>
        IList<UpdateMemberStatusLogEntity> GetUpdateMemberStatusLogs(ICriteria criteria);
    }
}