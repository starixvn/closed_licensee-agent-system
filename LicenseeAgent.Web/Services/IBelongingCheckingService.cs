// --------------------------------------------------------------------------
// <copyright file="IBelongingCheckingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBelongingCheckingService
    {
        bool CheckForEditingCustomerSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging);

        bool CheckForEditingAgentInformation(CustomerBelongingCheckingBase belongingCheckingBase);

        bool CheckForEditingMemberInformation(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDownLineMember(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDownLineTransaction(TransactionBelongingChecking transactionBelongingChecking);
    }
}