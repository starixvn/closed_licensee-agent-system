namespace LicenseeAgent.Transfer.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Transfer.Criteria;
    using LicenseeAgent.Transfer.Entities;
    using LicenseeAgent.Transfer.Services;
    using LicenseeAgent.Transfer.ViewModels;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// TransferController class.
    /// </summary>
    [LegendType(LegendType.Credit)]
    public class TransferController : BaseController
    {
        #region Variables

        private readonly ITransferService _transferService;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;

        #endregion Variables

        #region Constructors

        public TransferController()
            : this(null, null, null)
        {
        }

        public TransferController(
            ITransferService transferService,
            IMemberService memberService,
            IDownLinePermissionService downLinePermissionService)
        {
            _transferService = transferService ?? new TransferService();
            _memberService = memberService ?? new MemberService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        #endregion Constructors

        #region Index

        [HttpGet]
        public ActionResult Index()
        {
            var model = new TransferViewModel();
            return View(model);
        }

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns list of customer balance.</returns>
        [HttpPost]
        public JsonResult Index(TransferViewModel model)
        {
            if (CurrentSession.AgentInfo.IsLastLevel)
            {
                return SearchMember(model);
            }
            else
            {
                return SearchAgent(model);
            }
        }

        /// <summary>
        /// Searches the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchAgent(TransferViewModel model)
        {
            var list = _transferService.GetAgentBalanceInfo(new GetAgentBalanceInfoCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                Username = model.UserName,
                Status = model.Status
            });

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = list
            });
        }

        /// <summary>
        /// Searches the member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON balance.</returns>
        [NonAction]
        public JsonResult SearchMember(TransferViewModel model)
        {
            var list = _transferService.GetMemberBalanceInfo(new GetMemberBalanceInfoCriteria
            {
                AgentId = CurrentSession.AgentInfo.AgentId,
                Username = model.UserName
            });

            var response = _memberService.GetMemberInfoList<MemberBalanceInfo>(
                list,
                new GetMemberInfoListAction
                {
                    LASAgentId = CurrentSession.AgentInfo.AgentId,
                    SiteName = CurrentSession.AgentInfo.LicenseeName
                });

            if (response.ErrCode != Result.SuccessValue)
            {
                return Json(response);
            }

            list = list.Where(x => x.UserName.Contains(model.UserName, StringComparison.OrdinalIgnoreCase) &&
                (model.Status == null || x.Status == model.Status)).ToList();
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                List = list
            });
        }

        #endregion Index

        #region Transfer to agent

        /// <summary>
        /// Agents the specified agent identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns agent transfer page.
        /// </returns>
        [HttpGet]
        public ActionResult TransferAgent(TransferAgentViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineAgent(model.AgentId))
            {
                return RedirectToAccessDeniedPage();
            }

            var agentTransfer = _transferService.GetTransferAgentDetail(
                new GetTransferAgentDetailCriteria
                {
                    AgentId = model.AgentId
                });

            model.AgentName = agentTransfer.Username;
            SetModelAgentAmount(model, agentTransfer.YesterdayBalance);
            return View(model);
        }

        /// <summary>
        /// Agents the post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns View.</returns>
        [HttpPost, ActionName("TransferAgent")]
        [ValidateAntiForgeryToken]
        public ActionResult TransferAgentPost(TransferAgentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_downLinePermissionService.IsDirectDownLineAgent(model.AgentId))
                {
                    return RedirectToAccessDeniedPage();
                }

                var criteria = new TransferToAgentCriteria
                {
                    Updater = CurrentSession.LoginUserId,
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    ToAgentId = model.AgentId,
                    IP = Request.GetClientIp(),
                    Amount = model.Amount
                };
                _transferService.TransferToAgent(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.TransferSuccessfully;
                    SetModelAgentAmount(model, criteria.YesterdayBalance);
                }
            }

            return View(model);
        }

        [NonAction]
        public void SetModelAgentAmount(TransferAgentViewModel model, decimal amount)
        {
            // Transfer Amount must be opposite Yesterday Balance.
            model.Amount = -amount;
            model.MinAmount = model.Amount > 0 ? 0 : model.Amount;
            model.MaxAmount = model.Amount > 0 ? model.Amount : 0;
        }

        #endregion Transfer to agent

        #region Transfer to member

        /// <summary>
        /// Members the specified member identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns member transfer page.
        /// </returns>
        [HttpGet]
        public ActionResult TransferMember(TransferMemberViewModel model)
        {
            if (!_downLinePermissionService.IsDirectDownLineMember(model.MemberId))
            {
                return RedirectToAccessDeniedPage();
            }

            var criteria = new GetMemberDetailCriteria
            {
                CustId = model.MemberId
            };

            var memberDetail = _transferService.GetMemberDetail(criteria);
            model.UserName = memberDetail.UserName;
            model.BetCredit = memberDetail.BetCredit;
            model.LastLogin = memberDetail.LastLogin;
            model.LastTransaction = memberDetail.LastTransaction;
            SetModelMemberAmount(model, memberDetail.YesterdayBalance);

            return View(model);
        }

        /// <summary>
        /// Members the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns View.</returns>
        [HttpPost, ActionName("TransferMember")]
        [ValidateAntiForgeryToken]
        public ActionResult TransferMemberPost(TransferMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_downLinePermissionService.IsDirectDownLineMember(model.MemberId))
                {
                    return RedirectToAccessDeniedPage();
                }

                var criteria = new TransferToMemberCriteria
                {
                    Updater = CurrentSession.LoginUserId,
                    AgentId = CurrentSession.AgentInfo.AgentId,
                    ToCustId = model.MemberId,
                    IP = Request.GetClientIp(),
                    Amount = model.Amount
                };
                _transferService.TransferToMember(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.TransferSuccessfully;
                    SetModelMemberAmount(model, criteria.YesterdayBalance);
                }
            }

            return View(model);
        }

        [NonAction]
        public void SetModelMemberAmount(TransferMemberViewModel model, decimal amount)
        {
            // Transfer Amount must be opposite Yesterday Balance.
            model.Amount = -amount;
            model.MinAmount = model.Amount > 0 ? 0 : model.Amount;
            model.MaxAmount = model.Amount > 0 ? model.Amount : 0;
        }

        #endregion Transfer to member
    }
}