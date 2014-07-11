namespace LicenseeAgent.Web.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Filters;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Criteria;
    using LicenseeAgent.Web.ViewModels;
    using Services;

    /// <summary>
    /// Tool controller.
    /// </summary>
    [AdminSite]
    [TrustedAdmin]
    [AccountType(AccountType.CSDIT)]
    public class ToolController : BaseController
    {
        private readonly IMonitoringService _monitoringService;
        private readonly IMemberService _memberService;

        public ToolController()
            : this(null, null)
        {
        }

        public ToolController(IMonitoringService monitoringService, IMemberService memberService)
        {
            _monitoringService = monitoringService ?? new MonitoringService();
            _memberService = memberService ?? new MemberService();
        }

        [HttpGet]
        public ActionResult TestMsmq()
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
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Value = encodedPassword
            });
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
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Value = password
            });
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
                    Text = apiUrl,
                    Value = apiUrl
                });
            }

            return View(viewModel);
        }

        /// <summary>
        /// Monitoring Data's synchronization.
        /// </summary>
        /// <param name="actionId">Action Identification.</param>
        /// <param name="taiwanAPIUrl">Taiwan API's URL.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Verified")]
        public JsonResult SyncMonitoring(int actionId, string taiwanAPIUrl)
        {
            var criteria = new SyncMonitoringCriteria
            {
                ActionId = actionId,
                TaiwanAPIUrl = taiwanAPIUrl
            };

            var failedAPICalls = _monitoringService.GetFailedAPICalls(criteria).ToList();
            foreach (var item in failedAPICalls)
            {
                LogAction logAction = null;
                switch (item.ActionId)
                {
                    case (int)LogId.ResetPassword:
                        logAction = ObjectSerializer.Deserialize<ResetPasswordAction>(item.Command);
                        break;

                    case (int)LogId.UpdateMemberInfo:
                        logAction = ObjectSerializer.Deserialize<UpdateMemberInfoAction>(item.Command);
                        break;

                    case (int)LogId.InsertNewMember:
                        logAction = ObjectSerializer.Deserialize<InsertNewMemberAction>(item.Command);
                        break;

                    case (int)LogId.UpdateMemberStatus:
                        var updateMemberAction = ObjectSerializer.Deserialize<UpdateMemberStatusAction>(item.Command);
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
        /// <param name="keyId">The key identifier.</param>
        /// <returns>
        /// JSON object.
        /// </returns>
        [HttpPost]
        public JsonResult SingleReInvoke(int keyId)
        {
            ResponseResult responseResult = null;
            var logData = _monitoringService.GetLogData(keyId);
            switch (logData.ActionId)
            {
                case (int)LogId.ResetPassword:
                    var resetPasswordAction = ObjectSerializer.Deserialize<ResetPasswordAction>(logData.Command);
                    resetPasswordAction.LicenseeId = CurrentSession.OnLicenseeId;
                    resetPasswordAction.UserId = CurrentSession.LoginUserId;
                    responseResult = _memberService.ResetPassword(resetPasswordAction, true, logData.KeyId);
                    break;

                case (int)LogId.UpdateMemberInfo:
                    var updateMemberInfoAction = ObjectSerializer.Deserialize<UpdateMemberInfoAction>(logData.Command);
                    updateMemberInfoAction.LicenseeId = CurrentSession.OnLicenseeId;
                    updateMemberInfoAction.UserId = CurrentSession.LoginUserId;
                    responseResult = _memberService.UpdateMemberInfo(updateMemberInfoAction, true, logData.KeyId);
                    break;

                case (int)LogId.InsertNewMember:
                    var insertNewMemberAction = ObjectSerializer.Deserialize<InsertNewMemberAction>(logData.Command);
                    insertNewMemberAction.LicenseeId = CurrentSession.OnLicenseeId;
                    insertNewMemberAction.UserId = CurrentSession.LoginUserId;
                    responseResult = _memberService.InsertNewMember(insertNewMemberAction, true, logData.KeyId);
                    break;

                case (int)LogId.UpdateMemberStatus:
                    var updateMemberStatusAction = ObjectSerializer.Deserialize<UpdateMemberStatusAction>(logData.Command);
                    updateMemberStatusAction.LicenseeId = CurrentSession.OnLicenseeId;
                    updateMemberStatusAction.UserId = CurrentSession.LoginUserId;
                    responseResult = _memberService.SyncMonitoringUpdateMemberStatus(updateMemberStatusAction);
                    break;
            }

            UpdateLogAfterReInvoke(responseResult, logData.KeyId);

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
            criteria.UserId = CurrentSession.LoginUserId;
            criteria.NewStatusId = LogType.Ignore;
            _monitoringService.UpdateLogStatus(criteria);
            return Json(new { ErrCode = Result.SuccessValue });
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
                    UserId = CurrentSession.LoginUserId,
                    KeyId = keyId,
                    NewStatusId = LogType.Success
                });
            }
            else
            {
                _monitoringService.UpdateRunCount(new SyncMonitoringUpdateRunCountCriteria
                {
                    KeyId = keyId,
                    Remark = responseResult.ApiErrMsg
                });
            }
        }
    }
}