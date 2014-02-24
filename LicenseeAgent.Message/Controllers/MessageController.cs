// -----------------------------------------------------------------------
// <copyright file="MessageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2014.02.24">Created and Implement function for the message.</change>
// </history>

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
        #region Services

        #endregion Services

        #region Constant

        #endregion Constant

        #region Constructor

        public MessageController()
        {
        }

        #endregion Constructor

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