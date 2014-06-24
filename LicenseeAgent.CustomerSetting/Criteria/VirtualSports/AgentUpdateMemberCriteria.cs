// --------------------------------------------------------------------------
// <copyright file="AgentUpdateMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.VirtualSports
{
    using Fanex.Data.Repository;

    /// <summary>
    /// AgentUpdateMemberCriteria class.
    /// </summary>
    public class AgentUpdateMemberCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int CustId { get; set; }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public string PositionTaking3rd { get; set; }

        public string BetLimit3rd { get; set; }

        public string SettingOutput { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentEditMemberVirtualSports";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}