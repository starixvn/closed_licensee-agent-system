// -----------------------------------------------------------------------
// <copyright file="MemberAutoCompleteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// MemberAutoCompleteCriteria class.
    /// </summary>
    public class MemberAutoCompleteCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string Text { get; set; }

        public override string GetSettingKey()
        {
            return "Las_MemberGetAutoCompleteUserName";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && !string.IsNullOrEmpty(Text);
        }
    }
}