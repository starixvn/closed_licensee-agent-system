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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMemberService
    {
        /// <summary>
        /// Check username.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult CheckUserName(IAction action);

        /// <summary>
        /// Insert member.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult InsertNewMember(IAction action);

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult ResetPassword(IAction action);

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult UpdateMemberInfo(IAction action);

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        ResponseResult UpdateMemberStatus(IAction action);

        /// <summary>
        /// Get member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntity">Member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>Member entity after merging.</returns>
        ResponseResult GetMemberInformation<T>(T memberEntity, IAction action);

        /// <summary>
        /// Get list member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntitys">List member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>List member entity after merging.</returns>
        ResponseResult GetMemberInformations<T>(IList<T> memberEntitys, IAction action);
    }
}
