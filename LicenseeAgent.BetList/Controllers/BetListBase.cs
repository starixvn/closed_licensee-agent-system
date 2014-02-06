// ----------------------------------------------------------------------
// <copyright file="BetListBase.cs" company="Nexcel Solutions Vietnam">
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
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BetListBase : BaseController, IBetListBase
    {
        /// <summary>
        /// Format raw bet list details.
        /// </summary>
        /// <param name="originalData">List of raw bet list details.</param>
        /// <param name="status">Ticket status ID.</param>
        /// <returns>List of formatted bet list details.</returns>
        public List<BetListDetails> FormatRawBetListDetails(IEnumerable<RawBetListDetails> originalData, string status)
        {
            var formattedData = new List<BetListDetails>();
            foreach (var item in originalData)
            {
                formattedData.Add(new BetListDetails
                {
                    Agent = item.AgentName,
                    PositionTaking = GenerateHtmlStringForAgentPT(item, status),
                    Commission = GenerateHtmlStringForAgentComm(item, status)
                });
            }

            return formattedData;
        }

        private string GenerateHtmlStringForAgentPT(RawBetListDetails data, string status)
        {
            var colorValue = data.AgentPT >= 0 ? ">" : " class=\"red-text\">";
            var htmlString = "<div>" + string.Format("{0:#,##0.00}", data.AgentPTPercent * 100) + "%</div>";
            htmlString += (status == Constants.BETLISTTICKETSTATUSRUNNING || status == Constants.BETLISTTICKETSTATUSWAITING)
                ? string.Empty
                : "<div" + colorValue + string.Format("{0:#,##0.00}", data.AgentPT) + "</div>";

            return htmlString;
        }

        private string GenerateHtmlStringForAgentComm(RawBetListDetails data, string status)
        {
            var htmlString = data.AgentLevel == 1
                ? string.Empty
                : "<div>" + string.Format("{0:#,##0.00}", data.AgentCommPercent * 100) + "%</div>";
            htmlString += (status == Constants.BETLISTTICKETSTATUSRUNNING || status == Constants.BETLISTTICKETSTATUSWAITING)
                ? (data.AgentLevel == 1 ? "-" : string.Empty)
                : "<div>" + string.Format("{0:#,##0.00}", data.AgentComm) + "</div>";

            return htmlString;
        }
    }
}
