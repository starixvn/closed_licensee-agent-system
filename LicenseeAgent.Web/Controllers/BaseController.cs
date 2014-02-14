// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders" date="2013.4.6">Add CurrentSession Property</change>
//   <change who="Lewis" date="2013.4.10">Added RequestIP property</change>
//   <change who="Anders.Le" date="2013.5.2">Modify Current Session, change it to readonly</change>
//   <change who="Anders.Le" date="2013.5.2">Check whether request is authenticated</change>
//   <change who="Terry.Nguyen" date="2013.5.28">Handle unknow action</change>
//   <change who="Anders.Le" date="2013.6.30">Add property ActionName, ControllerName, and AreaName</change>
// </history>

namespace LicenseeAgent.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Criterias;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// This class override OnActionExecuting to check authentication, authorization, duplicated login
    /// All non-anonymous controller MUST inherit from this base controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        private readonly IPermissionService _permissionService;

        protected BaseController()
        { 
        }

        protected BaseController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected static ICurrentSession CurrentSession
        {
            get
            {
                return SessionManager.CurrentSession;
            }
        }

        protected string CurrentAction
        {
            get
            {
                return HttpContext.GetActionName();
            }
        }

        protected string CurrentController
        {
            get
            {
                return HttpContext.GetControllerName();
            }
        }

        protected string CurrentArea
        {
            get
            {
                return HttpContext.GetAreaName();
            }
        }

        protected string Language
        {
            get
            {
                var language = string.Empty;

                // current session is only existed when user is authenticated
                if (CurrentSession != null)
                {
                    language = CurrentSession.Language;
                }
                else
                {
                    if (Session[Constants.AppSessionLanguage] != null)
                    {
                        language = (string)Session[Constants.AppSessionLanguage];
                    }
                }

                if (string.IsNullOrEmpty(language))
                {
                    language = ModelSettings.Languages.First().Value;
                }

                return language;
            }

            set
            {
                var language = value;
                var selectedLanguage = ModelSettings.Languages.Where(x => x.Value == language).FirstOrDefault();
                language = selectedLanguage != null ? selectedLanguage.Value : ModelSettings.Languages.First().Value;
                if (!string.IsNullOrEmpty(language))
                {
                    if (CurrentSession != null)
                    {
                        // authenticated
                        CurrentSession.Language = language;
                    }
                    else
                    {
                        // not authenticated
                        Session[Constants.AppSessionLanguage] = language;
                    }
                }
            }
        }

        protected static string GetErrMsg(int errCode)
        {
            return ResourceManager.GetErrMsg(errCode);
        }

        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            var sessionIsTimeout = CurrentSession == null || !CurrentSession.IsValid ? true : false;
            if (sessionIsTimeout)
            {
                if (ControllerContext.IsAjaxRequest())
                {
                    var jsonResult = new Result(Las.UnauthorizedMsg);
                    Json(jsonResult, JsonRequestBehavior.AllowGet).ExecuteResult(ControllerContext);
                }
                else
                {
                    Response.Redirect(AppURLs.URLs.Unauthorized);
                }

                return;
            }

            base.HandleUnknownAction(actionName);
        }

        /// <summary>
        /// Override base action executing function to set culture for every request.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetRequestLanguage();
            HandleRequest(filterContext);
        }

        /// <summary>
        /// Sets the request language.
        /// </summary>
        protected void SetRequestLanguage()
        {
            var language = string.Empty;
            var currentSession = SessionManager.CurrentSession;
            if (currentSession != null)
            {
                language = currentSession.Language;
            }
            else
            {
                if (Session[Constants.AppSessionLanguage] != null)
                {
                    language = (string)Session[Constants.AppSessionLanguage];
                }
            }

            if (!string.IsNullOrEmpty(language))
            {
                ViewBag.Language = language;
                LanguageManager.SetCulture(language);
            }
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected void HandleRequest(ActionExecutingContext filterContext)
        {
            var isHandled = false;
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                if (filterContext.ActionDescriptor is ReflectedActionDescriptor)
                {
                    var actionDescriptor = (ReflectedActionDescriptor)filterContext.ActionDescriptor;
                    var isAjaxRequest = filterContext.IsAjaxRequest();
                    if (isAjaxRequest && actionDescriptor.MethodInfo.ReturnType.Equals(typeof(JsonResult)))
                    {
                        filterContext.Result = InvalidModelJsonResult();
                        isHandled = true;
                    }
                    else if (!isAjaxRequest && actionDescriptor.MethodInfo.ReturnType.Equals(typeof(ViewResult)))
                    {
                        filterContext.Result = InvalidModelViewResult();
                        isHandled = true;
                    }
                }
            }

            if (!isHandled)
            {
                base.OnActionExecuting(filterContext);
            }
        }

        protected JsonResult JsonResult(Result result)
        {
            return Json(new
            {
                ErrCode = result.ErrCode,
                ErrMsg = result.ErrMsg
            });
        }

        protected JsonResult JsonResult(ResponseResult result)
        {
            return Json(new
            {
                ErrCode = result.ErrCode,
                ErrMsg = result.ErrMsg
            });
        }

        protected JsonResult JsonResult(int errCode)
        {
            var result = new Result(errCode);
            return JsonResult(result);
        }

        protected JsonResult JsonResult(string errMsg)
        {
            var result = new Result(errMsg);
            return JsonResult(result);
        }

        protected JsonResult JsonResult(int errCode, string errMsg)
        {
            var result = new Result(errCode, errMsg);
            return JsonResult(result);
        }

        protected JsonResult InvalidModelJsonResult()
        {
            var result = new Result(GetModelErrorMessage());
            return JsonResult(result);
        }

        protected ActionResult InvalidModelViewResult()
        {
            var errorMessage = GetModelErrorMessage();
            return Content(errorMessage);
        }

        /// <summary>
        /// Override JSON for larger max JSON length.
        /// </summary>
        /// <param name="data">Original data.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="contentEncoding">Content Encoding.</param>
        /// <param name="behavior">Behavior value.</param>
        /// <returns>JSON object.</returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        protected ActionResult RedirectToAccessDeniedPage()
        {
            return Redirect(AppURLs.URLs.AccessDenied);
        }

        protected JsonResult RedirectToAccessDeniedJson()
        {
            return Json(new
            {
                ErrCode = Result.FailureValue,
                Messages = Las.AccessDeniedMsg
            });
        }

        /// <summary>
        /// Checks the valid currency.
        /// </summary>
        /// <exception cref="System.Exception">The exception of denied access.</exception>
        protected void CheckValidCurrency()
        {
            if (CurrentSession.AgentInfo != null)
            {
                var clientInfo = Request.GetClientInfo();
                var criteria = new InternalLoginAsUserCriteria
                {
                    AdminId = CurrentSession.LoginUserId,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                    UserName = CurrentSession.AgentInfo.UserName,
                    LoginIp = clientInfo.Ip,
                    BrowserName = clientInfo.BrowserName,
                    Platform = clientInfo.Platform,
                    CountryCode = clientInfo.CountryCode
                };

                if (!_permissionService.IsValidCurrency(criteria))
                {
                    throw new Exception(Las.AccessDeniedMsg);
                }
            }
        }

        /// <summary>
        /// Get Error message.
        /// </summary>
        /// <returns>Error message string.</returns>
        private string GetModelErrorMessage()
        {
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    return error.ErrorMessage;
                }
            }

            return "Undefined Error";
        }
    }
}