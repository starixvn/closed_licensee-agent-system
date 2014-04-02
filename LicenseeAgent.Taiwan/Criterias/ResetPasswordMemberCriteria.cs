// ----------------------------------------------------------------------
// <copyright file="ResetPasswordMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ResetPasswordMemberCriteria : SingleLogActionCriteria
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_ResetPassword_Member_Writelog";
        }

        public override bool IsValid()
        {
            return base.IsValid() && CustId > 0;
        }
    }
}