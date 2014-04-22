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
    using Fanex.Common;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Web.Filters;
    using Services;

    /// <summary>
    /// Tool controller.
    /// </summary>
    [AdminSite]
    [AllowAnonymous]
    public class ToolController : Controller
    {
        private readonly IMonitoringService _monitoringService;

        public ToolController(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
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
                switch (item.ActionID)
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
                        updateMemberAction.IBCCustId = item.CustID.ToString();
                        inputParamsHtmlString = GenerateInputParamsHtmlString(updateMemberAction);
                        break;
                }

                item.Command = inputParamsHtmlString;
            }
            
            return Json(new
            {
                List = failedAPICalls,
                TotalItems = failedAPICalls.Count
            });
        }

        /// <summary>
        /// Generate Input Parameters Html String.
        /// </summary>
        /// <param name="logActionObject">Log action object.</param>
        /// <returns>Html string.</returns>
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