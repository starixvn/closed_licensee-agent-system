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
    /// ThirdProductController class.
    /// </summary>
    public class ThirdProductController : ProductBaseController
    {
        #region Constructors

        public ThirdProductController()
            : base(null, null, null, null, null, null, null, null)
        {
        }

        #endregion Constructors

        #region Update setting

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="productCode">Product code.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        protected ActionResult EditAgent(EditAgentViewModel model, ProductCode productCode)
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

            if (!productInfos.Where(x => x.ProductId == (int)productCode).Any() || !CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("EditCustomer", settingContext);
        }

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="productCode">Product code.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        protected ActionResult EditMember(EditMemberViewModel model, ProductCode productCode)
        {
            var settingContext = GetEditMemberSettingContext(model);
            var productInfos = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            if (!productInfos.Where(x => x.ProductId == (int)productCode).Any() || !CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }
            
            return View("EditCustomer", settingContext);
        }

        #endregion Update setting
    }
}
