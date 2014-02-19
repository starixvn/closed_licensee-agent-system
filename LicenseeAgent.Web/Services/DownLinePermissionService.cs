// --------------------------------------------------------------------------
// <copyright file="DownLinePermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DownLinePermissionService : IDownlLinePermissionService
    {
        private IDownLineService _downLineService;

        public DownLinePermissionService(IDownLineService downLineService)
        {
            _downLineService = downLineService;
        }

        public bool CheckForEditingCustomerSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging)
        {
            return editingCustomerSettingBelonging.TargetRoleId == (int)Role.Member
                ? CheckForEditingMemberSetting(editingCustomerSettingBelonging)
                : CheckForEditingAgentSetting(editingCustomerSettingBelonging);
        }

        /// <summary>
        /// Determines whether [is down-line member] [the specified belonging checking base].
        /// </summary>
        /// <param name="belongingCheckingBase">The belonging checking base.</param>
        /// <returns>Returns true if down-line.</returns>
        public bool IsDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = (int)belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Member
            };

            return _downLineService.IsDownline(criteria);
        }

        public bool IsDownlineMember(int memberId)
        {
            var belongingCheckingBase = new CustomerBelongingCheckingBase
            {
                TargetCustId = memberId
            };

            return IsDownlineMember(belongingCheckingBase);
        }

        /// <summary>
        /// Determines whether [is down-line agent] [the specified belonging checking base].
        /// </summary>
        /// <param name="belongingCheckingBase">The belonging checking base.</param>
        /// <returns>Returns true if down-line.</returns>
        public bool IsDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = (int)belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Agent
            };

            return _downLineService.IsDownline(criteria);
        }

        public bool IsDownlineAgent(int agentId)
        {
            var belongingCheckingBase = new CustomerBelongingCheckingBase
            {
                TargetCustId = agentId
            };

            return IsDownlineAgent(belongingCheckingBase);
        }

        /// <summary>
        /// Determines whether [is direct down-line agent] [the specified belonging checking base].
        /// </summary>
        /// <param name="belongingCheckingBase">The belonging checking base.</param>
        /// <returns>Return true if direct down-line.</returns>
        public bool IsDirectDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Agent
            };

            return SessionManager.CurrentSession.AgentInfo.IsLevel0
            ? _downLineService.IsDownline(criteria)
            : _downLineService.IsDirectDownline(criteria);
        }

        public bool IsDirectDownlineAgent(int agentId)
        {
            var belongingCheckingBase = new CustomerBelongingCheckingBase
            {
                TargetCustId = agentId
            };

            return IsDirectDownlineAgent(belongingCheckingBase);
        }

        /// <summary>
        /// Determines whether [is direct down-line member] [the specified belonging checking base].
        /// </summary>
        /// <param name="belongingCheckingBase">The belonging checking base.</param>
        /// <returns>Returns true if direct down-line.</returns>
        public bool IsDirectDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Member
            };

            return SessionManager.CurrentSession.AgentInfo.IsLevel0
            ? _downLineService.IsDownline(criteria)
            : _downLineService.IsDirectDownline(criteria);
        }

        public bool IsDirectDownlineMember(int memberId)
        {
            var belongingCheckingBase = new CustomerBelongingCheckingBase
            {
                TargetCustId = memberId
            };

            return IsDirectDownlineMember(belongingCheckingBase);
        }

        public bool IsDownlineTransaction(TransactionBelongingChecking transactionBelongingChecking)
        {
            var criteria = new TicketDownLineCriteria
            {
                AgentId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TransId = transactionBelongingChecking.TransactionId
            };

            return _downLineService.IsDownlineTicket(criteria);
        }

        public bool IsDownlineTransaction(long transactionId)
        {
            var transactionBelongingChecking = new TransactionBelongingChecking
            {
                TransactionId = transactionId
            };

            return IsDownlineTransaction(transactionBelongingChecking);
        }

        public bool IsDownlineReferenceNumber(ReferenceNumberBelongingChecking referenceNumberBelongingChecking)
        {
            var criteria = new ReferenceNumberCriteria
            {
                AgentId = SessionManager.CurrentSession.AgentInfo.AgentId,
                RefNo = referenceNumberBelongingChecking.ReferenceNumber
            };

            return _downLineService.IsDownlineReferenceNumber(criteria);
        }

        public bool IsDownlineReferenceNumber(long referenceNumber)
        {
            var referenceNumberBelongingChecking = new ReferenceNumberBelongingChecking
            {
                ReferenceNumber = referenceNumber
            };

            return IsDownlineReferenceNumber(referenceNumberBelongingChecking);
        }

        /// <summary>
        /// Checks for editing agent.
        /// </summary>
        /// <param name="editingCustomerSettingBelonging">The editing customer setting belonging.</param>
        /// <returns>
        /// Is valid or not.
        /// </returns>
        private bool CheckForEditingAgentSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging)
        {
            var agentInfo = SessionManager.CurrentSession.AgentInfo;
            switch (editingCustomerSettingBelonging.FormId)
            {
                case FormType.Add:
                    if (editingCustomerSettingBelonging.CustId == agentInfo.AgentId)
                    {
                        return true;
                    }

                    break;

                case FormType.Edit:
                    if (IsDirectDownlineAgent(editingCustomerSettingBelonging.TargetCustId))
                    {
                        return true;
                    }

                    break;

                case FormType.ViewIndirectAgent:
                    if (IsDownlineAgent(editingCustomerSettingBelonging.TargetCustId))
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        /// <summary>
        /// Checks for editing member setting.
        /// </summary>
        /// <param name="editingCustomerSettingBelonging">The editing customer setting belonging.</param>
        /// <returns>Is valid or not.</returns>
        private bool CheckForEditingMemberSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging)
        {
            var agentInfo = SessionManager.CurrentSession.AgentInfo;
            switch (editingCustomerSettingBelonging.FormId)
            {
                case FormType.Add:
                    if (editingCustomerSettingBelonging.CustId == agentInfo.AgentId)
                    {
                        return true;
                    }

                    break;

                case FormType.Edit:
                    if (IsDirectDownlineMember(editingCustomerSettingBelonging.TargetCustId))
                    {
                        return true;
                    }

                    break;

                case FormType.EditIndirectMember:
                    if (IsDownlineMember(editingCustomerSettingBelonging.TargetCustId))
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }
    }
}