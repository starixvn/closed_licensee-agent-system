// ---------------------------------------------------------------------------------
// <copyright file="TotalbetsTicketBuilder.cs" company="Nexcel Solutions Vietnam">
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
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class TotalbetsTicketBuilder : TicketBuilder, ITicketBuilder
    {
        public TotalbetsTicketBuilder(TicketBuilderParameters ticketBuilderParameters)
            : base(ticketBuilderParameters)
        {
        }

        public override void Initialize()
        {
            this.ShowOdds = false;
            this.StartIndex = 1;
            this.IsShowMember = true;
            this.FormatDetail = "fanex.betlist.core.showSystemParlayDetails({0},{1},\"{2}\")";
            this.ClickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0}, '{1}')\"";
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