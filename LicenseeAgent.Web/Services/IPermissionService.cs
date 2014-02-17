// --------------------------------------------------------------------------
// <copyright file="IPermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Criterias;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPermissionService
    {
        bool CheckForEditingCustomerSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging);

        bool CheckForEditingAgentInformation(CustomerBelongingCheckingBase belongingCheckingBase);

        bool CheckForEditingMemberInformation(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDirectDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDirectDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDownlineTransaction(TransactionBelongingChecking transactionBelongingChecking);

        bool IsDownlineReferenceNumber(ReferenceNumberBelongingChecking referenceNumberBelongingChecking);

        bool IsValidCurrency(InternalLoginAsUserCriteria internalLoginAsUserCriteria);

        void AgentUserLogout(AgentUserLogoutCriteria criteria);

        void AdminUserLogout(AdminUserLogoutCriteria criteria);
    }
}