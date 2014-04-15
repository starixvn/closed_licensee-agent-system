// ---------------------------------------------------------------------------------
// <copyright file="BetListTicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
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
    public class BetListTicketBuilder : TicketBuilder
    {
        private readonly BetListCriteriaBase _criteria;

        public BetListTicketBuilder(TicketBuilderParameters ticketBuilderParameters, BetListCriteriaBase criteria)
            : base(ticketBuilderParameters)
        {
            _criteria = criteria;
        }

        public override void Initialize()
        {
            ShowOdds = false;
            StartIndex = 1;
            IsShowMember = false;
            FormatDetail = "fanex.betlist.core.showSystemParlayDetails({0},{1},\"{2}\")";
            ClickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0},'{1}')\"";
        }

        public override string BuildStatus(LasTicket ticket)
        {
            return Builder.BuildLasBetListResultStatus(
                                    ticket, TicketHelper, (List<ITicketData>)TicketResult.TicketDataList, _criteria, 20, "UUS");
        }

        public override long BuildCustId(LasTicket ticket)
        {
            return _criteria.CustId != 0 ? ticket.CustId : _criteria.CustId;
        }
    }
}