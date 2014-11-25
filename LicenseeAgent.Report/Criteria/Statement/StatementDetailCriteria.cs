namespace LicenseeAgent.Report.Criteria.Statement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// StatementDetailCriteria class.
    /// </summary>
    public class StatementDetailCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public bool IsAgent { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_Statement_SelByUsername";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Username);
        }
    }
}