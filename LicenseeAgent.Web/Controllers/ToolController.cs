// ----------------------------------------------------------------------
// <copyright file="ToolController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Entities;
    using Fanex.Common;
    using Filters;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.ViewModels;
    using Services;

    /// <summary>
    /// Tool controller.
    /// </summary>
    [AdminSite]
    [TrustedAdmin]
    [AccountType(AccountType.CSD_IT)]
    public class ToolController : BaseController
    {
        private readonly IMonitoringService _monitoringService;
        private readonly IMemberService _memberService;

        public ToolController(IMonitoringService monitoringService, IMemberService memberService)
        {
            _monitoringService = monitoringService;
            _memberService = memberService;
        }

        [HttpGet]
        public ActionResult TestMSMQ()
        {
            var message = "This is a MSMQ test";
            Logger.Error(message);
            return Content(message);
        }

        /// <summary>
        /// Tool for check password.
        /// </summary>
        /// <returns>View result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Password encode.
        /// </summary>
        /// <param name="password">Password input.</param>
        /// <returns>Password encoded.</returns>
        [HttpPost]
        public JsonResult PasswordEncode(string password)
        {
            var encodedPassword = Cryptography.CfsEnCrypt(password);

            return Json(encodedPassword);
        }

        /// <summary>
        /// Decode password.
        /// </summary>
        /// <param name="encodedpassword">Encoded password.</param>
        /// <returns>Plain password.</returns>
        [HttpPost]
        public JsonResult PasswordDecode(string encodedpassword)
        {
            var password = Cryptography.CfsDeCrypt(encodedpassword);

            return Json(password);
        }

        [HttpGet]
        public ActionResult WhoAmI()
        {
            return View();
        }

        /// <summary>
        /// Monitoring Data's synchronization.
        /// </summary>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult SyncMonitoring()
        {
            var viewModel = new SyncMonitoringViewModel();
            var apiUrls = _monitoringService.GetAPIUrls(new SyncMonitoringGetAPIUrlCriteria { });
            foreach (var apiUrl in apiUrls)
            {
                viewModel.APIUrls.Add(new SelectListItem
                {
                    Text = apiUrl.Url,
                    Value = apiUrl.Url
                });
            }

            return View(viewModel);
        }

        /// <summary>
        /// Monitoring Data's synchronization.
        /// </summary>
        /// <param name="actionID">Action Identification.</param>
        /// <param name="taiwanAPIUrl">Taiwan API's URL.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public JsonResult SyncMonitoring(int actionID, string taiwanAPIUrl)
        {
            var criteria = new SyncMonitoringCriteria
            {
                ActionID = actionID,
                TaiwanAPIUrl = taiwanAPIUrl
            };

            var failedAPICalls = _monitoringService.GetFailedAPICalls(criteria).ToList();
            foreach (var item in failedAPICalls)
            {
                LogAction logAction = null;
                var inputParamsHtmlString = string.Empty;
                switch (item.ActionId)
                {
                    case (int)LogId.ResetPassword:
                        logAction = ObjectSerializer<ResetPasswordAction>.Deserialize(item.Command);
                        break;

                    case (int)LogId.UpdateMemberInfo:
                        logAction = ObjectSerializer<UpdateMemberInfoAction>.Deserialize(item.Command);
                        break;

                    case (int)LogId.InsertNewMember:
                        logAction = ObjectSerializer<InsertNewMemberAction>.Deserialize(item.Command);
                        break;

                    case (int)LogId.UpdateMemberStatus:
                        var updateMemberAction = ObjectSerializer<UpdateMemberStatusAction>.Deserialize(item.Command);
                        updateMemberAction.IBCCustId = item.CustId.ToString();
                        logAction = updateMemberAction;
                        break;
                }

                if (logAction != null)
                {
                    item.InputParams = logAction.GenerateInputs();
                }
            }

            return Json(new
            {
                List = failedAPICalls,
                TotalItems = failedAPICalls.Count
            });
        }

        /// <summary>
        /// Re-invoke a failed action.
        /// </summary>
        /// <param name="syncData">Synchronization data.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult SingleReInvoke(SyncMonitoring syncData)
        {
            ResponseResult responseResult = null;
            var licenseeID = (int)DomainMapper.GetLicenseeId(Request);
            switch (syncData.ActionId)
            {
                case 301:
                    var resetPasswordAction = ObjectSerializer<ResetPasswordAction>.Deserialize(syncData.Command);
                    resetPasswordAction.LicenseeId = licenseeID;
                    resetPasswordAction.UserId = syncData.UserId;
                    responseResult = _memberService.ResetPassword(resetPasswordAction, true, syncData.KeyId);
                    break;

                case 303:
                    var updateMemberInfoAction = ObjectSerializer<UpdateMemberInfoAction>.Deserialize(syncData.Command);
                    updateMemberInfoAction.LicenseeId = licenseeID;
                    updateMemberInfoAction.UserId = syncData.UserId;
                    responseResult = _memberService.UpdateMemberInfo(updateMemberInfoAction, true, syncData.KeyId);
                    break;

                case 304:
                    var insertNewMemberAction = ObjectSerializer<InsertNewMemberAction>.Deserialize(syncData.Command);
                    insertNewMemberAction.LicenseeId = licenseeID;
                    insertNewMemberAction.UserId = syncData.UserId;
                    responseResult = _memberService.InsertNewMember(insertNewMemberAction, true, syncData.KeyId);
                    break;

                case 305:
                    var updateMemberStatusAction = ObjectSerializer<UpdateMemberStatusAction>.Deserialize(syncData.Command);
                    updateMemberStatusAction.LicenseeId = licenseeID;
                    updateMemberStatusAction.UserId = syncData.UserId;
                    responseResult = _memberService.SyncMonitoringUpdateMemberStatus(updateMemberStatusAction);
                    break;
            }

            UpdateLogAfterReInvoke(responseResult, syncData.KeyId);

            return Json(new
            {
                ErrMsg = responseResult.ApiErrMsg,
                ErrCode = responseResult.ErrCode
            });
        }

        /// <summary>
        /// Ignore a failed action.
        /// </summary>
        /// <param name="criteria">Synchronization data.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult SingleIgnore(SyncMonitoringUpdateStatusCriteria criteria)
        {
            try
            {
                criteria.UserID = CurrentSession.LoginUserId;
                criteria.NewStatusID = LogType.Ignore;
                _monitoringService.UpdateLogStatus(criteria);

                return Json(new { ErrCode = Result.SuccessValue });
            }
            catch
            {
                return Json(new { ErrCode = Result.FailureValue });
            }
        }

        /// <summary>
        /// Update Log After Re-Invoke.
        /// </summary>
        /// <param name="responseResult">Response result.</param>
        /// <param name="keyId">Input Log ID.</param>
        private void UpdateLogAfterReInvoke(ResponseResult responseResult, int keyId)
        {
            if (responseResult.ErrCode == Result.SuccessValue)
            {
                _monitoringService.UpdateLogStatus(new SyncMonitoringUpdateStatusCriteria
                {
                    UserID = CurrentSession.LoginUserId,
                    KeyID = keyId,
                    NewStatusID = LogType.Success
                });
            }
            else
            {
                _monitoringService.UpdateRunCount(new SyncMonitoringUpdateRunCountCriteria
                {
                    KeyID = keyId,
                    Remark = responseResult.ApiErrMsg
                });
            }
        }
    }
}