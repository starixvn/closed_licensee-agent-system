namespace LicenseeAgent.Taiwan.Criteria
{
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// AddNewMemberCriteria class.
    /// </summary>
    public class AddNewMemberCriteria : SingleLogActionCriteria
    {
        public AddNewMemberCriteria(LogAction logAction)
            : base(logAction)
        {
        }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_AddNew_Member_Writelog";
        }

        public override bool IsValid()
        {
            return base.IsValid() && CustId > 0;
        }
    }
}