// ----------------------------------------------------------------------
// <copyright file="IMemberService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System.Collections.Generic;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Results;

    public interface IMemberService
    {
        /// <summary>
        /// Check username.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult CheckUserName(CheckUserNameAction action);

        /// <summary>
        /// Insert member.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <returns>Response result.</returns>
        ResponseResult InsertNewMember(InsertNewMemberAction action, bool isReinvoke = false);

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <returns>Response result.</returns>
        ResponseResult ResetPassword(ResetPasswordAction action, bool isReinvoke = false);

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <returns>Response result.</returns>
        ResponseResult UpdateMemberInfo(UpdateMemberInfoAction action, bool isReinvoke = false);

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult UpdateMemberStatus(UpdateMemberStatusAction action);

        /// <summary>
        /// Synchronization Monitoring - Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult SyncMonitoringUpdateMemberStatus(UpdateMemberStatusAction action);

        /// <summary>
        /// Get member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntity">Member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>Member entity after merging.</returns>
        ResponseResult GetMemberInformation<T>(T memberEntity, GetMemberInfoAction action);

        /// <summary>
        /// Get list member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntitys">List member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>List member entity after merging.</returns>
        ResponseResult GetMemberInformations<T>(IEnumerable<T> memberEntitys, GetMemberInfoListAction action);
    }
}
