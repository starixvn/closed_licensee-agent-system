// --------------------------------------------------------------------------
// <copyright file="SportEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.TotalBets.Entities
{
    using LicenseeAgent.Core.Utils;

    /// <summary>
    /// League Entity.
    /// </summary>
    public class SportEntity
    {
        public int SportType { get; set; }

        public string SportName
        {
            get
            {
                return DataRefWrapper.GetSportName(SportType);
            }
        }        
    }
}