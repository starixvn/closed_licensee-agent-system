// ----------------------------------------------------------------------
// <copyright file="ChangeStatusMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeStatusMemberCriteria : MultipleLogActionCriteria
    {
        public override string GetSettingKey()
        {
            return "LAS_ChangeStatus_Member_Writelog";
        }
    }
}