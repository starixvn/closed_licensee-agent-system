namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// Agent information.
    /// </summary>
    [Serializable]
    public class AgentInfo
    {
        private string _userName;

        public string UserName
        {
            get
            {
                return IsLevel0 ? LicenseeName : _userName;
            }

            set
            {
                _userName = value;
            }
        }

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

        public LegendType LegendType { get; set; }

        public bool IsAllowedMemberComm { get; set; }

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

        public PasswordStatus PasswordStatus
        {
            get
            {
                return PasswordRule.GetPasswordStatus(this, LastLoggedTime);
            }
        }

        public bool IsDeposit
        {
            get
            {
                return LegendType == LegendType.Deposit;
            }
        }

        public bool IsCredit
        {
            get
            {
                return LegendType == LegendType.Credit;
            }
        }

        public bool IsLastAgent(int agentLevel)
        {
            return ModelHelper.IsLastAgent(agentLevel, NumberOfLevel);
        }
    }
}