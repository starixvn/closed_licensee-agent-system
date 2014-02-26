// --------------------------------------------------------------------------
// <copyright file="InstructionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Show instruction.
    /// </summary>
    [AllowAnonymous]
    public class InstructionController : BaseController
    {
        public ActionResult Index(
            string key,
            string trigger = "",
            string title = "",
            string tipTitle = "")
        {
            ViewBag.Trigger = trigger;
            ViewBag.Title = title;
            ViewBag.TipTitle = tipTitle;

            string viewName = string.Format("~/Views/Instruction/{0}/{1}.cshtml", CurrentSession.Language, key);

            return View(viewName);
        }
    }
}