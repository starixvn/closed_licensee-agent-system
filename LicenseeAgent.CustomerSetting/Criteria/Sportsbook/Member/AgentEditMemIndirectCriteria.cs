// ----------------------------------------------------------------------
// <copyright file="AgentEditMemIndirectCriteria.cs" company="Nexcel Solutions Vietnam">
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
    public class AgentEditMemIndirectCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int Updater { get; set; }

        public int MsgId { get; set; }

        public string PositionTaking { get; set; }

        public short MinPTOutput { get; set; }

        public short MaxPTOutput { get; set; }

        public int PTSportType { get; set; }

        public int PTBetType { get; set; }

        public bool IsLiveOutput { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey()
        {
            return "Las_AgentEditMemIndirect";
        }

        public override bool IsValid()
        {
            return CustId > 0 && AgentId > 0 && Updater > 0 && !string.IsNullOrEmpty(PositionTaking);
        }
    }
}
