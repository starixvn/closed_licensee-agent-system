// -----------------------------------------------------------------------
// <copyright file="MemberEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberEntity
    {
        public int AgentId { get; set; }

        public int CustId { get; set; }

        public string UserName { get; set; }

        public byte Status { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string IpLogin { get; set; }

        public string GroupName { get; set; }

        public int RowNumber { get; set; }
    }
}
