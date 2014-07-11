namespace LicenseeAgent.Taiwan.Actions
{
    using LicenseeAgent.Taiwan.Attributes;
    using LicenseeAgent.Taiwan.Criteria;

    /// <summary>
    /// UpdateMemberInfoAction class.
    /// </summary>
    public class UpdateMemberInfoAction : LogAction
    {
        public int IBCCustId { get; set; }

        public string Phone { get; set; }

        public string OtherPhone { get; set; }

        public string Fax { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Service]
        public override string ActionName
        {
            get
            {
                return "UpdateMemberInfo";
            }
        }

        public override LogActionCriteria CreateCriteria()
        {
            return new EditInfoMemberCriteria(this)
            {
                LicenseeId = LicenseeId,
                UserId = UserId,
                CustId = IBCCustId,
                Doing = DoingLogXml
            };
        }
    }
}