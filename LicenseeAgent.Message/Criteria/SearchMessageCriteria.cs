// -----------------------------------------------------------------------
// <copyright file="SearchMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2014.02.25">Created.</change>
// </history>

namespace LicenseeAgent.Message.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Search Message Criteria.
    /// </summary>
    public class SearchMessageCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "Las_GetAgentinfo_Detail";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}