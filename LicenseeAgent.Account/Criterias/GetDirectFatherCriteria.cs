// ----------------------------------------------------------------------
// <copyright file="GetDirectFatherCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetDirectFatherCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetDirectFather";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
