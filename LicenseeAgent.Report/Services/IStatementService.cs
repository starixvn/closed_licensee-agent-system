namespace LicenseeAgent.Report.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using LicenseeAgent.Report.Entities.Statement;

    /// <summary>
    /// IStatementService class.
    /// </summary>
    public interface IStatementService
    {
        IList<StatementEntity> GetStatementList(ICriteria criteria);

        IList<TransferDetail> GetTransferDetailList(ICriteria criteria);
    }
}