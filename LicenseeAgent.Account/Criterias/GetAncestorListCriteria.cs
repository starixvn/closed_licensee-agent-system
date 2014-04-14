// ----------------------------------------------------------------------
// <copyright file="GetAncestorListCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetAncestorListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }
        
        public override string GetSettingKey()
        {
            return "Las_GetAncestorList";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
