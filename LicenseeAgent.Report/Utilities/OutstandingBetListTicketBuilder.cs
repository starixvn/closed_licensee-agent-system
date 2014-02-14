// ----------------------------------------------------------------------
// <copyright file="OutstandingBetListTicketBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/13">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.Report.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using LicenseeAgent.BetList.Criteria;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Utilities;
    using LicenseeAgent.Report.Criteria;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web;

    /// <summary>
    /// Build ticket.
    /// </summary>
    public class OutstandingBetListTicketBuilder : TicketBuilder, ITicketBuilder
    {
        public OutstandingBetListTicketBuilder(TicketBuilderParameters ticketBuilderParameters)
            : base(ticketBuilderParameters)
        {
        }

        public override void Initialize()
        {
            this.ShowOdds = false;
            this.StartIndex = 1;
            this.IsShowMember = false;
            this.FormatDetail = "OUTSTANDING.showSystemParlayResult({0},{1},\"{2}\")";
            this.ClickFunction = "onclick=\"javascript: OUTSTANDING.showBetListDetails({0},'{1}')\"";
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