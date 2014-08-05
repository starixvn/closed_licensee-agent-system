namespace LicenseeAgent.CustomerSetting.Entities
{
    /// <summary>
    /// CommissionGroup class.
    /// </summary>
    public class GroupCommission
    {
        public int GroupCommId { get; set; }

        public string GroupName { get; set; }

        public decimal SBComm { get; set; }

        public decimal NGComm { get; set; }

        public decimal RacingComm { get; set; }

        public decimal LCComm { get; set; }

        public decimal RNGComm { get; set; }

        public int NumberOfCustomer { get; set; }

        public bool IsDefault { get; set; }
    }
}
