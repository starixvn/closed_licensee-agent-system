// ---------------------------------------------------------------------------------
// <copyright file="BetListTicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Paul.Nguyen" date="2014/02/06">Created.</change>
// </history>
// ---------------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Utilities
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class BetListTicketBuilder : TicketBuilder, ITicketBuilder
    {
        private BetListCriteriaBase _criteria;

        public BetListTicketBuilder(TicketBuilderParameters ticketBuilderParameters, BetListCriteriaBase criteria)
            : base(ticketBuilderParameters)
        {
            _criteria = criteria;
        }

        public override void Initialize()
        {
            this.ShowOdds = false;
            this.StartIndex = 1;
            this.IsShowMember = false;
            this.FormatDetail = "fanex.betlist.core.showSystemParlayDetails({0},{1},\"{2}\")";
            this.ClickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0},'{1}')\"";
        }

        public override string BuildStatus(LasTicket ticket)
        {
            return Builder.BuildLasBetListResultStatus(
                                    ticket, TicketHelper, (List<ITicketData>)TicketResult.TicketDataList, _criteria, 20, "UUS");
        }

        public override long BuildCustId(LasTicket ticket)
        {
            return this._criteria.CustId != 0 ? ticket.CustId : this._criteria.CustId;
        }
    }
}