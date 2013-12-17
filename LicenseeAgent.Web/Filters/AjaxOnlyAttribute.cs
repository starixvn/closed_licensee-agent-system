// --------------------------------------------------------------------------
// <copyright file="AjaxOnlyAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.9.30">Created</change>
// </history>

namespace LicenseeAgent.Web.Filters
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsAjaxRequest())
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Only Ajax requests");
            }
        }
    }
}