// -----------------------------------------------------------------------
// <copyright file="StatisticEntity.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StatisticEntity
    {
        public decimal OutStanding { get; set; }

        [SpParam(SqlParamName = "total_outstanding")]
        public decimal TotalOutStading { get; set; }

        [SpParam(SqlParamName = "number_newMembers")]
        public int NumberOfNewMember { get; set; }
    }
}
