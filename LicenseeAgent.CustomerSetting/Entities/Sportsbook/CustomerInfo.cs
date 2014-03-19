// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan;
    using LicenseeAgent.Web.Validations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerInfo
    {
        public CustomerInfo()
        {
            Status = (int)AccountStatus.Open;
        }

        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        [RegularExpression("^[a-zA-Z0-9]{6,30}$", ErrorMessageResourceName = "UsernameMinimumLength", ErrorMessageResourceType = typeof(Las))]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [AppPasswordRule]
        [StringLength(24, MinimumLength = 8, ErrorMessageResourceName = "PasswordSecurity", ErrorMessageResourceType = typeof(Las))]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessageResourceName = "InvalidPhone", ErrorMessageResourceType = typeof(Las))]
        public string Phone { get; set; }

        [Service(Name = "MobilePhone")]
        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessageResourceName = "InvalidMobilePhone", ErrorMessageResourceType = typeof(Las))]
        public string Mobile { get; set; }

        [RegularExpression(@"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$", ErrorMessageResourceName = "InvalidFax", ErrorMessageResourceType = typeof(Las))]
        public string Fax { get; set; }

        [Required]
        public decimal Credit { get; set; }

        public decimal? MaxCredit { get; set; }

        public int Status { get; set; }
    }
}