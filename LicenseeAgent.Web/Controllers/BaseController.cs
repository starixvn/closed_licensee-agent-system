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
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// This class override OnActionExecuting to check authentication, authorization, duplicated login
    /// All non-anonymous controller MUST inherit from this base controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected ICurrentSession CurrentSession
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
            if (filterContext.IsAjaxRequest())
            {
                if (!filterContext.Controller.ViewData.ModelState.IsValid)
                {
                    if (filterContext.ActionDescriptor is ReflectedActionDescriptor)
                    {
                        var actionDescriptor = (ReflectedActionDescriptor)filterContext.ActionDescriptor;
                        if (actionDescriptor.MethodInfo.ReturnType.Equals(typeof(JsonResult)))
                        {
                            filterContext.Result = InvalidModelJsonResult();
                            isHandled = true;
                        }
                    }
                }
            }

            if (!isHandled)
            {
                base.OnActionExecuting(filterContext);
            }
        }

        protected string GetErrorMsg(int msgId)
        {
            return ResourceManager.GetErrorMsg(msgId);
        }

        protected JsonResult JsonResult(Result result)
        {
            return Json(new
            {
                MsgId = result.MsgId,
                MsgContent = result.MsgContent
            });
        }

        protected JsonResult JsonResult(int msgId)
        {
            var result = new Result(msgId);
            return JsonResult(result);
        }

        protected JsonResult InvalidModelJsonResult()
        {
            var result = new Result(GetModelErrorMessage());
            return JsonResult(result);
        }

        protected ActionResult InvalidModelViewResult()
        {
            return Content("Invalid Data");
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