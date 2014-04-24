// ----------------------------------------------------------------------
// <copyright file="MemberService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Criterias;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Filter;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Utility;

    /// <summary>
    /// MemberService class.
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
            return ServiceWrapper.GetActionResponse<ResponseResult>(action);
        }

        /// <summary>
        /// Insert member.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>
        /// Response result.
        /// </returns>
        public ResponseResult InsertNewMember(InsertNewMemberAction action, bool isReinvoke = false)
        {
            action.FirstName = action.FirstName ?? Constants.EmptyString;
            action.LastName = action.LastName ?? Constants.EmptyString;
            action.Phone = action.Phone ?? string.Empty;
            action.OtherPhone = action.OtherPhone ?? string.Empty;
            action.Fax = action.Fax ?? string.Empty;
            action.EncryptedPassword = Cryptography.CfsEnCrypt(action.Password);

            return ExecuteSingleLog(action, isReinvoke);
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>
        /// Response result.
        /// </returns>
        public ResponseResult ResetPassword(ResetPasswordAction action, bool isReinvoke = false)
        {
            action.EncryptedPassword = Cryptography.CfsEnCrypt(action.Password);

            return ExecuteSingleLog(action, isReinvoke);
        }

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>
        /// Response result.
        /// </returns>
        public ResponseResult UpdateMemberInfo(UpdateMemberInfoAction action, bool isReinvoke = false)
        {
            action.FirstName = action.FirstName ?? Constants.EmptyString;
            action.LastName = action.LastName ?? Constants.EmptyString;
            action.Phone = action.Phone ?? string.Empty;
            action.OtherPhone = action.OtherPhone ?? string.Empty;
            action.Fax = action.Fax ?? string.Empty;

            return ExecuteSingleLog(action, isReinvoke);
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
            var memberResult = ServiceWrapper.GetActionResponse<MemberResult>(action);

            if (memberResult.ErrCode == Result.SuccessValue)
            {
                ServiceMapper.MapObject(memberResult.Member, memberEntity);
            }
            else
            {
                ServiceExtension.WriteError(action, memberResult);
                memberResult.ErrCode = Result.SuccessValue;
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
            var memberListResult = ServiceWrapper.GetActionResponse<MemberListResult>(action);

            if (memberListResult.ErrCode == Result.SuccessValue)
            {
                ServiceMapper.MapObjects(memberListResult.Members, memberEntitys, new MemberFilter());
            }
            else
            {
                ServiceExtension.WriteError(action, memberListResult);
                memberListResult.ErrCode = Result.SuccessValue;
            }

            return memberListResult;
        }

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateMemberStatus(UpdateMemberStatusAction action, bool isReinvoke = false)
        {
            var criteria = (ChangeStatusMemberCriteria)action.CreateCriteria();
            var customerLogs = _logService.GetUpdateMemberStatusLogs(criteria);

            var memberStatusResult = ServiceWrapper.GetActionResponse<MemberStatusResult>(action);
            if (ServiceExtension.IsUpdateMultipleStatus(action.IBCCustId))
            {
                ServiceExtension.UpdateLogInformations(customerLogs, memberStatusResult);
            }
            else
            {
                customerLogs.FirstOrDefault().Status = memberStatusResult.ErrCode != Result.SuccessValue ? LogType.Fail : LogType.Success;
            }

            try
            {
                var updateMemberStatusActionLog = new UpdateMemberStatusActionLog
                {
                    CustomerLogs = customerLogs.ToList()
                };
                criteria.LogXml = ObjectSerializer<UpdateMemberStatusActionLog>.Serialize(updateMemberStatusActionLog);
                criteria.ApiErrCode = memberStatusResult.ApiErrCode;
                criteria.ApiErrMsg = memberStatusResult.ApiErrMsg;

                if (!isReinvoke)
                {
                    _logService.WriteLog(criteria);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex);
            }

            ServiceExtension.WriteError(action, memberStatusResult);
            return memberStatusResult;
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Return action response result.</returns>
        private ResponseResult ExecuteSingleLog(LogAction action, bool isReinvoke)
        {
            var criteria = (SingleLogActionCriteria)action.CreateCriteria();
            _logService.WriteLog(criteria);
            if (criteria.LogIdReturn == 0)
            {
                return new ResponseResult(Result.ApiError, Las.HttpError_Default);
            }

            var responseResult = ServiceWrapper.GetActionResponse<ResponseResult>(action);
            try
            {
                criteria.LogId = criteria.LogIdReturn;
                criteria.ApiErrCode = responseResult.ApiErrCode;
                criteria.ApiErrMsg = responseResult.ApiErrMsg;

                if (!isReinvoke)
                {
                    _logService.WriteLog(criteria);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex);
            }

            ServiceExtension.WriteError(action, responseResult);
            return responseResult;
        }
    }
}