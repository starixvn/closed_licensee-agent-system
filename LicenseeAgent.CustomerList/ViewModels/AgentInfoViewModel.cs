// -----------------------------------------------------------------------
// <copyright file="AgentInfoViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.ViewModels
{
    /// <summary>
    /// AgentInfoViewModel class.
    /// </summary>
    public class AgentInfoViewModel
    {
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public short AgentLevel { get; set; }

        public short NumberOfLevel { get; set; }
    }
}