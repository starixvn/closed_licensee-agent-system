// ----------------------------------------------------------------------
// <copyright file="DownLineEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Entities
{
    /// <summary>
    /// DownLineEntity class.
    /// </summary>
    public class DownLineEntity
    {
        public int DownlineCustId { get; set; }

        public string UserName { get; set; }

        public int Level { get; set; }
    }
}