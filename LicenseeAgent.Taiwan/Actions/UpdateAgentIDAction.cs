namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Criteria;

    /// <summary>
    /// UpdateMemberInfoAction class.
    /// </summary>
    public class UpdateAgentIDAction : LogAction
    {
        public string Username { get; set; }

        public int? NewLASAgentID { get; set; }

        public int? NewIBCAgentID { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "UpdateAgentID";
            }
        }

        public override LogActionCriteria CreateCriteria()
        {
            return new EditInfoMemberCriteria(this)
            {
                LicenseeId = LicenseeId,
                UserId = UserId,
                Doing = DoingLogXml
            };
        }
    }
}