namespace LicenseeAgent.CustomerList.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerList.Criteria.PendingMember;
    using LicenseeAgent.CustomerList.Services;
    using LicenseeAgent.CustomerList.ViewModels.PendingMember;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// PendingMemberListController class.
    /// </summary>
    public class PendingMemberController : BaseController
    {
        #region Variables

        private readonly IPendingMemberListService _pendingMemberService;
        private readonly IMemberService _memberService;

        #endregion Variables

        #region Constructors

        public PendingMemberController()
            : this(null, null)
        {
        }

        public PendingMemberController(
            IPendingMemberListService customerService,
            IMemberService memberService)
        {
            _pendingMemberService = customerService ?? new PendingMemberListService();
            _memberService = memberService ?? new MemberService();
        }

        #endregion Constructors

        #region Index

        [HttpGet]
        [LegendType(LegendType.Deposit)]
        public ViewResult Index()
        {
            var model = new PendingMemberViewModel
            {
                DirectMember = new DirectMemberViewModel(),
                IndirectMember = new IndirectMemberViewModel()
            };

            ViewBag.PageContext = new PageContext(CurrentSession);
            return View(model);
        }

        /// <summary>
        /// Get pending member list from Indirect agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns result.</returns>
        [HttpPost]
        [LegendType(LegendType.Deposit)]
        public JsonResult Index(IndirectMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonResult(ErrCode.Msg_2110);
            }

            var agentId = _pendingMemberService.GetAgentIdByUserName(new GetAgentIdByUsernameCriteria
            {
                UserName = model.AgentUserName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId
            });
            if (agentId == 0)
            {
                return JsonResult(ErrCode.Msg_2002);
            }

            var result = _memberService.GetDepositMemberInfoList(new GetDepositMemberInfoListAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                LASAgentId = agentId,
                DepositOption = (int)DepositOption.DepositNot,
                Username = model.IndirectMemberUserName
            });

            return Json(result);
        }

        #endregion Index

        #region Direct member

        /// <summary>
        /// Get pending member list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns result.</returns>
        [HttpPost]
        [LegendType(LegendType.Deposit)]
        public JsonResult DirectMember(DirectMemberViewModel model)
        {
            var result = _memberService.GetDepositMemberInfoList(new GetDepositMemberInfoListAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                LASAgentId = CurrentSession.AgentInfo.AgentId,
                DepositOption = (int)DepositOption.DepositNot,
                Username = model.DirectMemberUserName
            });

            return Json(result);
        }

        #endregion Direct member
    }
}