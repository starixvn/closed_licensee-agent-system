namespace LicenseeAgent.ProductSetting.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.ViewModels;

    /// <summary>
    /// KenoController class.
    /// </summary>
    public class KenoController : ThirdProductController
    {
        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        [HttpGet]
        public ActionResult EditAgent(EditAgentViewModel model)
        {
            return EditAgent(model, ProductCode.Product_8);
        }

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        [HttpGet]
        public ActionResult EditMember(EditMemberViewModel model)
        {
            return EditMember(model, ProductCode.Product_8);
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        public JsonResult UpdateCustomer(UpdateCustomerViewModel model)
        {
            if (!ModelState.IsValid || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            if (!CheckPermissionForEditing(model.SettingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var results = KenoService.UpdateCustomer(model);

            var messages = new List<Result>();
            foreach (var result in results)
            {
                messages.Add(GenerateMessages(result));
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Messages = messages
            });
        }
    }
}
