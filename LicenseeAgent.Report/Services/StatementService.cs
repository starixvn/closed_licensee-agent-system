namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Report.Entities;

    /// <summary>
    /// StatementService class.
    /// </summary>
    public class StatementService : QueryService, IStatementService
    {
        public IList<Statement> GetStatementList(ICriteria criteria)
        {
            return GetEntites<Statement>(criteria);
        }

        public IList<TransferDetail> GetTransferDetailList(ICriteria criteria)
        {
            return GetEntites<TransferDetail>(criteria);
        }
    }
}