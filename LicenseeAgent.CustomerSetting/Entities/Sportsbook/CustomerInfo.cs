// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.29">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Entities.Sportsbook
{
    using System.ComponentModel.DataAnnotations;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan;
    using LicenseeAgent.Taiwan.Utility;
    using LicenseeAgent.Web.Validations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerInfo
    {
        public CustomerInfo()
        {
            Status = (byte)AccountStatus.Open;
        }

        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [AppPasswordRule]
        [StringLength(24, MinimumLength = 8, ErrorMessageResourceName = "PasswordRuleInstruction", ErrorMessageResourceType = typeof(Las))]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        [Service(Name = "MobilePhone")]
        public string Mobile { get; set; }

        public string Fax { get; set; }

        [Required]
        public decimal Credit { get; set; }

        public decimal MaxCredit { get; set; }

        public int Status { get; set; }
    }
}