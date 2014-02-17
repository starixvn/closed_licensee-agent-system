// ----------------------------------------------------------------------
// <copyright file="TicketDetailsDownLineCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/17">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Web.Criterias
{

    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketDetailsDownLineCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_RefNo_CheckBelonging";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && RefNo > 0;
        }
    }
}
