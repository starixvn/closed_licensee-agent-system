// ----------------------------------------------------------------------
// <copyright file="AgentGetSettingForInsertCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.NumberGame.Agent
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentGetNumberGameSettingForInsertCriteria class.
    /// </summary>
    public class AgentGetSettingForInsertCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MsgId { get; set; }

        public decimal MaxCredit { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentNumberGameSettingSel";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
