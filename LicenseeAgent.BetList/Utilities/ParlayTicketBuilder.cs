// ---------------------------------------------------------------------------------
// <copyright file="ParlayTicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Paul.Nguyen" date="2014/02/06">Created.</change>
// </history>
// ---------------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class ParlayTicketBuilder : TicketBuilder, ITicketBuilder
    {
        public ParlayTicketBuilder(TicketBuilderParameters ticketBuilderParameters)
            : base(ticketBuilderParameters)
        {
        }

        public override void Initialize()
        {
            this.ShowOdds = false;
            this.StartIndex = 1;
            this.IsShowMember = true;
            this.FormatDetail = "fanex.totalBets.parlay.showSystemParlayDetails({0},{1},\"{2}\")";
            this.ClickFunction = "onclick=\"javascript: fanex.totalBets.parlay.showParlayDetails({0}, '{1}')\"";
        }

        public override string BuildStatus(LasTicket ticket)
        {
            return Builder.BuildTotalBetsParlayStatus(
                        ticket, TicketHelper, (List<ITicketData>)TicketResult.TicketDataList);
        }

        public override long BuildCustId(LasTicket ticket)
        {
            return ticket.CustId;
        }
    }
}