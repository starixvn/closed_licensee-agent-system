namespace LicenseeAgent.Core.Entities
{
    using System;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// InternalLoginInfo class.
    /// </summary>
    [Serializable]
    public class InternalLoginInfo
    {
        public InternalLoginInfo()
        {
            Timeout = DateTime.Now.AddMinutes(3);
        }

        public DateTime Timeout { get; private set; }

        public AccountType LoginAccountType { get; set; }

        public int LoginUserId { get; set; }

        public string LoginUserName { get; set; }

        public LoginType LoginType { get; set; }

        public int OnLoginUserId { get; set; }

        public string Language { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public string AgentName { get; set; }

        public bool IsExpired
        {
            get
            {
                return DateTime.Now > Timeout;
            }
        }
    }
}