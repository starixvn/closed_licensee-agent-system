// -----------------------------------------------------------------------
// <copyright file="AgentEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.CustomerList.Entities
{
    using System;
    using LicenseeAgent.CustomerList.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentEntity
    {
        public AgentEntity()
        {
            this.AgentLevel = 0;
        }

        public int ParentId { get; set; }

        public string ParentUserName { get; set; }

        public string UserName { get; set; }

        public int AgentId { get; set; }

        public bool HasAgentDownline { get; set; }

        public short AgentLevel { get; set; }

        public int Status { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public DateTime CreatedDate { get; set; }

        public string IpLogin { get; set; }

        public int NumberOfAgent { get; set; }

        public int RowNumber { get; set; }

        public int NumberAgentLevel { get; set; }

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