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
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using Services;

    /// <summary>
    /// Tool controller.
    /// </summary>
    [AdminSite]
    [AllowAnonymous]
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
                var inputParamsHtmlString = string.Empty;
                switch (item.ActionId)
                {
                    case 301:
                        inputParamsHtmlString = GenerateInputParamsHtmlString(ObjectSerializer<ResetPasswordAction>.Deserialize(item.Command));
                        break;
                    case 303:
                        inputParamsHtmlString = GenerateInputParamsHtmlString(ObjectSerializer<UpdateMemberInfoAction>.Deserialize(item.Command));
                        break;
                    case 304:
                        inputParamsHtmlString = GenerateInputParamsHtmlString(ObjectSerializer<InsertNewMemberAction>.Deserialize(item.Command));
                        break;
                    case 305:
                        var updateMemberAction = ObjectSerializer<UpdateMemberStatusAction>.Deserialize(item.Command);
                        updateMemberAction.IBCCustId = item.CustId.ToString();
                        inputParamsHtmlString = GenerateInputParamsHtmlString(updateMemberAction);
                        break;
                }

                item.InputParams = inputParamsHtmlString;
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
        /// <param name="syncData">Synchonization data.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult SingleReInvoke(SyncMonitoring syncData)
        {
            var responseResult = new ResponseResult(0, string.Empty);
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

            if (responseResult.ErrCode == Result.SuccessValue)
            {
                _monitoringService.UpdateLogStatus(new SyncMonitoringUpdateStatusCriteria
                {
                        KeyID = syncData.KeyId,
                        NewStatusID = 2
                });
            }

            return Json(new
            {
                ActionID = syncData.ActionId,
                IsSuccess = responseResult.ErrCode == Result.SuccessValue
            });
        }

        /// <summary>
        /// Generate Input Parameters Html String.
        /// </summary>
        /// <param name="logActionObject">Log action object.</param>
        /// <returns>Html string.</returns>
        [NonAction]
        public string GenerateInputParamsHtmlString(LogAction logActionObject)
        {
            var inputParamsHtmlString = string.Empty;
            var properties = logActionObject.GetType().GetProperties();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(logActionObject, null);

                inputParamsHtmlString += propertyName == "ActionName"
                    ? string.Empty
                    : string.Format("{0}: {1} <br />", propertyName, propertyValue);
            }

            return inputParamsHtmlString;
        }
    }
}