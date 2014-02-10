// ----------------------------------------------------------------------
// <copyright file="ToolController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ToolController : Controller
    {
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
    }
}
