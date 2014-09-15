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
    public class VirtualSportsController : ProductBaseController
    {
        #region Constructors

        public VirtualSportsController()
            : base(null, null, null, null, null, null, null)
        {
        }

        #endregion Constructors

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
            var agentInfo = CoreService.GetAgentInfo(model.AgentId);
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (CurrentSession.AgentInfo.IsLevel0 || agentInfo.AgentLevel - CurrentSession.AgentInfo.AgentLevel == 1)
                    ? FormType.Edit
                    : FormType.ViewIndirectAgent,
                TargetCustId = model.AgentId,
                TargetRoleId = ModelHelper.IsAgentLevel1(agentInfo.AgentLevel) ? (int)Role.AgentLevel1 : (int)Role.Agent,
                Ip = Request.GetClientIp()
            };

            var productInfos = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            if (!productInfos.Where(x => x.ProductId == (int)Product.Product_5).Any() || !CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("EditCustomer", settingContext);
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
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = FormType.Edit,
                TargetCustId = model.MemberId,
                TargetRoleId = (int)Role.Member,
                Ip = Request.GetClientIp()
            };

            if (CurrentSession.AgentInfo.IsLevel0 && (model.AgentId == null || model.AgentLevel == null))
            {
                settingContext.FormId = FormType.Edit;
            }
            else
            {
                var agentId = model.AgentId ?? CurrentSession.AgentInfo.AgentId;
                var agentLevel = model.AgentLevel ?? CurrentSession.AgentInfo.AgentLevel;
                if (!CurrentSession.AgentInfo.IsLastAgent(agentLevel))
                {
                    settingContext.FormId = FormType.EditIndirectMember;
                    settingContext.CustId = agentId;
                }
            }

            var productInfos = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            if (!productInfos.Where(x => x.ProductId == (int)Product.Product_5).Any() || !CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }
            
            return View("EditCustomer", settingContext);
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