// --------------------------------------------------------------------------
// <copyright file="UpdateAgentCreditCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CreditBalance.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateAgentCreditCriteria class.
    /// </summary>
    public class UpdateAgentCreditCriteria : CriteriaBase
    {
        private readonly string _settingKey = string.Empty;

        public UpdateAgentCreditCriteria(string settingKey)
        {
            _settingKey = settingKey;
        }

        public int AgentId { get; set; }

        public decimal Credit { get; set; }

        public int MsgId { get; set; }

        public int Updater { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return _settingKey;
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}