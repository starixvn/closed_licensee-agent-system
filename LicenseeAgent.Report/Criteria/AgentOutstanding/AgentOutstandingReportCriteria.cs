namespace LicenseeAgent.Report.Criteria.AgentOutstanding
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Outstanding Report Criteria.
    /// </summary>
    public class AgentOutstandingReportCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Rpt_Outstanding";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}