namespace LicenseeAgent.Web.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.ProductSetting.ViewModels;

    /// <summary>
    /// Show instruction.
    /// </summary>
    [AllowAnonymous]
    public class InstructionController : BaseController
    {
        /// <summary>
        /// Instruction Index.
        /// </summary>
        /// <param name="viewModel">Instruction view model.</param>
        /// <returns>Action result.</returns>
        public ActionResult Index(InstructionViewModel viewModel)
        {
            ViewBag.Trigger = viewModel.Trigger;
            ViewBag.Title = viewModel.Title;
            ViewBag.TipTitle = viewModel.TipTitle;

            string viewName = string.Format(
                "~/Areas/ProductSetting/Views/Instruction/{0}/{1}/{2}.cshtml", 
                CurrentSession.Language, 
                viewModel.Product, 
                viewModel.Key);

            return View(viewName);
        }
    }
}