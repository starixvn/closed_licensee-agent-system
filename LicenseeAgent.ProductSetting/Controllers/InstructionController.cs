namespace LicenseeAgent.ProductSetting.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.ProductSetting.ViewModels.Instruction;
    using LicenseeAgent.Web.Controllers;

    /// <summary>
    /// Show instruction.
    /// </summary>
    [AllowAnonymous]
    public class InstructionController : BaseController
    {
        /// <summary>
        /// Instruction Index.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Action result.
        /// </returns>
        public ActionResult Index(InstructionViewModel model)
        {
            string viewName = string.Format(
                "~/Areas/ProductSetting/Views/Instruction/{0}/{1}/{2}.cshtml",
                CurrentSession.Language,
                model.Product,
                model.Key);

            return View(viewName, model);
        }
    }
}