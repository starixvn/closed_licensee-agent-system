namespace LicenseeAgent.Web.Services
{
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Web.Criteria;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Enumerations;

    /// <summary>
    /// Down-line permission service.
    /// </summary>
    public class DownLinePermissionService : IDownLinePermissionService
    {
        private IDownLineService _downLineService;

        public DownLinePermissionService()
            : this(null)
        {
        }

        public DownLinePermissionService(IDownLineService downLineService)
        {
            _downLineService = downLineService ?? new DownLineService();
        }

        public bool CheckForEditingCustomerSetting(CustomerSettingBelonging customerSettingBelonging)
        {
            return customerSettingBelonging.TargetRoleId == (int)Role.Member
                ? CheckForEditingMemberSetting(customerSettingBelonging)
                : CheckForEditingAgentSetting(customerSettingBelonging);
        }

        public bool CheckForInsertingCustomerSetting(CustomerSettingBelonging customerSettingBelonging)
        {
            return customerSettingBelonging.FormId == FormType.Add
                && IsDirectDownLineAgent(customerSettingBelonging.CustId)
                ? true
                : false;
        }

        /// <summary>
        /// Determines whether [is down-line member] [the specified belonging checking base].
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>
        /// Returns true if down-line.
        /// </returns>
        public bool IsDownLineMember(int memberId)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = memberId,
                Type = (int)DownLineRole.Member
            };

            return _downLineService.IsDownLine(criteria);
        }

        /// <summary>
        /// Determines whether [is down-line agent] [the specified belonging checking base].
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns>
        /// Returns true if down-line.
        /// </returns>
        public bool IsDownLineAgent(int agentId)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = agentId,
                Type = (int)DownLineRole.Agent
            };

            return _downLineService.IsDownLine(criteria);
        }

        /// <summary>
        /// Determines whether [is direct down-line agent] [the specified belonging checking base].
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns>
        /// Return true if direct down-line.
        /// </returns>
        public bool IsDirectDownLineAgent(int agentId)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = agentId,
                Type = (int)DownLineRole.Agent
            };

            return SessionManager.CurrentSession.AgentInfo.IsLevel0
            ? _downLineService.IsDownLine(criteria)
            : _downLineService.IsDirectDownLine(criteria);
        }

        /// <summary>
        /// Determines whether [is direct down-line member] [the specified belonging checking base].
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>
        /// Returns true if direct down-line.
        /// </returns>
        public bool IsDirectDownLineMember(int memberId)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = memberId,
                Type = (int)DownLineRole.Member
            };

            return SessionManager.CurrentSession.AgentInfo.IsLevel0
            ? _downLineService.IsDownLine(criteria)
            : _downLineService.IsDirectDownLine(criteria);
        }

        /// <summary>
        /// Determines whether [is indirect down line agent] [the specified agent identifier].
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns>
        /// Returns true if indirect.
        /// </returns>
        public bool IsIndirectDownLineAgent(int agentId)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = agentId,
                Type = (int)DownLineRole.Agent
            };

            return SessionManager.CurrentSession.AgentInfo.IsLevel0
            ? _downLineService.IsDownLine(criteria)
            : _downLineService.IsIndirectDownLine(criteria);
        }

        /// <summary>
        /// Determines whether [is indirect down line member] [the specified member identifier].
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Returns true if indirect.</returns>
        public bool IsIndirectDownLineMember(int memberId)
        {
            var criteria = new CheckDownlineBelongCriteria
            {
                CustId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TargetCustId = memberId,
                Type = (int)DownLineRole.Member
            };

            return SessionManager.CurrentSession.AgentInfo.IsLevel0
            ? _downLineService.IsDownLine(criteria)
            : _downLineService.IsIndirectDownLine(criteria);
        }

        /// <summary>
        /// Is Down Line Transaction.
        /// </summary>
        /// <param name="transactionId">Transaction Id.</param>
        /// <param name="sportType">Sport Type.</param>
        /// <param name="betType">Bet Type of Transaction.</param>
        /// <returns>Is down line or not.</returns>
        public bool IsDownLineTransaction(long transactionId, int sportType, int betType)
        {
            var criteria = new TicketDownLineCriteria
            {
                AgentId = SessionManager.CurrentSession.AgentInfo.AgentId,
                TransId = transactionId,
                SportType = sportType,
                BetType = betType
            };

            return _downLineService.IsDownLineTicket(criteria);
        }

        public bool IsDownLineReferenceNumber(long referenceNumber)
        {
            var criteria = new ReferenceNumberCriteria
            {
                AgentId = SessionManager.CurrentSession.AgentInfo.AgentId,
                RefNo = referenceNumber
            };

            return _downLineService.IsDownLineReferenceNumber(criteria);
        }

        /// <summary>
        /// Checks for editing agent.
        /// </summary>
        /// <param name="editingCustomerSettingBelonging">The editing customer setting belonging.</param>
        /// <returns>
        /// Is valid or not.
        /// </returns>
        private bool CheckForEditingAgentSetting(CustomerSettingBelonging editingCustomerSettingBelonging)
        {
            switch (editingCustomerSettingBelonging.FormId)
            {
                case FormType.Edit:
                    if (IsDirectDownLineAgent(editingCustomerSettingBelonging.TargetCustId))
                    {
                        return true;
                    }

                    break;

                case FormType.ViewIndirectAgent:
                    if (IsIndirectDownLineAgent(editingCustomerSettingBelonging.TargetCustId))
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
        private bool CheckForEditingMemberSetting(CustomerSettingBelonging editingCustomerSettingBelonging)
        {
            switch (editingCustomerSettingBelonging.FormId)
            {
                case FormType.Edit:
                    if (IsDirectDownLineMember(editingCustomerSettingBelonging.TargetCustId))
                    {
                        return true;
                    }

                    break;

                case FormType.EditIndirectMember:
                    if (IsIndirectDownLineMember(editingCustomerSettingBelonging.TargetCustId))
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }
    }
}