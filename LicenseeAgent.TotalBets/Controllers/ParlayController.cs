namespace LicenseeAgent.TotalBets.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;

    /// <summary>
    /// ParlayController class.
    /// </summary>
    public class ParlayController : BaseController
    {
        #region Index

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageContext = new PageContext(CurrentSession, 0);
            return View();
        }

        #endregion Index
    }
}