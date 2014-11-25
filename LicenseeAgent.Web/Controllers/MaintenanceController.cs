namespace LicenseeAgent.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
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
    using LicenseeAgent.Web.Criteria;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.ViewModels;
    using Services;

    /// <summary>
    /// Tool controller.
    /// </summary>
    [AdminSite]
    [TrustedAdmin]
    [AccountType(AccountType.CSDIT)]
    public class MaintenanceController : BaseController
    {
        private static readonly Dictionary<string, Type> ActionDictionary = InternalActionDictionary;
        private readonly IMonitoringService _monitoringService;
        private readonly IMemberService _memberService;

        public MaintenanceController()
            : this(null, null)
        {
        }

        public MaintenanceController(IMonitoringService monitoringService, IMemberService memberService)
        {
            _monitoringService = monitoringService ?? new MonitoringService();
            _memberService = memberService ?? new MemberService();
        }

        private static Dictionary<string, Type> InternalActionDictionary
        {
            get
            {
                var actionDictionary = new Dictionary<string, Type>();
                foreach (var type in Assembly.GetAssembly(typeof(LogAction)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(LogAction))))
                {
                    var logAction = (LogAction)Activator.CreateInstance(type);
                    actionDictionary.Add(logAction.ActionName, type);
                }

                return actionDictionary;
            }
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
        public ActionResult Password()
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
            var actions = new List<SyncMonitoringAction>();
            var values = Enum.GetValues(typeof(LogActionId));
            foreach (var value in values)
            {
                actions.Add(new SyncMonitoringAction
                {
                    ActionId = (int)value,
                    ApiMethod = Enum.GetName(typeof(LogActionId), value)
                });
            }

            var viewModel = new SyncMonitoringViewModel
            {
                Actions = actions,
                Urls = new List<string> { AppSettings.TaiwanAPIUrl.ToString() }
            };
            return View(viewModel);
        }

        /// <summary>
        /// Monitoring Data's synchronization.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpPost]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Verified")]
        public JsonResult SyncMonitoring(SyncMonitoringPostViewModel model)
        {
            var criteria = new SyncMonitoringCriteria
            {
                ActionId = model.Action ?? 0,
                TaiwanAPIUrl = model.Url,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };
            var list = _monitoringService.GetFailedAPICalls(criteria);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var types = ActionDictionary.Where(x => string.Compare(x.Key, item.ActionName, StringComparison.OrdinalIgnoreCase) == 0).Select(x => x.Value);
                    if (types.Any())
                    {
                        var type = types.First();
                        var logAction = (LogAction)ObjectSerializer.Deserialize(item.Command, type);
                        if (logAction != null)
                        {
                            switch (item.ActionId)
                            {
                                case (int)LogActionId.UpdateMemberStatus:
                                    var updateMemberAction = (UpdateMemberStatusAction)logAction;
                                    updateMemberAction.IBCCustId = item.CustId.ToString();
                                    break;
                            }

                            item.InputParams = logAction.GenerateInputs();
                        }
                    }
                }
            }

            return Json(new
            {
                List = list,
                TotalRows = criteria.TotalRows
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
            var types = ActionDictionary.Where(x => string.Compare(x.Key, logData.ActionName, StringComparison.OrdinalIgnoreCase) == 0).Select(x => x.Value);
            if (types.Any())
            {
                var type = types.First();
                var logAction = (LogAction)ObjectSerializer.Deserialize(logData.Command, type);
                logAction.LicenseeId = CurrentSession.OnLicenseeId;
                logAction.UserId = CurrentSession.LoginUserId;
                switch (logData.ActionId)
                {
                    case (int)LogActionId.ReSetPassword:
                        responseResult = _memberService.ResetPassword((ResetPasswordAction)logAction, true, logData.KeyId);
                        break;

                    case (int)LogActionId.UpdateMemberInfo:
                        responseResult = _memberService.UpdateMemberInfo((UpdateMemberInfoAction)logAction, true, logData.KeyId);
                        break;

                    case (int)LogActionId.InsertNewMember:
                        responseResult = _memberService.InsertNewMember((InsertNewMemberAction)logAction, true, logData.KeyId);
                        break;

                    case (int)LogActionId.UpdateMemberStatus:
                        responseResult = _memberService.UpdateMemberStatus((UpdateMemberStatusAction)logAction, true, logData.KeyId);
                        break;

                    case (int)LogActionId.UpdateAgentID:
                        responseResult = _memberService.UpdateAgentId((UpdateAgentIdAction)logAction, true, logData.KeyId);
                        break;
                }
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