// -----------------------------------------------------------------------
// <copyright file="WinlossBase.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Report.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Win loss Report Base.
    /// </summary>
    public class WinlossBase : CriteriaBase
    {
        public int AgentId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public short DrawType { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
