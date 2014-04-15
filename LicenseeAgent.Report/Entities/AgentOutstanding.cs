// ----------------------------------------------------------------------
// <copyright file="AgentOutstanding.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Report.Entities
{
    /// <summary>
    /// Agent Outstanding.
    /// </summary>
    public class AgentOutstanding
    {
        public string Username { get; set; }

        public int Id { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Positiontaking { get; set; }
    }
}
