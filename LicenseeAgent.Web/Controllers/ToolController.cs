// ----------------------------------------------------------------------
// <copyright file="ToolController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.Common;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Web.Filters;

    [AdminSite]
    [AllowAnonymous]
    public class ToolController : Controller
    {
        [HttpGet]
        public static void TestMSMQ()
        {
            Logger.Error("This is a test");
        }

        /// <summary>
        /// Tool for check password.
        /// </summary>
        /// <returns>View result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Password encode.
        /// </summary>
        /// <param name="password">Password input.</param>
        /// <returns>Password encoded.</returns>
        [HttpPost]
        public JsonResult PasswordEncode(string password)
        {
            var encodedPassword = Cryptography.CfsEnCrypt(password);

            return Json(encodedPassword);
        }

        /// <summary>
        /// Decode password.
        /// </summary>
        /// <param name="encodedpassword">Encoded password.</param>
        /// <returns>Plain password.</returns>
        [HttpPost]
        public JsonResult PasswordDecode(string encodedpassword)
        {
            var password = Cryptography.CfsDeCrypt(encodedpassword);

            return Json(password);
        }

        [HttpGet]
        public ActionResult WhoAmI()
        {
            return View();
        }
    }
}