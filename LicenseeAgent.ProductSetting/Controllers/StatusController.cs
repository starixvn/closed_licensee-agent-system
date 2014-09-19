namespace LicenseeAgent.ProductSetting.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.ProductSetting.Criteria.Status;
    using LicenseeAgent.ProductSetting.Services;
    using LicenseeAgent.ProductSetting.ViewModels.Status;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// StatusController class.
    /// </summary>
    public class StatusController : BaseController
    {
        #region Variables

        private readonly IStatusService _statusService;
        private readonly IDownLinePermissionService _downLinePermissionService;

        #endregion Variables

        #region Constructors

        public StatusController()
            : this(null, null)
        {
        }

        public StatusController(
            IStatusService statusService,
            IDownLinePermissionService downLinePermissionService)
        {
            _statusService = statusService ?? new StatusService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        #endregion Constructors

        #region Update status

        /// <summary>
        /// Updates the agent product setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns result.</returns>
        [HttpPost]
        public JsonResult UpdateAgent(UpdateAgentViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            if (!CurrentSession.IsAdmin && model.ProductId == (int)ProductCode.Product_2)
            {
                return JsonResult(ErrCode.Msg_2142);
            }

            var criteria = new UpdateAgentCriteria
            {
                UpdatedBy = CurrentSession.LoginUserId,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                ProductId = model.ProductId,
                AgentId = model.AgentId,
                Disable = model.Disable,
                IsAllDownLine = model.IsAllDownLine,
                Ip = Request.GetClientIp()
            };

            _statusService.UpdateAgent(criteria);
            return JsonResult(criteria.ErrCode);
        }

        /// <summary>
        /// Updates the member product setting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns result.</returns>
        [HttpPost]
        public JsonResult UpdateMember(UpdateMemberViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineMember(model.CustId))
            {
                return RedirectToAccessDeniedJson();
            }

            if (!CurrentSession.IsAdmin && model.ProductId == (int)ProductCode.Product_2)
            {
                return JsonResult(ErrCode.Msg_2142);
            }

            var criteria = new UpdateMemberCriteria
            {
                UpdatedBy = CurrentSession.LoginUserId,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                ProductId = model.ProductId,
                CustId = model.CustId,
                Disable = model.Disable,
                Ip = Request.GetClientIp()
            };

            _statusService.UpdateMember(criteria);
            return JsonResult(criteria.ErrCode);
        }

        #endregion Update status
    }
}