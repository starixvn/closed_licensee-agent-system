// ----------------------------------------------------------------------
// <copyright file="BetListDetailsViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/25">Created</change>
//   <change who="" date=""></change>
// </history>

namespace LicenseeAgent.BetList.ViewModels
{
    using System.Collections.Generic;
    using Entities;

    /// <summary>
    /// Bet List Details View Model.
    /// </summary>
    public class BetListDetailsViewModel
    {
        public BetListDetailsViewModel()
        {
            BetListDetails = new List<BetListDetails>();
        }

        public List<BetListDetails> BetListDetails { get; set; }
    }
}
