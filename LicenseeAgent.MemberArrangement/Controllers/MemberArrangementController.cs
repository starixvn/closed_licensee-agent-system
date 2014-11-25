namespace LicenseeAgent.MemberArrangement.Controllers
{
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.MemberArrangement.Criteria;
    using LicenseeAgent.MemberArrangement.Enumerations;
    using LicenseeAgent.MemberArrangement.Services;
    using LicenseeAgent.MemberArrangement.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Member Re-Arrangement controller.
    /// </summary>
    [AccountType(AccountType.Admin)]
    [LegendType(LegendType.Deposit)]
    public class MemberArrangementController : BaseController
    {
        #region Variables

        private readonly IMemberService _memberService;
        private readonly IMemberArrangementService _memberArrangementService;

        #endregion Variables

        #region Constructors

        public MemberArrangementController()
            : this(null, null)
        {
        }

        public MemberArrangementController(IMemberService memberService, IMemberArrangementService customerSettingService)
        {
            _memberService = memberService ?? new MemberService();
            _memberArrangementService = customerSettingService ?? new MemberArrangementService();
        }

        #endregion Constructors

        #region Index

        [HttpGet]
        public ViewResult Index()
        {
            var model = new MemberArrangementViewModel();
            return View(model);
        }

        /// <summary>
        /// Members the arrangement.
        /// </summary>
        /// <param name="model">The Member Arrangement View model.</param>
        /// <returns>View Result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MemberArrangementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var errCode = Result.SuccessValue;
            var customer = GetDepositMemberInformation(model.MemberName, ref errCode);
            if (errCode != Result.SuccessValue)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(errCode));
                return View(model);
            }

            var isFirstDeposit = IsFirstDepositMember(model.MemberName);
            return model.Mode == ArrangementMode.SwitchFromNormal
                ? SwitchFromNormal(model, customer, isFirstDeposit)
                : AssignMember(model, customer, isFirstDeposit);
        }

        /// <summary>
        /// Switches from normal.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="isFirstDeposit">If set to <c>true</c> [is first deposit].</param>
        /// <returns>Returns view.</returns>
        [NonAction]
        public ActionResult SwitchFromNormal(MemberArrangementViewModel model, DepositCustomerEntity customer, bool isFirstDeposit)
        {
            var errCode = Result.SuccessValue;
            if (isFirstDeposit)
            {
                errCode = ValidateBeforeSwitching(model, customer);
                if (errCode == Result.SuccessValue)
                {
                    // Close deposit member before Update (Pending member: CustId = 0).
                    if (!customer.IsClosed && customer.CustId > 0)
                    {
                        errCode = UpdateCustomerStatus(customer, true);
                    }
                }

                if (errCode == Result.SuccessValue)
                {
                    errCode = UpdateAgentIdDeposited(model);
                }
            }
            else
            {
                // Can not switch legend member(has Legend Id).
                errCode = customer.LASAgentId != 0 ? (int)ErrCode.Msg_2076 : Result.SuccessValue;
                if (errCode == Result.SuccessValue)
                {
                    errCode = UpdateAgentID(model, customer);
                }
            }

            if (errCode == Result.SuccessValue)
            {
                ViewBag.Result = Las.UpdateSuccessfully;
                if (customer.CustId > 0)
                {
                    OpenCustomerStatus(model.AgentName, customer);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(errCode));
            }

            return View(model);
        }

        /// <summary>
        /// Assigns the member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="customer">The customer.</param>
        /// <param name="isFirstDeposit">If set to <c>true</c> [is first deposit].</param>
        /// <returns>Returns view.</returns>
        [NonAction]
        public ActionResult AssignMember(MemberArrangementViewModel model, DepositCustomerEntity customer, bool isFirstDeposit)
        {
            var errCode = Result.SuccessValue;
            if (isFirstDeposit)
            {
                errCode = UpdateAgentIdDeposited(model);
            }
            else
            {
                errCode = CheckDepositMemberBelongToAgent(model.OldAgentName, customer);
                if (errCode == Result.SuccessValue)
                {
                    errCode = UpdateAgentID(model, customer);
                }
            }

            if (errCode == Result.SuccessValue)
            {
                ViewBag.Result = Las.UpdateSuccessfully;
            }
            else
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(errCode));
            }

            return View(model);
        }

        #endregion Index

        #region Utility

        /// <summary>
        /// Validates the switching member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Returns result.</returns>
        private int ValidateBeforeSwitching(MemberArrangementViewModel model, DepositCustomerEntity customer)
        {
            // validate before switching
            var criteria = new SwitchUserValidateCriteria
            {
                AgentName = model.AgentName,
                CurrencyId = customer.Currency,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                MemberName = model.MemberName
            };
            _memberArrangementService.SwitchUserValidate(criteria);
            return criteria.ErrCode;
        }

        /// <summary>
        /// Updates the agent identifier deposited.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Error Code.</returns>
        private int UpdateAgentIdDeposited(MemberArrangementViewModel model)
        {
            var errCode = Result.SuccessValue;
            var oldAgentName = string.Empty;
            var agentInfo = _memberArrangementService.GetAgentInfoByMember(new GetAgentInfoByMemberCriteria
            {
                MemberName = model.MemberName,
                LiceneeId = CurrentSession.AgentInfo.LicenseeId
            });

            if (agentInfo == null)
            {
                // Member does not exist. Please check again.
                if (model.Mode == ArrangementMode.AssignMember)
                {
                    return (int)ErrCode.Msg_2048;
                }
            }
            else
            {
                // Member has been assigned. Please check again.
                if (model.Mode == ArrangementMode.SwitchFromNormal)
                {
                    return (int)ErrCode.Msg_2074;
                }

                oldAgentName = agentInfo.UserName;
            }

            var agentName = model.Mode == ArrangementMode.AssignMember ? model.NewAgentName : model.AgentName;

            // Get Agent Info and Call API update Data.
            errCode = UpdateAgentIDHasDeposit(model, agentName);
            if (errCode != Result.SuccessValue)
            {
                return errCode;
            }

            errCode = model.Mode != ArrangementMode.AssignMember ? SwitchUserFromNormalToLegend(model) : MemberReArrange(model);
            if (errCode != Result.SuccessValue && model.Mode == ArrangementMode.AssignMember)
            {
                // Restore Data of API for case "Assign member".
                UpdateAgentIDHasDeposit(model, oldAgentName);
            }

            return errCode;
        }

        /// <summary>
        /// Arrangement member.
        /// </summary>
        /// <param name="model">The Member Arrangement View model.</param>
        /// <returns>Error Code.</returns>
        private int MemberReArrange(MemberArrangementViewModel model)
        {
            var criteria = new ReArrangementMemberCriteria()
            {
                MemberName = model.MemberName,
                OldAgentName = model.OldAgentName,
                NewAgentName = model.NewAgentName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UpdatedBy = CurrentSession.LoginUserId,
                Ip = Request.GetClientIp()
            };
            _memberArrangementService.MemberReArrangement(criteria);
            return criteria.ErrCode;
        }

        /// <summary>
        /// Switch User From Normal To Legend.
        /// </summary>
        /// <param name="model">The Member Arrangement View model.</param>
        /// <returns>Error Code.</returns>
        private int SwitchUserFromNormalToLegend(MemberArrangementViewModel model)
        {
            var criteria = new SwitchUserFromNormalToLegendCriteria()
            {
                MemberName = model.MemberName,
                AgentName = model.AgentName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UpdatedBy = CurrentSession.LoginUserId,
                Ip = Request.GetClientIp()
            };
            _memberArrangementService.SwitchUserFromNormalToLegend(criteria);
            return criteria.ErrCode;
        }

        /// <summary>
        /// Updates the agent identifier.
        /// </summary>
        /// <param name="model">The model re-arrangement.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Error Code.</returns>
        private int UpdateAgentID(MemberArrangementViewModel model, DepositCustomerEntity customer)
        {
            var agentName = model.Mode == ArrangementMode.AssignMember ? model.NewAgentName : model.AgentName;

            // Call SP to get New IBCAgentID and LASAgentID.
            var criteria = new GetLegendAgentIdCriteria
            {
                AgentName = agentName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId,
                MemberCurrencyId = customer.Currency,
                SiteName = CurrentSession.AgentInfo.LicenseeName
            };
            _memberArrangementService.GetLegendAgentId(criteria);

            if (criteria.ErrCode != Result.SuccessValue)
            {
                // Message: "Agent does not exist" => "New Agent does not exist." if Mode is Assign Member.
                return criteria.ErrCode == (int)ErrCode.Msg_2111 && model.Mode == ArrangementMode.AssignMember ? (int)ErrCode.Msg_2051 : criteria.ErrCode;
            }

            var action = new UpdateAgentIdAction(customer.CustId, customer.Currency, model.MemberName, agentName, criteria.IBCAgentId, criteria.LegendAgentId);

            return UpdateAgentIdDepositDB(action, model.Mode);
        }

        /// <summary>
        /// Updates the agent identifier has deposit.
        /// </summary>
        /// <param name="model">The model re-arrangement.</param>
        /// <param name="agentName">Name of the agent.</param>
        /// <returns>Error Code.</returns>
        private int UpdateAgentIDHasDeposit(MemberArrangementViewModel model, string agentName)
        {
            var customerInfo = _memberArrangementService.GetMemberInfo(new GetMemberInfoCriteria
            {
                MemberName = model.MemberName,
                LiceneeId = CurrentSession.AgentInfo.LicenseeId
            });

            // Call SP to get New IBCAgentID and LASAgentID.
            var criteria = new GetLegendAgentIdHasDepositCriteria
            {
                AgentName = agentName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId,
                MemberName = model.MemberName,
                SiteName = CurrentSession.AgentInfo.LicenseeName
            };
            _memberArrangementService.GetLegendAgentIdHasDeposit(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                // Message: "Agent does not exist" => "New Agent does not exist." if Mode is Assign Member.
                return criteria.ErrCode == (int)ErrCode.Msg_2111 && model.Mode == ArrangementMode.AssignMember ? (int)ErrCode.Msg_2051 : criteria.ErrCode;
            }

            var action = new UpdateAgentIdAction(customerInfo.CustId, customerInfo.Currency, model.MemberName, agentName, criteria.IBCAgentId, criteria.LegendAgentId);

            return UpdateAgentIdDepositDB(action, model.Mode);
        }

        /// <summary>
        /// Updates the agent identifier deposit database.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="mode">The mode of Re-Arrangement.</param>
        /// <returns>Error Code.</returns>
        private int UpdateAgentIdDepositDB(UpdateAgentIdAction action, ArrangementMode mode)
        {
            // Log old agent name if assign member.
            var errCode = Result.SuccessValue;
            action.LicenseeId = CurrentSession.AgentInfo.LicenseeId;
            action.UserId = CurrentSession.LoginUserId;
            action.SiteName = CurrentSession.AgentInfo.LicenseeName;
            action.LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId;

            // Call API to Update AgentId here.
            var serviceResult = _memberService.UpdateAgentId(action);

            if (serviceResult.ErrCode != Result.SuccessValue)
            {
                // If Update Error => "Failed to assign member" or "Faild to switch user".
                errCode = mode == ArrangementMode.AssignMember ? (int)ErrCode.Msg_2061 : (int)ErrCode.Msg_2078;
            }

            return errCode;
        }

        /// <summary>
        /// Checks the deposit member belong to agent.
        /// </summary>
        /// <param name="agentName">Name of the agent.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Error Code.</returns>
        private int CheckDepositMemberBelongToAgent(string agentName, DepositCustomerEntity customer)
        {
            var errCode = Result.SuccessValue;
            var criteria = new GetLegendAgentIdCriteria
            {
                AgentName = agentName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId,
                MemberCurrencyId = customer.Currency,
                SiteName = CurrentSession.AgentInfo.LicenseeName
            };
            _memberArrangementService.GetLegendAgentId(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                // Message: "Agent does not exist" => "Old Agent does not exist."
                errCode = criteria.ErrCode == (int)ErrCode.Msg_2111 ? (int)ErrCode.Msg_2050 : criteria.ErrCode;
            }
            else if (criteria.LegendAgentId != customer.LASAgentId)
            {
                // This member is not a downline of Old Agent.
                errCode = (int)ErrCode.Msg_2052;
            }

            return errCode;
        }

        /// <summary>
        /// Gets the deposit member information.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>Deposit Customer Entity.</returns>
        private DepositCustomerEntity GetDepositMemberInformation(string userName, ref int errCode)
        {
            var customer = new DepositCustomerEntity();
            var serviceResult = _memberService.GetDepositMemberInfo(
                customer,
                new GetDepositMemberInfoAction
                {
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    UserName = userName
                });
            if (serviceResult.ErrCode != Result.SuccessValue || customer.Currency == 0)
            {
                // Member does not exists.
                errCode = (int)ErrCode.Msg_2001;
            }

            return customer;
        }

        private bool IsFirstDepositMember(string userName)
        {
            MemberIsFirstDepositCriteria criteria = new MemberIsFirstDepositCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                MemberName = userName
            };
            _memberArrangementService.MemberIsFirstDeposit(criteria);
            return criteria.HasFirstDeposit;
        }

        /// <summary>
        /// Updates the customer status.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="isClosed">Member Status.</param>
        /// <returns>Error Code.</returns>
        private int UpdateCustomerStatus(DepositCustomerEntity customer, bool isClosed)
        {
            var action = new UpdateMemberStatusAction
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                UserId = CurrentSession.LoginUserId,
                IBCCustId = customer.CustId.ToString(),
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                IsClosed = isClosed
            };
            var serviceResult = _memberService.UpdateMemberStatus(action);
            return serviceResult.ErrCode != Result.SuccessValue ? (int)ErrCode.Msg_2022 : Result.SuccessValue;
        }

        /// <summary>
        /// Call API to Open the member.
        /// </summary>
        /// <param name="agentName">Name of the agent.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Returns error code.</returns>
        private int OpenCustomerStatus(string agentName, DepositCustomerEntity customer)
        {
            var criteria = new GetAgentInfoCriteria
            {
                Username = agentName
            };
            var agent = _memberArrangementService.GetAgentInfo(criteria);
            if (!customer.IsClosed && !agent.IsClosed)
            {
                // Open member.
                return UpdateCustomerStatus(customer, false);
            }

            return Result.SuccessValue;
        }

        #endregion Utility
    }
}