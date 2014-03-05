// --------------------------------------------------------------------------
// <copyright file="BetTypeRowItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetTypeRowItem
    {
        public BetTypeRowItem(string title, string betTeam)
        {
            Title = title;
            BetTeam = betTeam;
        }

        public string Title { get; set; }

        public string BetTeam { get; set; }
    }
}