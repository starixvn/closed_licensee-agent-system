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
    using Services;

    /// <summary>
    /// Tool controller.
    /// </summary>
    [AdminSite]
    [AccountType(AccountType.CSD_IT)]
    public class ToolController : Controller
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
            return View();
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
                    responseResult = _memberService.ResetPassword(resetPasswordAction, true);
                    break;

                case 303:
                    var updateMemberInfoAction = ObjectSerializer<UpdateMemberInfoAction>.Deserialize(syncData.Command);
                    updateMemberInfoAction.LicenseeId = licenseeID;
                    updateMemberInfoAction.UserId = syncData.UserId;
                    responseResult = _memberService.UpdateMemberInfo(updateMemberInfoAction, true);
                    break;

                case 304:
                    var insertNewMemberAction = ObjectSerializer<InsertNewMemberAction>.Deserialize(syncData.Command);
                    insertNewMemberAction.LicenseeId = licenseeID;
                    insertNewMemberAction.UserId = syncData.UserId;
                    responseResult = _memberService.InsertNewMember(insertNewMemberAction, true);
                    break;

                case 305:
                    var updateMemberStatusAction = ObjectSerializer<UpdateMemberStatusAction>.Deserialize(syncData.Command);
                    updateMemberStatusAction.LicenseeId = licenseeID;
                    updateMemberStatusAction.UserId = syncData.UserId;
                    responseResult = _memberService.UpdateMemberStatus(updateMemberStatusAction, true);
                    break;
            }

            UpdateLofAfterReInvoke(responseResult, syncData.KeyId);

            return Json(new
            {
                Results = responseResult.ApiErrMsg,
                IsSuccess = responseResult.ErrCode == Result.SuccessValue
            });
        }

        private void UpdateLofAfterReInvoke(ResponseResult responseResult, int keyId)
        {
            if (responseResult.ErrCode == Result.SuccessValue)
            {
                _monitoringService.UpdateLogStatus(new SyncMonitoringUpdateStatusCriteria
                {
                    KeyID = keyId,
                    NewStatusID = 2
                });
            }
            else
            {
                _monitoringService.UpdateRunCount(new SyncMonitoringUpdateRunCountCriteria
                {
                    KeyID = keyId
                });
            }
        }
    }
}