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
    using LicenseeAgent.Taiwan.Filter;
    using LicenseeAgent.Taiwan.Results;
    using Newtonsoft.Json;

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
            return ServiceWrapper.GetResponseData(action);
        }

        /// <summary>
        /// Insert member.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult InsertNewMember(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult ResetPassword(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberInfo(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
        }

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberStatus(IAction action)
        {
            return ServiceWrapper.GetResponseData(action);
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
            var response = ServiceWrapper.GetResponseData(action);

            if (response.ErrorCode == 0)
            {
                var memberResult = JsonConvert.DeserializeObject<MemberResult>(response.Data);

                ServiceMapper.MapObject<MemberResult, T>(memberResult, memberEntity);
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
            var response = ServiceWrapper.GetResponseData(action);

            if (response.ErrorCode == 0)
            {
                var memberResults = JsonConvert.DeserializeObject<IList<MemberResult>>(response.Data);

                ServiceMapper.MapObjects<MemberResult, T>(memberResults, memberEntitys, new MemberFilter());
            }

            return response;
        }
    }
}
