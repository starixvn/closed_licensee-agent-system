// ---------------------------------------------------------------------------------
// <copyright file="TotalbetsTicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Utilities
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class TotalbetsTicketBuilder : TicketBuilder
    {
        public TotalbetsTicketBuilder(TicketBuilderParameters ticketBuilderParameters)
            : base(ticketBuilderParameters)
        {
        }

        public override void Initialize()
        {
            ShowOdds = false;
            StartIndex = 1;
            IsShowMember = true;
            FormatDetail = "fanex.betlist.core.showSystemParlayDetails({0},{1},\"{2}\")";
            ClickFunction = "onclick=\"javascript: fanex.betlist.core.showDetails({0}, '{1}',{2},{3})\"";
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