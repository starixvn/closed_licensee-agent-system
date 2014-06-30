namespace LicenseeAgent.CustomerSetting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// UpdateCustomerCriteria class.
    /// </summary>
    public class UpdateCustomerCriteria : CriteriaBase
    {
        public UpdateCustomerCriteria()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCustomerCriteria" /> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public UpdateCustomerCriteria(UpdateCustomerCriteria criteria)
        {
            Ip = criteria.Ip;
            Updater = criteria.Updater;
            Discount = criteria.Discount;
            PositionTaking = criteria.PositionTaking;
            BetLimit = criteria.BetLimit;
            Discount3rd = criteria.Discount3rd;
            PositionTaking3rd = criteria.PositionTaking3rd;
            BetLimit3rd = criteria.BetLimit3rd;
        }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public string Discount { get; set; }

        public string PositionTaking { get; set; }

        public string BetLimit { get; set; }

        public string Discount3rd { get; set; }

        public string PositionTaking3rd { get; set; }

        public string BetLimit3rd { get; set; }

        public string SettingOutput { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}