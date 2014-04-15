// -----------------------------------------------------------------------
// <copyright file="TeamEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utility;

    /// <summary>
    /// TeamEntity class.
    /// </summary>
    public class TeamEntity
    {
        private string _teamName;

        public int TeamId { get; set; }

        public string TeamName
        {
            get
            {
                return _teamName ?? (_teamName = DataRefWrapper.GetTeamName(TeamId));
            }

            set
            {
                _teamName = value;
            }
        }
    }
}
