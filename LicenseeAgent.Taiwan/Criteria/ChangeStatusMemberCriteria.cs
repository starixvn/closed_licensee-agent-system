// ----------------------------------------------------------------------
// <copyright file="ChangeStatusMemberCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Criteria
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
        }

        public override string GetSettingKey()
        {
            return "LAS_ChangeStatus_Member_Writelog";
        }
    }
}