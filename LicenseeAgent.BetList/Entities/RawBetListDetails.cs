// ----------------------------------------------------------------------
// <copyright file="RawBetListDetails.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Entities
{
    /// <summary>
    /// RawBetListDetails class.
    /// </summary>
    public class RawBetListDetails
    {
        public int AgentID { get; set; }

        public int AgentLevel { get; set; }

        public string AgentName { get; set; }

        public decimal AgentPT { get; set; }

        public decimal AgentPTPercent { get; set; }

        public decimal AgentComm { get; set; }

        public decimal AgentCommPercent { get; set; }
    }
}