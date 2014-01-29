// -----------------------------------------------------------------------
// <copyright file="AgentAutoCompleteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentAutoCompleteCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Text { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentGetAutoCompleteUserName";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && !string.IsNullOrEmpty(Text);
        }
    }
}