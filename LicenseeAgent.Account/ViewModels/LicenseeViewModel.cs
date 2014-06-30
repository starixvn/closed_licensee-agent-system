namespace LicenseeAgent.Account.ViewModels
{
    using System.Collections.Generic;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Enumerations;

    /// <summary>
    /// LicenseeViewModel class.
    /// </summary>
    public class LicenseeViewModel
    {
        public IEnumerable<LicenseeEntity> Licensees { get; set; }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public IEnumerable<CurrencyEntity> Currencies { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public int AgentLevel { get; set; }

        public AccountType AccountType { get; set; }
    }
}