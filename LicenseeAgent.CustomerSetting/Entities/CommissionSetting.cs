// --------------------------------------------------------------------------
// <copyright file="CommissionSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Class name.
    /// </summary>
    public class CommissionSetting
    {
        public CommissionSetting()
        {
            CommissionItems = new List<CommissionItem>();
        }

        public IList<CommissionItem> CommissionItems { get; set; }

        public string UserGroup { get; set; }
    }
}