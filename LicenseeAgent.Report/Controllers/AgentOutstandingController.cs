namespace LicenseeAgent.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.BetList.Entities;
    using LicenseeAgent.BetList.Services;
    using LicenseeAgent.Report.Criteria.AgentOutstanding;
    using LicenseeAgent.Report.Entities.AgentOutstanding;
    using LicenseeAgent.Report.Services;
    using LicenseeAgent.Report.Utilities;
    using LicenseeAgent.Report.ViewModels.AgentOutstanding;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// AgentOutstandingController class.
    /// </summary>
    public class AgentOutstandingController : BaseController
    {
        #region Variables

        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly IAgentOutstandingService _agentOutstandingService;
        private readonly IBetListService _betlistService;

        #endregion Variables

        #region Constructors

        public AgentOutstandingController()
            : this(null, null, null)
        {
        }

        public AgentOutstandingController(
            IDownLinePermissionService downLinePermissionService,
            IAgentOutstandingService agentOutstandingService,
            IBetListService betlistService)
        {
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _agentOutstandingService = agentOutstandingService ?? new AgentOutstandingService();
            _betlistService = betlistService ?? new BetListService();
        }

        #endregion Constructors

        #region Index

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <returns>
        /// Return a JSON object.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new AgentOutstandingViewModel
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                ShowMembers = CurrentSession.AgentInfo.IsLastLevel
            };

            return View(model);
        }

        /// <summary>
        /// Outstanding Report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a JSON object.
        /// </returns>
        [HttpPost]
        [ActionName("Index")]
        public JsonResult IndexPost(AgentOutstandingViewModel model)
        {
            model.AgentId = model.AgentId == 0 ? CurrentSession.AgentInfo.AgentId : model.AgentId;
            if (!_downLinePermissionService.IsDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedJson();
            }

            var list = new List<AgentOutstandingEntity>();
            if (!model.ShowMembers)
            {
                list.AddRange(_agentOutstandingService.GetAgentList(new AgentOutstandingReportCriteria
                {
                    AgentId = model.AgentId
                }));

                var memberSum = _agentOutstandingService.GetMemberSum(new MemberOutstandingReportCriteria
                {
                    AgentId = model.AgentId
                });

                if (memberSum != null)
                {
                    list.Add(memberSum);
                }
            }
            else
            {
                list.AddRange(_agentOutstandingService.GetMemberList(new MemberOutstandingReportCriteria
                {
                    AgentId = model.AgentId
                }));
            }

            decimal sumOutstanding = list.Sum(i => i.Outstanding);
            decimal sumPositiontaking = list.Sum(i => i.Positiontaking);
            return Json(new
            {
                List = list,
                SumOutstanding = sumOutstanding,
                SumPositiontaking = sumPositiontaking
            });
        }

        #endregion Index

        #region BetList

        /// <summary>
        /// Outstanding - Bet List Report.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Return a JSON object.
        /// </returns>
        /// Bet list result.
        [HttpPost]
        public JsonResult BetList(OutstandingReportBetListViewModel model)
        {
            if (!_downLinePermissionService.IsDownLineMember(model.CustId))
            {
                return RedirectToAccessDeniedJson();
            }

            var criteria = new OutstandingReportBetListCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                CustId = model.CustId
            };

            var ticketResult = _betlistService.GetBetList(criteria);
            var ticketBuilderParameters = new TicketBuilderParameters
            {
                TicketResult = ticketResult,
                IsAdmin = CurrentSession.IsAdmin,
                IsViewPlayerCommission = CurrentSession.IsViewPlayerCommission()
            };

            var betListTicketBuilder = new OutstandingBetListTicketBuilder(ticketBuilderParameters);
            var ticketTextList = betListTicketBuilder.BuildTickets();
            var subTotalWinLost = ticketResult.MainTicketList.Sum(x => x.Winlost);
            var subTotalComm = ticketResult.MainTicketList.Sum(x => x.PlayerComm);
            return Json(new
            {
                List = ticketTextList,
                LegendType = CurrentSession.AgentInfo.LegendType,
                TotalItem = ticketTextList.Count(),
                SubTotalWinLost = subTotalWinLost,
                SubTotalComm = subTotalComm,
                TotalWinLost = subTotalWinLost + subTotalComm
            });
        }

        #endregion BetList
    }
}