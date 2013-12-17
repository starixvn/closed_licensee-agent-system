// -----------------------------------------------------------------------
// <copyright file="LicenseeViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using LicenseeAgent.Account.Entities;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
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
