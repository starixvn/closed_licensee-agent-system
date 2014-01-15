// --------------------------------------------------------------------------
// <copyright file="ResetPasswordCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.1.14">Created</change>
// </history>

namespace LicenseeAgent.Account.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Reset Password Criteria.
    /// </summary>
    public class ResetPasswordCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int AgentId { get; set; }

        public string Password { get; set; }

        public string UserSalt { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_ResetPassword_Agent";
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
