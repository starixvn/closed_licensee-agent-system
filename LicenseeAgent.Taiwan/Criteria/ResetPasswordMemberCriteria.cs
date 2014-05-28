// ----------------------------------------------------------------------
// <copyright file="ResetPasswordMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criteria
{
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// ResetPasswordMemberCriteria class.
    /// </summary>
    public class ResetPasswordMemberCriteria : SingleLogActionCriteria
    {
        public ResetPasswordMemberCriteria(LogAction logAction)
            : base(logAction)
        {
        }

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