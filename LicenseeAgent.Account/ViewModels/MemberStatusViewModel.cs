// -----------------------------------------------------------------------
// <copyright file="MemberStatusViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberStatusViewModel
    {
        public int MemberId { get; set; }

        public int AgentId { get; set; }

        public byte Status { get; set; }
    }
}
