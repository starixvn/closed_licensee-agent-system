// ----------------------------------------------------------------------
// <copyright file="DownlineEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.AccountInformation.Entities
{
    public class DownlineEntity
    {
        public int DownlineCustId { get; set; }

        public string UserName { get; set; }

        public int Level { get; set; }
    }
}