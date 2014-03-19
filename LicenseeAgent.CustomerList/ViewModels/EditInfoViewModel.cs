// -----------------------------------------------------------------------
// <copyright file="EditInfoViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditInfoViewModel
    {
        public int CustId { get; set; }

        [Display(Name = "Username", ResourceType = typeof(Las))]
        public string UserName { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Las))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Las))]
        public string LastName { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(Las))]
        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessageResourceName = "InvalidPhone", ErrorMessageResourceType = typeof(Las))]
        public string Phone { get; set; }

        [Display(Name = "MobilePhone", ResourceType = typeof(Las))]
        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessageResourceName = "InvalidMobilePhone", ErrorMessageResourceType = typeof(Las))]
        public string Mobile { get; set; }

        [Display(Name = "Fax", ResourceType = typeof(Las))]
        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessageResourceName = "InvalidFax", ErrorMessageResourceType = typeof(Las))]
        public string Fax { get; set; }
    }
}