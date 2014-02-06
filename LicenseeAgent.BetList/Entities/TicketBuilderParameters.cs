// ----------------------------------------------------------------------
// <copyright file="TicketBuilderParameters.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014/02/06">Created.</change>
// </history>

namespace LicenseeAgent.BetList.Entities
{
    using LicenseeAgent.BetList.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketBuilderParameters
    {
        public TicketResult TicketResult { get; set; }

        public bool IsAdmin { get; set; }
    }
}
