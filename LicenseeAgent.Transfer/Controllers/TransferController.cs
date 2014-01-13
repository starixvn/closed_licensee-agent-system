// --------------------------------------------------------------------------
// <copyright file="TransferController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Transfer.Services;
    using LicenseeAgent.Transfer.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferController : BaseController
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Search(TransferSearchViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
        }

        [HttpGet]
        public ViewResult Agent(int agentId)
        {
            var model = new TransferAgentViewModel
            {
                AgentId = agentId
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Agent(TransferAgentViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
        }

        [HttpGet]
        public ViewResult Member(int memberId)
        {
            var model = new TransferMemberViewModel
            {
                MemberId = memberId
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Member(TransferMemberViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
        }
    }
}