// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Services
{
    using Fanex.Data;
    using LicenseeAgent.Core.Criterias;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Web.Criterias;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionService : AuthorizeService, IPermissionService
    {
        public bool CheckForEditingCustomerSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging)
        {
            if (editingCustomerSettingBelonging.TargetRoleId == (int)Role.Member)
            {
                return CheckForEditingMemberSetting(editingCustomerSettingBelonging);
            }
            else
            {
                return CheckForEditingAgent(editingCustomerSettingBelonging);
            }
        }

        public bool CheckForEditingAgentInformation(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            return this.CheckForEditingAgent(belongingCheckingBase);
        }

        public bool CheckForEditingMemberInformation(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Member
            };

            return SessionManager.CurrentSession.AgentInfo.IsLevel0 ?
                this.IsDownline(criteria) : this.IsDirectDownline(criteria);
        }

        public bool IsDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Member
            };

            return this.IsDownline(criteria);
        }

        public bool IsDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Agent
            };

            return this.IsDownline(criteria);
        }

        public bool IsDirectDownlineAgent(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Agent
            };

            return this.IsDirectDownline(criteria);
        }

        public bool IsDirectDownlineMember(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Member
            };

            return this.IsDirectDownline(criteria);
        }

        public bool IsDownlineTransaction(TransactionBelongingChecking transactionBelongingChecking)
        {
            var criteria = new TicketDownLineCriteria { TransId = transactionBelongingChecking.TransactionId };

            return this.IsDownlineTicket(criteria);
        }

        public bool IsDownlineTransaction(long ticketDetailsRefNo)
        {
            var criteria = new TicketDetailsDownLineCriteria { RefNo = ticketDetailsRefNo };

            return this.IsDownlineTicketDetails(criteria);
        }

        public bool IsDownlineReferenceNumber(ReferenceNumberBelongingChecking referenceNumberBelongingChecking)
        {
            var criteria = new ReferenceNumberCriteria { RefNo = referenceNumberBelongingChecking.ReferenceNumber };

            return this.IsDownlineReferenceNumber(criteria);
        }

        /// <summary>
        /// Checks for editing agent.
        /// </summary>
        /// <param name="belongingCheckingBase">The belonging checking base.</param>
        /// <returns>
        /// Is valid or not.
        /// </returns>
        private bool CheckForEditingAgent(CustomerBelongingCheckingBase belongingCheckingBase)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                TargetCustId = (int)belongingCheckingBase.TargetCustId,
                Type = (int)DownLineRole.Agent
            };

            return belongingCheckingBase.TargetRoleId == (int)Role.AgentLevel1 ?
                this.IsDirectDownline(criteria) :
                SessionManager.CurrentSession.AgentInfo.IsLevel0 ? this.IsDownline(criteria) : this.IsDirectDownline(criteria);
        }

        /// <summary>
        /// Checks for editing member setting.
        /// </summary>
        /// <param name="editingCustomerSettingBelonging">The editing customer setting belonging.</param>
        /// <returns>Is valid or not.</returns>
        private bool CheckForEditingMemberSetting(EditingCustomerSettingBelonging editingCustomerSettingBelonging)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = editingCustomerSettingBelonging.CustId,
                TargetCustId = (int)editingCustomerSettingBelonging.TargetCustId,
                Type = (int)DownLineRole.Member
            };

            return editingCustomerSettingBelonging.FormId == (int)FormType.EditIndirectMember ?
                this.IsDownline(criteria) : this.IsDirectDownline(criteria);
        }
    }
}