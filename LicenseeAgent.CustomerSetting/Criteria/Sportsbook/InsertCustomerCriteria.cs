// --------------------------------------------------------------------------
// <copyright file="InsertCustomerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.30">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
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
            UserPwd = criteria.UserPwd;
            FirstName = criteria.FirstName;
            LastName = criteria.LastName;
            Phone = criteria.Phone;
            Mobile = criteria.Mobile;
            Fax = criteria.Fax;
            Credit = criteria.Credit;
            Status = criteria.Status;
            Ip = criteria.Ip;
            Creator = criteria.Creator;
            Discount = criteria.Discount;
            PositionTaking = criteria.PositionTaking;
            BetLimit = criteria.BetLimit;
        }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public decimal Credit { get; set; }

        public byte Status { get; set; }

        public int Creator { get; set; }

        public string Ip { get; set; }

        public string Discount { get; set; }

        public string PositionTaking { get; set; }

        public string BetLimit { get; set; }

        public int MsgId { get; set; }

        public decimal MaxCreditOutput { get; set; }

        public decimal BetLimitOutput { get; set; }

        public int BetLimitSportType { get; set; }

        public int BetLimitBetType { get; set; }

        public int PTSportType { get; set; }

        public int PTBetType { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName) &&
                !string.IsNullOrEmpty(UserPwd);
        }
    }
}