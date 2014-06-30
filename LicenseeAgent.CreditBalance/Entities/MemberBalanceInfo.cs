namespace LicenseeAgent.CreditBalance.Entities
{
    using LicenseeAgent.Taiwan.Attributes;

    /// <summary>
    /// MemberBalanceInfo class.
    /// </summary>
    public class MemberBalanceInfo
    {
        public long RowNo { get; set; }

        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        public string CustomerName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string UserName { get; set; }

        public bool Status
        {
            get
            {
                return IsClosed;
            }
        }

        public decimal BetCredit { get; set; }

        public decimal YesterdayBalance { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal Outstanding { get; set; }

        public decimal GivenCredit { get; set; }

        [Service(Name = "IsClosed")]
        public bool IsClosed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}