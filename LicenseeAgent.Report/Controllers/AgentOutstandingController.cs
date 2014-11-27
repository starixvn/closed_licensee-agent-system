namespace LicenseeAgent.Report.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Report.Criteria.AgentOutstanding;
    using LicenseeAgent.Report.Entities.AgentOutstanding;
    using LicenseeAgent.Report.Services;
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

        #endregion Variables

        #region Constructors

        public AgentOutstandingController()
            : this(null, null)
        {
        }

        public AgentOutstandingController(
            IDownLinePermissionService downLinePermissionService,
            IAgentOutstandingService agentOutstandingService)
        {
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _agentOutstandingService = agentOutstandingService ?? new AgentOutstandingService();
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
    }
}