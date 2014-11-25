namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Criteria;

    /// <summary>
    /// UpdateMemberInfoAction class.
    /// </summary>
    public class UpdateAgentIdAction : LogAction
    {
        public UpdateAgentIdAction()
        {
        }

        public UpdateAgentIdAction(int custId, int currency, string username, string agentName, int ibcAgentId, int legendAgentId)
        {
            IBCCustId = custId;
            MemberCurrencyId = currency;
            Username = username;
            AgentName = agentName;
            NewIBCAgentId = ibcAgentId;
            NewLASAgentId = legendAgentId;
        }

        public int IBCCustId { get; set; }

        public string Username { get; set; }

        public string AgentName { get; set; }

        public int? NewLASAgentId { get; set; }

        public int? NewIBCAgentId { get; set; }

        public int LicenseeCurrencyId { get; set; }

        public int MemberCurrencyId { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "UpdateAgentID";
            }
        }

        /// <summary>
        /// Override Log Action Criteria by Update Agent Id Criteria.
        /// </summary>
        /// <returns>Log Action Criteria.</returns>
        public override LogActionCriteria CreateCriteria()
        {
            return new UpdateAgentIdCriteria(this)
            {
                LicenseeId = LicenseeId,
                UserId = UserId,
                CustId = IBCCustId,
                NewIBCAgentId = NewIBCAgentId,
                NewLASAgentId = NewLASAgentId,
                AgentName = AgentName,
                LicenseeCurrencyId = LicenseeCurrencyId,
                MemberCurrencyId = MemberCurrencyId,
                Doing = DoingLogXml
            };
        }
    }
}