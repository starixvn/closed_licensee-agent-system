namespace LicenseeAgent.ProductSetting.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.ViewModels;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// VirtualSportsController class.
    /// </summary>
    public class VirtualSportsController : ThirdProductController
    {
        #region Update setting

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
            return EditAgent(model, ProductCode.Product_5);
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
            return EditMember(model, ProductCode.Product_5);
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        public JsonResult UpdateCustomer(UpdateVirtualSportsCustomerViewModel model)
        {
            if (!ModelState.IsValid || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            if (!CheckPermissionForEditing(model.SettingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var results = VirtualSportsService.UpdateCustomer(model);
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

        #endregion Update setting
    }
}