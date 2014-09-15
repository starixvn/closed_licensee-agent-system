namespace LicenseeAgent.ProductSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// InsertCustomerCriteria class.
    /// </summary>
    public class InsertCustomerCriteria : CriteriaBase
    {
        public InsertCustomerCriteria()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCustomerCriteria"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public InsertCustomerCriteria(InsertCustomerCriteria criteria)
        {
            UserName = criteria.UserName;
            Credit = criteria.Credit;
            Ip = criteria.Ip;
            Creator = criteria.Creator;
            Discount = criteria.Discount;
            PositionTaking = criteria.PositionTaking;
            BetLimit = criteria.BetLimit;
            Discount3RD = criteria.Discount3RD;
            BetLimit3RD = criteria.BetLimit3RD;
            PositionTaking3RD = criteria.PositionTaking3RD;
        }

        public string UserName { get; set; }

        public decimal Credit { get; set; }

        public int Creator { get; set; }

        public string Ip { get; set; }

        public string Discount { get; set; }

        public string PositionTaking { get; set; }

        public string BetLimit { get; set; }

        public string Discount3RD { get; set; }

        public string PositionTaking3RD { get; set; }

        public string BetLimit3RD { get; set; }

        public int MsgId { get; set; }

        public decimal MaxCreditOutput { get; set; }

        public decimal BetLimitOutput { get; set; }

        public int BetLimitSportType { get; set; }

        public int BetLimitBetType { get; set; }

        public int PTSportType { get; set; }

        public int PTBetType { get; set; }

        public bool IsLiveOutput { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}