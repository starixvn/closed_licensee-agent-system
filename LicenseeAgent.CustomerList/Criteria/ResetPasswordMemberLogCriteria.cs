// --------------------------------------------------------------------------
// <copyright file="ResetPasswordMemberLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.1.16">Created</change>
// </history>

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Reset Password Member Log Criteria.
    /// </summary>
    public class ResetPasswordMemberLogCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public string Doing { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_ResetPassword_Member_Writelog";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}