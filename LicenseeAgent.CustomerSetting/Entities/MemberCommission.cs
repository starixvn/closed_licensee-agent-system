namespace LicenseeAgent.CustomerSetting.Entities
{
    /// <summary>
    /// MemberCommission class.
    /// </summary>
    public class MemberCommission
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string SystemId { get; set; }

        public string GroupName { get; set; }

        public decimal SBComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal RacingComm { get; set; }

        public decimal LCComm { get; set; }

        public decimal RNGComm { get; set; }
    }
}
