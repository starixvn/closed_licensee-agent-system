// ----------------------------------------------------------------------
// <copyright file="CustomerEntity.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerEntity
    {
        public int CustId { get; set; }

        public int Currency { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public int Level { get; set; }

        public int NumberOfLevel { get; set; }
    }
}
