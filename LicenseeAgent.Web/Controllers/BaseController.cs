namespace LicenseeAgent.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// This class override OnActionExecuting to check authentication, authorization, duplicated login
    /// All non-anonymous controller MUST inherit from this base controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected static ICurrentSession CurrentSession
        {
            get
            {
                return SessionManager.CurrentSession;
            }
        }

        protected override bool DisableAsyncSupport
        {
            get
            {
                return true;
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
                var language = ModelSettings.DefaultLanguage;

                // current session is only existed when user is authenticated
                if (CurrentSession != null)
                {
                    language = CurrentSession.Language;
                }
                else
                {
                    // not authenticated
                    var languageCookie = Request.Cookies[Constants.AppCookieLanguage];
                    if (languageCookie != null)
                    {
                        var cookieValue = languageCookie.Value;
                        if (ModelSettings.Languages.Where(x => x.Value == cookieValue).Any())
                        {
                            language = cookieValue;
                        }
                    }
                }

                return language;
            }

            set
            {
                var language = value;
                var selectedLanguage = ModelSettings.Languages.Where(x => x.Value == language).FirstOrDefault();
                language = selectedLanguage != null ? selectedLanguage.Value : ModelSettings.DefaultLanguage;
                if (!string.IsNullOrEmpty(language))
                {
                    if (CurrentSession != null)
                    {
                        // authenticated
                        CurrentSession.Language = language;
                    }

                    var languageCookie = new HttpCookie(Constants.AppCookieLanguage, language);
                    languageCookie.Expires = DateTime.Now.AddDays(30);
                    Response.SetCookie(languageCookie);
                }
            }
        }

        protected static string GetErrMsg(int errCode)
        {
            return ResourceManager.GetErrMsg(errCode);
        }

        /// <summary>
        /// Renders the razor view to string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns>Returns string from view.</returns>
        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
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
                if (ControllerContext.IsJsonRequest())
                {
                    var jsonResult = new Result(Result.Unauthorized, Las.UnauthorizedMsg);
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

        protected override void ExecuteCore()
        {
            LanguageManager.Language = Language;
            base.ExecuteCore();
        }

        /// <summary>
        /// Override base action executing function to set culture for every request.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HandleRequest(filterContext);
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
                    var isAjaxRequest = filterContext.IsJsonRequest();
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
            var result = new Result(Result.FailureValue, errMsg);
            return JsonResult(result);
        }

        protected JsonResult JsonResult(int errCode, string errMsg)
        {
            var result = new Result(errCode, errMsg);
            return JsonResult(result);
        }

        protected JsonResult InvalidModelJsonResult()
        {
            var result = new Result(Result.InvalidModel, GetModelErrorMessage());
            return JsonResult(result);
        }

        protected ActionResult InvalidModelViewResult()
        {
            string errorMessage = GetModelErrorMessage();
            return Content(string.Format("<div class='alert alert-error' style='display: block;'>{0}</div>", errorMessage));
        }

        protected JsonResult RedirectToAccessDeniedJson()
        {
            return JsonResult(Las.AccessDeniedMsg);
        }

        protected ActionResult RedirectToAccessDeniedPage()
        {
            return Redirect(AppURLs.URLs.AccessDenied);
        }

        protected JavaScriptResult RedirectToAccessDeniedJavascript()
        {
            string script = string.Format("alert('{0}'", Las.AccessDeniedMsg);
            return JavaScript(script);
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