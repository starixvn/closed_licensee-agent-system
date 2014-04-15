// -----------------------------------------------------------------------
// <copyright file="MemberStatusViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.ViewModels
{
    /// <summary>
    /// MemberStatusViewModel class.
    /// </summary>
    public class MemberStatusViewModel
    {
        public int MemberId { get; set; }

        public int AgentId { get; set; }

        public int Status { get; set; }
    }
}