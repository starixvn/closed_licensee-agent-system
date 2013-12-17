// --------------------------------------------------------------------------
// <copyright file="LicenseeGetAgentSettingForInsertCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.29">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook.Licensee
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeGetAgentSettingForInsertCriteria : CriteriaBase
    {
        public int AgentL0Id { get; set; }

        public decimal MaxCredit { get; set; }

        public int MsgId { get; set; }

        public short MaxPTOfLicensee { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeSportsbookSettingSel";
        }

        public override bool IsValid()
        {
            return AgentL0Id > 0;
        }
    }
}