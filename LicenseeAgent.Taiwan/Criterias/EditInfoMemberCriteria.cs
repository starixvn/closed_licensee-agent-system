// ----------------------------------------------------------------------
// <copyright file="EditInfoMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditInfoMemberCriteria : SingleLogActionCriteria
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "LAS_EditInfo_Member_Writelog";
        }

        public override bool IsValid()
        {
            return base.IsValid() && CustId > 0;
        }
    }
}