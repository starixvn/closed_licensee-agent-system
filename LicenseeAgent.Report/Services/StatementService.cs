namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Entities.Statement;

    /// <summary>
    /// StatementService class.
    /// </summary>
    public class StatementService : QueryService, IStatementService
    {
        public IList<StatementEntity> GetStatementList(ICriteria criteria)
        {
            return GetEntites<StatementEntity>(criteria);
        }

        public IList<TransferDetail> GetTransferDetailList(ICriteria criteria)
        {
            return GetEntites<TransferDetail>(criteria);
        }
    }
}