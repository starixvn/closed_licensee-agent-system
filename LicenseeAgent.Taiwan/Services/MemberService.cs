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
    using Fanex.Common;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Criterias;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Filter;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberService : IMemberService
    {
        private readonly ILogService _logService;

        public MemberService()
        {
            _logService = new LogService();
        }

        /// <summary>
        /// Check username.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult CheckUserName(CheckUserNameAction action)
        {
            return ServiceWrapper.GetResponseData<ResponseResult>(action);
        }

        /// <summary>
        /// Insert member.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult InsertNewMember(InsertNewMemberAction action)
        {
            action.EncryptedPassword = Cryptography.CfsEnCrypt(action.Password);

            var criteria = new AddNewMemberCriteria
            {
                LicenseeId = action.LicenseeId,
                UserId = action.UserId,
                CustId = action.IBCCustId,
                Status = (int)LogType.BeforeCall,
                Doing = LogBinder.GetActionInformation<InsertNewMemberAction>(action)
            };
            _logService.WriteLog(criteria);

            var responseResult = ServiceWrapper.GetResponseData<ResponseResult>(action);

            criteria.LogId = criteria.LogIdReturn;
            criteria.Status = responseResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            criteria.ErrMsg = responseResult.ErrMsg;
            _logService.WriteLog(criteria);

            return responseResult;
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult ResetPassword(ResetPasswordAction action)
        {
            action.EncryptedPassword = Cryptography.CfsEnCrypt(action.Password);
            var criteria = new ResetPasswordMemberCriteria
            {
                LicenseeId = action.LicenseeId,
                UserId = action.UserId,
                CustId = action.IBCCustId,
                Status = (int)LogType.BeforeCall,
                Doing = LogBinder.GetActionInformation<ResetPasswordAction>(action)
            };
            _logService.WriteLog(criteria);

            var responseResult = ServiceWrapper.GetResponseData<ResponseResult>(action);

            criteria.LogId = criteria.LogIdReturn;
            criteria.Status = responseResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            criteria.ErrMsg = responseResult.ErrMsg;
            _logService.WriteLog(criteria);

            return responseResult;
        }

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberInfo(UpdateMemberInfoAction action)
        {
            var criteria = new EditInfoMemberCriteria
            {
                LicenseeId = action.LicenseeId,
                UserId = action.UserId,
                CustId = action.IBCCustId,
                Status = (int)LogType.BeforeCall,
                Doing = LogBinder.GetActionInformation<UpdateMemberInfoAction>(action)
            };
            _logService.WriteLog(criteria);

            var responseResult = ServiceWrapper.GetResponseData<ResponseResult>(action);

            criteria.LogId = criteria.LogIdReturn;
            criteria.Status = responseResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            criteria.ErrMsg = responseResult.ErrMsg;
            _logService.WriteLog(criteria);

            return responseResult;
        }

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberStatus(UpdateMemberStatusAction action)
        {
            var criteria = new ChangeStatusMemberCriteria
            {
                LicenseeId = action.LicenseeId,
                UserId = action.UserId,
                AgentId = action.LASAgentId,
                MemberId = action.IBCCustId,
                Status = (int)LogType.BeforeCall,
                Doing = LogBinder.GetActionInformation<UpdateMemberStatusAction>(action)
            };
            var logInformations = _logService.GetLogInformations(criteria);

            var memberStatusResult = ServiceWrapper.GetResponseData<MemberStatusResult>(action);

            criteria.LogId = criteria.LogIdReturn;
            criteria.Status = memberStatusResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            criteria.ErrMsg = memberStatusResult.ErrMsg;
            _logService.WriteLog(criteria);

            return memberStatusResult;
        }

        /// <summary>
        /// Get member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntity">Member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>Member entity after merging.</returns>
        public ResponseResult GetMemberInformation<T>(T memberEntity, GetMemberInfoAction action)
        {
            var memberResult = ServiceWrapper.GetResponseData<MemberResult>(action);

            if (memberResult.ErrCode == 0)
            {
                ServiceMapper.MapObject<ServiceMemberEntity, T>(memberResult.Member, memberEntity);
            }
            else
            {
                memberResult.ErrCode = 0;
            }

            return memberResult;
        }

        /// <summary>
        /// Get list member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntitys">List member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>List member entity after merging.</returns>
        public ResponseResult GetMemberInformations<T>(IEnumerable<T> memberEntitys, GetMemberInfoListAction action)
        {
            var memberListResult = ServiceWrapper.GetResponseData<MemberListResult>(action);

            if (memberListResult.ErrCode == 0)
            {
                ServiceMapper.MapObjects<ServiceMemberEntity, T>(memberListResult.Members, memberEntitys, new MemberFilter());
            }

            return memberListResult;
        }
    }
}
