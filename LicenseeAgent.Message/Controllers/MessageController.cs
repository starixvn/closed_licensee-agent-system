// -----------------------------------------------------------------------
// <copyright file="MessageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LicenseeAgent.Message.Controllers
{
    using System.Web.Mvc;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// All action for the message.
    /// </summary>
    public class MessageController : BaseController
    {
        /// <summary>
        /// Index page of Message.
        /// </summary>
        /// <param name="model">Message view model.</param>
        /// <returns>Return a view.</returns>
        [HttpGet]
        public ActionResult Index(MessageViewModel model)
        {
            return View(model);
        }
    }
}