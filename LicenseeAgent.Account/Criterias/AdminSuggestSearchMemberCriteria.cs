// ----------------------------------------------------------------------
// <copyright file="AdminSuggestSearchMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

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
    public class AdminSuggestSearchMemberCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }
        
        public int CurrencyId { get; set; }

        public int UserId { get; set; }

        [SpParam(SqlParamName = "StrSearch")]
        public string UserName { get; set; }

        public int MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "Las_SuggestSearchMember_Admin";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 
                && CurrencyId > 0 
                && UserId > 0 
                && !string.IsNullOrEmpty(UserName);
        }
    }
}
