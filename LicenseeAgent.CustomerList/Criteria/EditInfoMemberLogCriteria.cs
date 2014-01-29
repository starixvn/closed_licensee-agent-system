// --------------------------------------------------------------------------
// <copyright file="EditInfoMemberLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2014.1.20">Created</change>
// </history>

namespace LicenseeAgent.CustomerList.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Edit Info Member Log Criteria.
    /// </summary>
    public class EditInfoMemberLogCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public string Doing { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_EditInfo_Member_Writelog";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}