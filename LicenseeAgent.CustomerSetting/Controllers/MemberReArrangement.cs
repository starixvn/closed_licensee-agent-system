namespace LicenseeAgent.CustomerSetting.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Enumerations;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Entities;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Member Re-Arrangement controller.
    /// </summary>
    public partial class CustomerSettingController : BaseController
    {
        [AccountType(AccountType.Admin)]
        [HttpGet]
        [LegendType(LegendType.Deposit)]
        public ViewResult MemberArrangement()
        {
            return View(new MemberArrangementViewModel()
            {
                Mode = ArrangementMode.SwitchFromNormal
            });
        }

        /// <summary>
        /// Members the arrangement.
        /// </summary>
        /// <param name="model">The Member Arrangement View model.</param>
        /// <returns>View Result.</returns>
        [AccountType(AccountType.Admin)]
        [HttpPost]
        [LegendType(LegendType.Deposit)]
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Because urgent matter. Will Fix later.")]
        public ViewResult MemberArrangement(MemberArrangementViewModel model)
        {
            var errCode = 0;
            var customer = GetDepositMemberInformation(model.MemberName, ref errCode);
            if (errCode != 0)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(errCode));
                return View(model);
            }

            var isFirstDeposit = IsFirstDepositMember(model.MemberName);
            if (isFirstDeposit && model.Mode == ArrangementMode.SwitchFromNormal)
            {
                var criteria = new SwitchUserValidateCriteria
                {
                    AgentName = model.AgentName,
                    CurrencyId = customer.Currency,
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    MemberName = model.MemberName
                };
                _customerSettingService.SwitchUserValidate(criteria);
                if (criteria.ErrCode != 0)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                    return View(model);
                }
            }

            // Close deposit member before Update (Pending member: CustId = 0).
            if (!customer.IsClosed && customer.CustId > 0 && model.Mode == ArrangementMode.SwitchFromNormal)
            {
                errCode = UpdateCustomerStatus(customer, true);
                if (errCode != 0)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(errCode));
                    return View(model);
                }
            }

            if (isFirstDeposit)
            {
                errCode = UpdateAgentIdDeposited(model);
            }
            else
            {
                if (model.Mode == ArrangementMode.AssignMember)
                {
                    errCode = CheckDepositMemberBelongToAgent(model.OldAgentName, customer);
                }
                else
                {
                    // Can not switch legend member(has Legend Id).
                    errCode = customer.LASAgentId != 0 ? (int)ErrCode.Msg_2076 : 0;
                }

                if (errCode == 0)
                {
                    errCode = UpdateAgentID(model, customer);
                }                
            }

            if (errCode == 0)
            {
                ViewBag.Result = Las.UpdateSuccessfully;
                if (model.Mode == ArrangementMode.SwitchFromNormal && customer.CustId > 0)
                {
                    OpenMember(model, customer);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(errCode));
            }

            return View(model);
        }

        /// <summary>
        /// Updates the agent identifier deposited.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Error Code.</returns>
        private int UpdateAgentIdDeposited(MemberArrangementViewModel model)
        {
            var errCode = 0;
            var oldAgentName = string.Empty;
            var agentInfo = _customerSettingService.GetAgentInfoByMember(new GetAgentInfoByMemberCriteria
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
            if (errCode != 0)
            {
                return errCode;
            }

            errCode = model.Mode != ArrangementMode.AssignMember ? SwitchUserFromNormalToLegend(model) : MemberReArrange(model);
            
            if (errCode != 0 && model.Mode == ArrangementMode.AssignMember)
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
            _customerSettingService.MemberReArrangement(criteria);
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
            _customerSettingService.SwitchUserFromNormalToLegend(criteria);
            return criteria.ErrCode;
        }

        private bool IsFirstDepositMember(string username)
        {
            MemberIsFirstDepositCriteria criteria = new MemberIsFirstDepositCriteria()
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                MemberName = username
            };
            _customerSettingService.MemberIsFirstDeposit(criteria);
            return criteria.HasFirstDeposit;
        }

        /// <summary>
        /// Gets the deposit member information.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="errCode">The error code.</param>
        /// <returns>Deposit Customer Entity.</returns>
        private DepositCustomerEntity GetDepositMemberInformation(string username, ref int errCode)
        {
            var customer = new DepositCustomerEntity();
            var serviceResult = _memberService.GetDepositMemberInformation(
                customer,
                new GetDepositMemberInfoAction
                {
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    UserName = username
                });
            if (serviceResult.ErrCode != Result.SuccessValue || customer.Currency == 0)
            {
                // Member does not exists.
                errCode = (int)ErrCode.Msg_2001;
            }

            return customer;
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
            _customerSettingService.GetLegendAgentId(criteria);

            if (criteria.ErrCode != Result.SuccessValue)
            {
                // Message: "Agent does not exist" => "New Agent does not exist." if Mode is Assign Member.
                return criteria.ErrCode == (int)ErrCode.Msg_2111 && model.Mode == ArrangementMode.AssignMember ? (int)ErrCode.Msg_2051 : criteria.ErrCode;
            }

            var action = new UpdateAgentIDAction(customer.CustId, customer.Currency, model.MemberName, agentName, criteria.IBCAgentId, criteria.LegendAgentId);

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
            var customerInfo = _customerSettingService.GetMemberInfo(new GetMemberInfoCriteria
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
            _customerSettingService.GetLegendAgentIdHasDeposit(criteria);

            if (criteria.ErrCode != Result.SuccessValue)
            {
                // Message: "Agent does not exist" => "New Agent does not exist." if Mode is Assign Member.
                return criteria.ErrCode == (int)ErrCode.Msg_2111 && model.Mode == ArrangementMode.AssignMember ? (int)ErrCode.Msg_2051 : criteria.ErrCode;
            }

            var action = new UpdateAgentIDAction(customerInfo.CustId, customerInfo.Currency, model.MemberName, agentName, criteria.IBCAgentId, criteria.LegendAgentId);

            return UpdateAgentIdDepositDB(action, model.Mode);
        }

        /// <summary>
        /// Updates the agent identifier deposit database.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="mode">The mode of Re-Arrangement.</param>
        /// <returns>Error Code.</returns>
        private int UpdateAgentIdDepositDB(UpdateAgentIDAction action, ArrangementMode mode)
        {
            // Log old agent name if assign member.
            var errCode = 0;
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
            var errCode = 0;

            var criteria = new GetLegendAgentIdCriteria
            {
                AgentName = agentName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                LicenseeCurrencyId = CurrentSession.AgentInfo.CurrencyId,
                MemberCurrencyId = customer.Currency,
                SiteName = CurrentSession.AgentInfo.LicenseeName
            };
            _customerSettingService.GetLegendAgentId(criteria);

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

            // Call API to Update Member Status here.
            var serviceResult = _memberService.UpdateMemberStatus(action);

            if (serviceResult.ErrCode != Result.SuccessValue)
            {
                return (int)ErrCode.Msg_2022;
            }

            return Result.SuccessValue;
        }

        /// <summary>
        /// Call API to Open the member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="customer">The customer.</param>
        private void OpenMember(MemberArrangementViewModel model, DepositCustomerEntity customer)
        {
            var criteria = new GetAgentInfoCriteria
            {
                Username = model.Mode == ArrangementMode.AssignMember ? model.NewAgentName : model.AgentName
            };
            var agent = _customerSettingService.GetAgentInfo(criteria);
            
            if (!customer.IsClosed && !agent.IsClosed)
            {
                // Open member.
                UpdateCustomerStatus(customer, false);
            }
        }
    }
}