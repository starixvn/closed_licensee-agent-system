// -----------------------------------------------------------------------
// <copyright file="TeamEntity.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TeamEntity
    {
        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return DataRefWrapper.GetTeamname(TeamId);
            }
        }
    }
}
