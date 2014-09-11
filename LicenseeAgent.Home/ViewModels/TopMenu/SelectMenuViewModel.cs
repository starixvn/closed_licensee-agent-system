namespace LicenseeAgent.Home.ViewModels.TopMenu
{
    using System.Collections.Generic;
    using LicenseeAgent.Home.Entities.TopMenu;

    /// <summary>
    /// SelectMenuViewModel class.
    /// </summary>
    public class SelectMenuViewModel
    {
        public IList<LicenseeEntity> Licensees { get; set; }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public IList<CurrencyEntity> Currencies { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public int AgentLevel { get; set; }
    }
}