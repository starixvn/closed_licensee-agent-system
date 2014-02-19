// --------------------------------------------------------------------------
// <copyright file="IDownlLinePermissionService.cs" company="Nexcel Solutions Vietnam">
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
    public interface IDownlLinePermissionService
    {
        bool CheckForEditingCustomerSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging);

        bool IsDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDownlineMember(int memberId);

        bool IsDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDownlineAgent(int agentId);

        bool IsDirectDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDirectDownlineAgent(int agentId);

        bool IsDirectDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase);

        bool IsDirectDownlineMember(int memberId);

        bool IsDownlineTransaction(TransactionBelongingChecking transactionBelongingChecking);

        bool IsDownlineTransaction(long transactionId);

        bool IsDownlineReferenceNumber(ReferenceNumberBelongingChecking referenceNumberBelongingChecking);

        bool IsDownlineReferenceNumber(long referenceNumber);
    }
}