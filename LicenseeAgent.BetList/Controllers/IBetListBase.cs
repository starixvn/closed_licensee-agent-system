// ----------------------------------------------------------------------
// <copyright file="IBetListBase.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014/2/6">Created</change>
// </history>

namespace LicenseeAgent.BetList.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Entities;
    using LicenseeAgent.BetList.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBetListBase
    {
        List<BetListDetails> FormatRawBetListDetails(IEnumerable<RawBetListDetails> originalData, string status);

        string BuildSystemParlayDetail(SystemParlayData systemParlayData);

        ActionResult BuildMixParlayDetails(TicketResult ticketResult);
    }
}
