// ----------------------------------------------------------------------
// <copyright file="LicenseeGetSettingForInsertCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria.NumberGame.Licensee
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// LicenseeGetNumberGameSettingForInsertCriteria class.
    /// </summary>
    public class LicenseeGetSettingForInsertCriteria : CriteriaBase
    {
        public int AgentL0Id { get; set; }

        public int MsgId { get; set; }

        public short MaxPTOfLicensee { get; set; }

        public override string GetSettingKey()
        {
            return "Las_LicenseeNumberGameSettingSel";
        }

        public override bool IsValid()
        {
            return AgentL0Id > 0;
        }
    }
}
