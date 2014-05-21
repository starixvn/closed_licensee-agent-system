// ----------------------------------------------------------------------
// <copyright file="ICustomerSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICustomerSettingService
    {
        void CheckExistUserName(CheckExistUserNameCriteria criteria);

        CustomerInfo GetGeneralInfoAgent(EditProductAgentViewModel model);

        CustomerInfo GetGeneralInfoMember(EditProductMemberViewModel model);

        InsertResult InsertCustomer(InsertCustomerViewModel model);

        IList<UpdateResult> UpdateCustomer(UpdateCustomerViewModel model);

        IList<UpdateResult> UpdateIndirectCustomer(UpdateCustomerViewModel model);
    }
}