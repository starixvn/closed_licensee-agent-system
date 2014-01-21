// -----------------------------------------------------------------------
// <copyright file="BalanceInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BalanceInfoCriteria : CriteriaBase
    {
        public int AgentID { get; set; }

        public DateTime MoveBetDay { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_HomePage_SelBalanceInfo";
        }

        public override bool IsValid()
        {
            return AgentID > 0;
        }
    }
}