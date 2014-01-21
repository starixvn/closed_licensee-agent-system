// -----------------------------------------------------------------------
// <copyright file="EditInfoViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Validations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditInfoViewModel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }
    }
}
