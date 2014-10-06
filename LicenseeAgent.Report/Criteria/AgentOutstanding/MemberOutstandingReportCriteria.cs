namespace LicenseeAgent.Report.Criteria.AgentOutstanding
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MemberOutstandingReportCriteria class.
    /// </summary>
    public class MemberOutstandingReportCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_Rpt_Outstanding_DirectMem";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}