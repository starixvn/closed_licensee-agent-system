// ----------------------------------------------------------------------
// <copyright file="AgentOutstanding.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2014/02/12">Created</change>
//   <change who="" date=""></change>
// </history>

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
