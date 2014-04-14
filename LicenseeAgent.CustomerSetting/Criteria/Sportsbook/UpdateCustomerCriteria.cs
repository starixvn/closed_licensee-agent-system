// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2013.12.06">Created</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Criteria.Sportsbook
{
    using Fanex.NAP.Framework.Data;

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
        }

        public int Updater { get; set; }

        public string Ip { get; set; }

        public string Discount { get; set; }

        public string PositionTaking { get; set; }

        public string BetLimit { get; set; }

        public int MsgId { get; set; }

        public int BetLimitSportType { get; set; }

        public int BetLimitBetType { get; set; }

        public int PTSportType { get; set; }

        public int PTBetType { get; set; }

        public short MinPTOutput { get; set; }

        public short MaxPTOutput { get; set; }

        public decimal BetLimitOutput { get; set; }

        public bool IsLiveOutput { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}