namespace LicenseeAgent.Report.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ReportCriteriaBase class.
    /// </summary>
    public abstract class ReportCriteriaBase : CriteriaBase
    {
        public bool IsShowAll { get; set; }
    }
}