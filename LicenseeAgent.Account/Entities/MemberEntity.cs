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
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Taiwan;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberEntity
    {
        [Service(Name = "LASAgentId")]
        public int AgentId { get; set; }

        [Service(Name = "IBCCustId")]
        public int CustId { get; set; }

        public string UserName { get; set; }

        [Service(Name = "IsClosed")]
        public bool IsClosed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string IpLogin { get; set; }

        public string GroupName { get; set; }

        public int RowNumber { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public byte Status
        {
            get
            {
                return IsClosed ? (byte)AccountStatus.Closed : (byte)AccountStatus.Open;
            }

            set
            {
                IsClosed = value == (byte)AccountStatus.Closed;
            }
        }
    }
}
