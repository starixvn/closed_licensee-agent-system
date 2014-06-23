// ----------------------------------------------------------------------
// <copyright file="GetDirectDownLineCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetDirectDownLineCriteria class.
    /// </summary>
    public class GetDirectDownLineCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetDirectDownline";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}