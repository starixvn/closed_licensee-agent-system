// ----------------------------------------------------------------------
// <copyright file="MemberService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using LicenseeAgent.Core.Configurations;
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
            action.FirstName = action.FirstName ?? Constants.EmptyString;
            action.LastName = action.LastName ?? Constants.EmptyString;
            action.EncryptedPassword = Cryptography.CfsEnCrypt(action.Password);

            var criteria = new AddNewMemberCriteria
            {
                LicenseeId = action.LicenseeId,
                UserId = action.UserId,
                CustId = action.IBCCustId,
                Status = (int)LogType.BeforeCall,
                Doing = LogBinder.GetActionInformation(action)
            };
            _logService.WriteLog(criteria);

            var responseResult = ServiceWrapper.GetResponseData<ResponseResult>(action);

            criteria.LogId = criteria.LogIdReturn;
            criteria.Status = responseResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            criteria.ErrMsg = responseResult.ErrMsg;
            _logService.WriteLog(criteria);

            ServiceExtension.WriteError(action, responseResult);

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
                Doing = LogBinder.GetActionInformation(action)
            };
            _logService.WriteLog(criteria);

            var responseResult = ServiceWrapper.GetResponseData<ResponseResult>(action);

            criteria.LogId = criteria.LogIdReturn;
            criteria.Status = responseResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            criteria.ErrMsg = responseResult.ErrMsg;
            _logService.WriteLog(criteria);

            ServiceExtension.WriteError(action, responseResult);

            return responseResult;
        }

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberInfo(UpdateMemberInfoAction action)
        {
            action.FirstName = action.FirstName ?? Constants.EmptyString;
            action.LastName = action.LastName ?? Constants.EmptyString;
            action.Phone = action.Phone ?? string.Empty;
            action.OtherPhone = action.OtherPhone ?? string.Empty;
            action.Fax = action.Fax ?? string.Empty;

            var criteria = new EditInfoMemberCriteria
            {
                LicenseeId = action.LicenseeId,
                UserId = action.UserId,
                CustId = action.IBCCustId,
                Status = (int)LogType.BeforeCall,
                Doing = LogBinder.GetActionInformation(action)
            };
            _logService.WriteLog(criteria);

            var responseResult = ServiceWrapper.GetResponseData<ResponseResult>(action);

            criteria.LogId = criteria.LogIdReturn;
            criteria.Status = responseResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            criteria.ErrMsg = responseResult.ErrMsg;
            _logService.WriteLog(criteria);

            ServiceExtension.WriteError(action, responseResult);

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
                MemberStatus = LogBinder.GetMembersContent(ServiceExtension.GetLogInformations(action.IBCCustId, action.LASAgentId)),
                Doing = LogBinder.GetActionInformation(action)
            };

            var logInformations = _logService.GetLogInformations(criteria);
            var memberStatusResult = ServiceWrapper.GetResponseData<MemberStatusResult>(action);

            if (ServiceExtension.IsUpdateMultipleStatus(action.IBCCustId))
            {
                ServiceExtension.UpdateLogInformations(logInformations, memberStatusResult);
            }
            else
            {
                logInformations.FirstOrDefault().Status = memberStatusResult.ErrCode != 0 ? (short)LogType.Fail : (short)LogType.Success;
            }

            criteria.ErrMsg = memberStatusResult.ErrMsg;
            criteria.MemberStatus = LogBinder.GetMembersContent(logInformations);

            _logService.WriteLog(criteria);

            ServiceExtension.WriteError(action, memberStatusResult);

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

            ServiceExtension.WriteError(action, memberResult);

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

            ServiceExtension.WriteError(action, memberListResult);

            return memberListResult;
        }
    }
}
