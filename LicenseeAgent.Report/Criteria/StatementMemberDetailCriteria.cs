namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Statement Member Detail Criteria.
    /// </summary>
    public class StatementMemberDetailCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string CustUsername { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_SelMemberStatement";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CustUsername);
        }
    }
}