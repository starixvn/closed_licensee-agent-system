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
        private string _teamName;

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                if (_teamName == null)
                {
                    _teamName = DataRefWrapper.GetTeamname(TeamId);
                }

                return _teamName;
            }
            set
            {
                _teamName = value;
            }
        }
    }
}
