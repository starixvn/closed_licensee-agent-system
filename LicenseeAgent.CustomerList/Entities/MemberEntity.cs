// -----------------------------------------------------------------------
// <copyright file="MemberEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Entities
{
    using System;
    using LicenseeAgent.CustomerList.Core;
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

        public string SystemId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string IpLogin { get; set; }

        public string GroupName { get; set; }

        public int RowNumber { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public int Status { get; set; }

        public string ShortUserName
        {
            get
            {
                if (UserName.Length > Constants.MediumStringLength)
                {
                    return string.Format("{0}...", UserName.Substring(0, Constants.MediumStringLength));
                }

                return UserName;
            }
        }
    }
}