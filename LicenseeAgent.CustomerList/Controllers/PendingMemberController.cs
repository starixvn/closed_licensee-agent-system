namespace LicenseeAgent.Customer.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Customer.Criteria;
    using LicenseeAgent.Customer.Services;
    using LicenseeAgent.Customer.ViewModels.PendingMember;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Enumerations;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// PendingMemberListController class.
    /// </summary>
    public class PendingMemberController : BaseController
    {
        #region Variables

        private readonly ICustomerService _customerService;
        private readonly IMemberService _memberService;

        #endregion Variables

        #region Constructors

        public PendingMemberController()
            : this(null, null)
        {
        }

        public PendingMemberController(
            ICustomerService customerService,
            IMemberService memberService)
        {
            _customerService = customerService ?? new CustomerService();
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
                IsLastLevel = CurrentSession.AgentInfo.IsLastLevel
            };

            return View(model);
        }

        /// <summary>
        /// Get pending member list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns result.</returns>
        [HttpPost]
        [LegendType(LegendType.Deposit)]
        public JsonResult Index(PendingMemberViewModel model)
        {
            var agentId = CurrentSession.AgentInfo.AgentId;
            if (!CurrentSession.AgentInfo.IsLastLevel)
            {
                agentId = _customerService.GetAgentIdByUserName(new GetAgentIdByUsernameCriteria
                {
                    UserName = model.DirectAgentUserName,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId
                });

                if (agentId == 0)
                {
                    return JsonResult(ErrCode.Msg_2002);
                }
            }

            var result = _memberService.GetDepositMemberInfoList(new GetDepositMemberInfoListAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                LASAgentId = agentId,
                DepositOption = (int)DepositOption.DepositNot,
                Username = model.MemberUserName
            });

            return Json(result);
        }

        #endregion Index
    }
}