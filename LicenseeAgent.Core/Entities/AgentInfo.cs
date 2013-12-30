// --------------------------------------------------------------------------
// <copyright file="AgentInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// Agent information.
    /// </summary>
    [Serializable]
    public class AgentInfo
    {
        public string UserName { get; set; }

        public string UserSalt { get; set; }

        public short AgentLevel { get; set; }

        public short NumberOfLevel { get; set; }

        public int AgentId { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public int SubAccId { get; set; }

        public bool IsFirstLogin { get; set; }

        public DateTime PwdExpiry { get; set; }

        public DateTime LastChangedPwdDate { get; set; }

        public bool IsNickName { get; set; }

        public DateTime LastLoggedTime { get; set; }

        public bool IsLevel0
        {
            get
            {
                return AgentLevel == 1;
            }
        }

        public bool IsLevel1
        {
            get
            {
                return AgentLevel == 2;
            }
        }

        public bool IsLastLevel
        {
            get
            {
                return ModelHelper.IsLastAgent(AgentLevel, NumberOfLevel);
            }
        }

        public string AgentName
        {
            get
            {
                return IsLevel0 ? LicenseeName : UserName;
            }
        }

        public bool IsLastAgent(int agentLevel)
        {
            return ModelHelper.IsLastAgent(agentLevel, NumberOfLevel);
        }
    }
}