// ---------------------------------------------------------------------------------
// <copyright file="ILASSystemParlayDetail.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Jacbo.Phan" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace LicenseeAgent.BetList.Utilities
{
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Build html for system parlay ticket detail.
    /// </summary>
    public interface ILASSystemParlayDetail
    {
        string Build();
    }
}