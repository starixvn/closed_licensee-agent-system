// ----------------------------------------------------------------------
// <copyright file="ChangeStatusMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeStatusMemberCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        [SpParam(SqlParamName = "strXML")]
        public string MemberStatus { get; set; }

        public string Doing { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_ChangeStatus_Member_Writelog";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 && UserId > 0;
        }
    }
}
