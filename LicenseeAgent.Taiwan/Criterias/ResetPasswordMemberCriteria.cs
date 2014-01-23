// ----------------------------------------------------------------------
// <copyright file="ResetPasswordMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResetPasswordMemberCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public string Doing { get; set; }

        public string ErrMsg { get; set; }

        public short Status { get; set; }

        public long LogId { get; set; }

        public long LogIdReturn { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_ResetPassword_Member_Writelog";
        }

        public override bool IsValid()
        {
            return LicenseeId > 0 && UserId > 0 && CustId > 0;
        }
    }
}
