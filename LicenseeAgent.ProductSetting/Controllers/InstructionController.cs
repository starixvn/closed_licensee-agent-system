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
            string tipTitle = "",
            string product = "")
        {
            ViewBag.Trigger = trigger;
            ViewBag.Title = title;
            ViewBag.TipTitle = tipTitle;

            string viewName = string.Format("~/Areas/ProductSetting/Views/Instruction/{0}/{1}/{2}.cshtml", CurrentSession.Language, product, key);

            return View(viewName);
        }
    }
}