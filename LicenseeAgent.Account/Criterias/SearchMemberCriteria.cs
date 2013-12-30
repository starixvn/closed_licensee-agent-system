// -----------------------------------------------------------------------
// <copyright file="SearchMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Account.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchMemberCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public string UserName { get; set; }

        public byte? AccStatus { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SearchMember";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
