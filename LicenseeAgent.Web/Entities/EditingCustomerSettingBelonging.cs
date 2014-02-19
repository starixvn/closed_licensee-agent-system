// --------------------------------------------------------------------------
// <copyright file="EditingCustomerSettingBelonging.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Entities
{
    using LicenseeAgent.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditingCustomerSettingBelonging
    {
        public FormType FormId { get; set; }

        public int CustId { get; set; }

        public int RoleId { get; set; }

        public int TargetCustId { get; set; }

        public int TargetRoleId { get; set; }
    }
}