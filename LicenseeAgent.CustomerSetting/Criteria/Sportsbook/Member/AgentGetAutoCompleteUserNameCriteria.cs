// ----------------------------------------------------------------------
// <copyright file="AgentGetAutoCompleteUserNameCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Member
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentGetAutoCompleteUserNameCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Text { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetAutoCompleteUserName";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
