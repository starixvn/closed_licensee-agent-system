namespace LicenseeAgent.Taiwan.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Criteria;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Errors;
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
        /// Gets the deposit member information list.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Returns action result.</returns>
        public GetDepositMemberInfoListResult GetDepositMemberInfoList(GetDepositMemberInfoListAction action)
        {
            var result = ServiceWrapper.GetActionResponse<GetDepositMemberInfoListResult>(action);
            if (result.ApiErrCode == (int)GetDepositMemberInfoListError.DataNotFound && string.IsNullOrWhiteSpace(action.Username))
            {
                result.ErrCode = Result.SuccessValue;
                result.Data = new List<DepositMemberInfo>();
            }

            return result;
        }

        /// <summary>
        /// Insert member.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <param name="logId">Input Log ID.</param>
        /// <returns>
        /// Response result.
        /// </returns>
        public ResponseResult InsertNewMember(InsertNewMemberAction action, bool isReinvoke = false, int logId = 0)
        {
            action.FirstName = action.FirstName ?? Constants.EmptyString;
            action.LastName = action.LastName ?? Constants.EmptyString;
            action.Phone = action.Phone ?? string.Empty;
            action.OtherPhone = action.OtherPhone ?? string.Empty;
            action.Fax = action.Fax ?? string.Empty;
            action.EncryptedPassword = isReinvoke
                ? action.Password
                : Cryptography.CfsEnCrypt(action.Password);

            return ExecuteSingleLog(action, isReinvoke, logId);
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <param name="logId">Input Log ID.</param>
        /// <returns>
        /// Response result.
        /// </returns>
        public ResponseResult ResetPassword(ResetPasswordAction action, bool isReinvoke = false, int logId = 0)
        {
            action.EncryptedPassword = Cryptography.CfsEnCrypt(action.Password);

            return ExecuteSingleLog(action, isReinvoke, logId);
        }

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <param name="logId">Input Log ID.</param>
        /// <returns>
        /// Response result.
        /// </returns>
        public ResponseResult UpdateMemberInfo(UpdateMemberInfoAction action, bool isReinvoke = false, int logId = 0)
        {
            action.FirstName = action.FirstName ?? Constants.EmptyString;
            action.LastName = action.LastName ?? Constants.EmptyString;
            action.Phone = action.Phone ?? string.Empty;
            action.OtherPhone = action.OtherPhone ?? string.Empty;
            action.Fax = action.Fax ?? string.Empty;

            return ExecuteSingleLog(action, isReinvoke, logId);
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
            var memberResult = ServiceWrapper.GetActionResponse<GetMemberInfoResult>(action);
            if (memberResult.ErrCode == Result.SuccessValue)
            {
                ServiceMapper.Map(memberResult.Member, memberEntity);
            }
            else
            {
                ServiceExtension.WriteError(action, memberResult);
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
        public ResponseResult GetMemberInformations<T>(IList<T> memberEntitys, GetMemberInfoListAction action)
        {
            var memberListResult = ServiceWrapper.GetActionResponse<GetMemberInfoListResult>(action);
            if (memberListResult.ErrCode == Result.SuccessValue)
            {
                ServiceMapper.Map(memberListResult.Members, memberEntitys, new MemberFilter());
            }
            else
            {
                if (memberListResult.ErrCode == Result.ApiError)
                {
                    memberListResult.ErrCode = Result.SuccessValue;
                }

                ServiceExtension.WriteError(action, memberListResult);
            }

            return memberListResult;
        }

        /// <summary>
        /// Get member information.
        /// </summary>
        /// <typeparam name="T">Type object response.</typeparam>
        /// <param name="memberEntity">Member entity.</param>
        /// <param name="action">Service action.</param>
        /// <returns>
        /// Member entity after merging.
        /// </returns>
        public ResponseResult GetDepositMemberInformation<T>(T memberEntity, GetDepositMemberInfoAction action)
        {
            var memberResult = ServiceWrapper.GetActionResponse<GetDepositMemberInfoResult>(action);
            if (memberResult.ErrCode == Result.SuccessValue)
            {
                ServiceMapper.Map(memberResult.Member, memberEntity);
            }
            else
            {
                ServiceExtension.WriteError(action, memberResult);
            }

            return memberResult;
        }

        /// <summary>
        /// Update member information.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <param name="logId">Input Log ID.</param>
        /// <returns>Response result.</returns>
        public ResponseResult UpdateAgentId(UpdateAgentIDAction action, bool isReinvoke = false, int logId = 0)
        {
            action.Username = action.Username ?? Constants.EmptyString;
            action.NewIBCAgentId = action.NewIBCAgentId ?? 0;
            action.NewLASAgentId = action.NewLASAgentId ?? 0;

            return ExecuteSingleLog(action, isReinvoke, logId);
        }

        /// <summary>
        /// Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Verified.")]
        public ResponseResult UpdateMemberStatus(UpdateMemberStatusAction action)
        {
            var criteria = (ChangeStatusMemberCriteria)action.CreateCriteria();
            var customerLogs = _logService.GetUpdateMemberStatusLogs(criteria);

            var memberStatusResult = ServiceWrapper.GetActionResponse<UpdateMemberStatusResult>(action);
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
                criteria.LogXml = ObjectSerializer.Serialize<UpdateMemberStatusActionLog>(updateMemberStatusActionLog);
                criteria.ApiErrCode = memberStatusResult.ApiErrCode;
                criteria.ApiErrMsg = memberStatusResult.ApiErrMsg;

                _logService.WriteLog(criteria);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex);
            }

            ServiceExtension.WriteError(action, memberStatusResult);
            return memberStatusResult;
        }

        /// <summary>
        /// Synchronization Monitoring - Update member status.
        /// </summary>
        /// <param name="action">Service action.</param>
        /// <returns>Response result.</returns>
        public ResponseResult SyncMonitoringUpdateMemberStatus(UpdateMemberStatusAction action)
        {
            return ServiceWrapper.GetActionResponse<UpdateMemberStatusResult>(action);
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="isReinvoke">Is Re-invoke.</param>
        /// <param name="logId">Input Log ID.</param>
        /// <returns>Return action response result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Verified.")]
        private ResponseResult ExecuteSingleLog(LogAction action, bool isReinvoke, int logId)
        {
            var criteria = (SingleLogActionCriteria)action.CreateCriteria();
            if (!isReinvoke)
            {
                _logService.WriteLog(criteria);
            }
            else
            {
                criteria.LogIdReturn = logId;
            }

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

            if (!isReinvoke)
            {
                ServiceExtension.WriteError(action, responseResult);
            }

            return responseResult;
        }
    }
}