namespace LicenseeAgent.Taiwan.Criteria
{
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// EditInfoMemberCriteria class.
    /// </summary>
    public class UpdateAgenIdCriteria : SingleLogActionCriteria
    {
        public UpdateAgenIdCriteria(LogAction logAction)
            : base(logAction)
        {
        }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_EditInfo_Member_Writelog";
        }

        public override bool IsValid()
        {
            return base.IsValid();
        }
    }
}