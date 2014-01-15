// ----------------------------------------------------------------------
// <copyright file="MemberService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Filter;
    using LicenseeAgent.Taiwan.Results;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberService : IMemberService
    {
        /// <summary>
        /// Check username.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult CheckUserName(IAction action)
        {
            return ServiceWrapper.GetResponseData<ResponseResult>(action);
        }

        /// <summary>
        /// Insert member.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult InsertNewMember(IAction action)
        {
            return ServiceWrapper.GetResponseData<ResponseResult>(action);
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult ResetPassword(IAction action)
        {
            return ServiceWrapper.GetResponseData<ResponseResult>(action);
        }

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberInfo(IAction action)
        {
            return ServiceWrapper.GetResponseData<ResponseResult>(action);
        }

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberStatus(IAction action)
        {
            return ServiceWrapper.GetResponseData<ResponseResult>(action);
        }

        /// <summary>
        /// Get member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntity">Member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>Member entity after merging.</returns>
        public ResponseResult GetMemberInformation<T>(T memberEntity, IAction action)
        {
            var response = ServiceWrapper.GetResponseData<MemberResult>(action);

            if (response.ErrCode == 0)
            {
                ServiceMapper.MapObject<ServiceMemberEntity, T>(response.Member, memberEntity);
            }

            return response;
        }

        /// <summary>
        /// Get list member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntitys">List member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>List member entity after merging.</returns>
        public ResponseResult GetMemberInformations<T>(IList<T> memberEntitys, IAction action)
        {
            var response = ServiceWrapper.GetResponseData<MemberListResult>(action);

            if (response.ErrCode == 0)
            {
                ServiceMapper.MapObjects<ServiceMemberEntity, T>(response.Members, memberEntitys, new MemberFilter());
            }

            return response;
        }
    }
}
