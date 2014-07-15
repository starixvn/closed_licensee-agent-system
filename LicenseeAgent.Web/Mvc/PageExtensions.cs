namespace LicenseeAgent.Web.Mvc
{
    using System.Net;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// Page extensions.
    /// </summary>
    public static class PageExtensions
    {
        private static readonly ErrorPageInfo NotFound = new ErrorPageInfo(
            HttpStatusCode.NotFound, Result.NotFound, Las.HttpError_404, AppURLs.URLs.NotFound);

        private static readonly ErrorPageInfo Unauthorized = new ErrorPageInfo(
            HttpStatusCode.Forbidden, Result.Unauthorized, Las.UnauthorizedMsg, AppURLs.URLs.Unauthorized);

        private static readonly ErrorPageInfo UnderMaintenance = new ErrorPageInfo(
            HttpStatusCode.Forbidden, Result.UnderMaintenance, Las.UnderMaintenance, AppURLs.URLs.UnderMaintenance);

        private static readonly ErrorPageInfo AccessDenied = new ErrorPageInfo(
            HttpStatusCode.Forbidden, Result.AccessDenied, Las.AccessDeniedMsg, AppURLs.URLs.AccessDenied);

        private static readonly ErrorPageInfo CurrencyPermissionDenied = new ErrorPageInfo(
            HttpStatusCode.Forbidden, Result.NoCurrencyPermission, Las.AlertNoCurrencyPermission, AppURLs.URLs.CurrencyPermissionDenied);

        public static void ReturnCurrencyPermissionDenied(this ControllerContext filterContext)
        {
            Return(filterContext, CurrencyPermissionDenied);
        }

        public static void ReturnUnauthorized(this ActionExecutingContext filterContext)
        {
            Return(filterContext, Unauthorized);
        }

        public static void ReturnUnauthorized(this ControllerContext controllerContext)
        {
            Return(controllerContext, Unauthorized);
        }

        public static void ReturnUnderMaintenance(this ActionExecutingContext filterContext)
        {
            Return(filterContext, UnderMaintenance);
        }

        public static void ReturnAccessDenied(this ActionExecutingContext filterContext)
        {
            Return(filterContext, AccessDenied);
        }

        public static void ReturnAccessDenied(this ControllerContext controllerContext)
        {
            Return(controllerContext, AccessDenied);
        }

        public static void ReturnNotFound(this ActionExecutingContext filterContext)
        {
            Return(filterContext, NotFound);
        }

        /// <summary>
        /// Returns the specified filter context.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <param name="errorPageInfo">The error page information.</param>
        private static void Return(
            this ActionExecutingContext filterContext,
            ErrorPageInfo errorPageInfo)
        {
            var response = filterContext.HttpContext.Response;
            if (response.IsRequestBeingRedirected)
            {
                return;
            }

            response.Clear();
            response.StatusCode = (int)errorPageInfo.StatusCode;
            response.TrySkipIisCustomErrors = true;
            if (filterContext.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new Result(errorPageInfo.ErrCode, errorPageInfo.ErrMsg),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult(errorPageInfo.ErrPageUrl);
            }
        }

        /// <summary>
        /// Returns the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="errorPageInfo">The error page information.</param>
        private static void Return(
            this ControllerContext controllerContext,
            ErrorPageInfo errorPageInfo)
        {
            var response = controllerContext.HttpContext.Response;
            if (response.IsRequestBeingRedirected)
            {
                return;
            }

            response.Clear();
            response.StatusCode = (int)errorPageInfo.StatusCode;
            response.TrySkipIisCustomErrors = true;
            if (controllerContext.IsAjaxRequest())
            {
                response.WriteJson(new Result(errorPageInfo.ErrCode, errorPageInfo.ErrMsg));
                response.End();
            }
            else
            {
                response.Redirect(errorPageInfo.ErrPageUrl);
            }
        }
    }
}