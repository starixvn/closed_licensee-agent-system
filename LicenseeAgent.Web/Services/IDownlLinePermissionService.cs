// --------------------------------------------------------------------------
// <copyright file="IDownlLinePermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDownlLinePermissionService
    {
        bool CheckForEditingCustomerSetting(CustomerSettingBelonging customerSettingBelonging);

        bool CheckForInsertingCustomerSetting(CustomerSettingBelonging customerSettingBelonging);

        bool IsDownLineMember(int memberId);

        bool IsDownLineAgent(int agentId);

        bool IsDirectDownLineAgent(int agentId);

        bool IsDirectDownLineMember(int memberId);

        bool IsIndirectDownLineAgent(int agentId);

        bool IsIndirectDownLineMember(int memberId);

        bool IsDownLineTransaction(long transactionId);

        bool IsDownLineReferenceNumber(long referenceNumber);
    }
}