// -----------------------------------------------------------------------
// <copyright file="BreadcrumbItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Web.Entities
{
    /// <summary>
    /// Breadcrumb item.
    /// </summary>
    public class BreadcrumbItem
    {
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public short AgentLevel { get; set; }

        public bool IsValid()
        {
            return AgentId > 0 && !string.IsNullOrEmpty(AgentName) && AgentLevel > 0;
        }
    }
}