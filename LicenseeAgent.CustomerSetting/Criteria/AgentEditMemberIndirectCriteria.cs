// ----------------------------------------------------------------------
// <copyright file="AgentEditMemberIndirectCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentEditMemberIndirectCriteria class.
    /// </summary>
    public class AgentEditMemberIndirectCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int Updater { get; set; }

        public int MsgId { get; set; }

        public string PositionTaking { get; set; }

        public string PositionTaking3rd { get; set; }

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
            return CustId > 0 && AgentId > 0 && Updater > 0;
        }
    }
}