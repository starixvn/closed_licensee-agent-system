// ----------------------------------------------------------------------
// <copyright file="ChangeStatusMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criterias
{
    using LicenseeAgent.Taiwan.Actions;

    /// <summary>
    /// ChangeStatusMemberCriteria class.
    /// </summary>
    public class ChangeStatusMemberCriteria : MultipleLogActionCriteria
    {
        public ChangeStatusMemberCriteria(LogAction logAction)
            : base(logAction)
        {
            IsReInvoke = false;
        }

        public bool IsReInvoke { get; set; }

        public override string GetSettingKey()
        {
            return IsReInvoke
                ? "LAS_ChangeStatus_Member_Writelog"
                : "LAS_ChangeStatus_Member_Writelog";
        }
    }
}