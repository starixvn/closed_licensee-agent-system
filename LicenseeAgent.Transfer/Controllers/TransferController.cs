// --------------------------------------------------------------------------
// <copyright file="TransferController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Transfer.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Transfer.ViewModels;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransferController : BaseController
    {
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

        public ViewResult Agent(int agentId)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Agent(TransferAgentViewModel model)
        {
            return Json(new
            {
                ErrCode = Result.SuccessValue
            });
        }

        public ViewResult Member(int memberId)
        {
            return View();
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