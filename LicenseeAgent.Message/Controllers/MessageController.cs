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
    using Criteria;
    using Services;
    using ViewModels;
    using Web.Controllers;

    /// <summary>
    /// All action for the message.
    /// </summary>
    public class MessageController : BaseController
    {
        #region Services

        private readonly IMessageService _messageService;

        #endregion Services

        #region Constructor

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
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
            ////var criteria = new SearchMessageCriteria
            ////{
            ////    FromDate = model.FromDate,
            ////    ToDate = model.ToDate
            ////};
            ////var message = _messageService.GetMessageList(criteria);
            ////return Json(new
            ////{
            ////    List = message
            ////});
            return View(model);
        }
    }
}