// --------------------------------------------------------------------------
// <copyright file="SportsbookSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Account.Criterias;
    using LicenseeAgent.Account.Services;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.CustomerSetting.Criterias;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.Sportsbook;
    using LicenseeAgent.CustomerSetting.Service;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Sport books setting controller.
    /// </summary>
    public class SportsbookSettingController : BaseController
    {
        private static ErrCode[] betSettingErrCodes =
        {
            ErrCode.Msg_1003,
            ErrCode.Msg_1004,
            ErrCode.Msg_1005,
            ErrCode.Msg_1006,
            ErrCode.Msg_1007,
            ErrCode.Msg_1008,
            ErrCode.Msg_1025,
            ErrCode.Msg_1028
        };

        private static ErrCode[] positionTakingErrCodes =
        {
            ErrCode.Msg_1010,
            ErrCode.Msg_1011,
            ErrCode.Msg_1012,
            ErrCode.Msg_1013,
            ErrCode.Msg_1014,
            ErrCode.Msg_1015,
            ErrCode.Msg_1021,
            ErrCode.Msg_1026,
            ErrCode.Msg_1029
        };

        private readonly ISportsbookSettingService _customerSettingService;

        private readonly IAccountService _accountService;

        private readonly IMemberService _memberService;

        public SportsbookSettingController(
                ISportsbookSettingService customerSettingService, 
                IAccountService accountService,
                IMemberService memberService)
        {
            _customerSettingService = customerSettingService;
            _accountService = accountService;
            _memberService = memberService;
        }

        /// <summary>
        /// Get Setting Context For Insert.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="targetCustId">The target customer identifier.</param>
        /// <param name="targetRoleId">The target role identifier.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        public ActionResult GetSettingForInsert(int custId, int roleId, int targetCustId, int targetRoleId)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                CustId = custId,
                RoleId = roleId,
                TargetCustId = targetCustId,
                TargetRoleId = targetRoleId
            };

            var sportsBookCustomer = _customerSettingService.GetSportsbookSettingForInsert(settingContext);
            string javascriptString = _customerSettingService.FormatJavascriptReturn(
                    settingContext,
                    sportsBookCustomer);
            return JavaScript(javascriptString);
        }

        /// <summary>
        /// Get Setting for updating customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JAVASCRIPT string object.
        /// </returns>
        [DownLine]
        public ActionResult GetSettingForUpdate(GetSettingForUpdateViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = model.FormId == (int)FormType.Edit ? FormType.Edit : FormType.ReadOnly,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId,
                Ip = Request.GetClientIp(),
                IsOnlyUpdatePT = CurrentSession.AdminInfo == null
                                && !ModelHelper.IsLastAgent(CurrentSession.AgentInfo.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
                                && model.TargetRoleId == (int)Role.Member
            };

            var sportsBookCustomer = _customerSettingService.GetSportsbookSettingForUpdate(settingContext);
            string javascriptString = _customerSettingService.FormatJavascriptReturn(
                    settingContext,
                    sportsBookCustomer);

            return JavaScript(javascriptString);
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="agentLevel">The agent level.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpGet]
        public ActionResult InsertCustomer(int? agentId, int? agentLevel)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                Ip = Request.GetClientIp()
            };

            if (agentId != null && agentLevel != null)
            {
                settingContext.CustId = agentId.Value;
                settingContext.RoleId = (int)Role.Agent;
                settingContext.TargetRoleId = CurrentSession.AgentInfo.IsLastAgent(agentLevel.Value)
                    ? (int)Role.Member
                    : (int)Role.Agent;
            }

            return View("~/Views/Account/InsertCustomer.cshtml", settingContext);
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        public JsonResult InsertCustomer(InsertCustomerViewModel model)
        {
            if (model == null ||
                model.SettingContext == null ||
                model.UpdateDataSportsbookCustomer == null)
            {
                return InvalidModelJsonResult();
            }

            model.SettingContext.CustId = model.SettingContext.CustId;
            model.SettingContext.LoginUserId = CurrentSession.LoginUserId;
            model.UpdateDataSportsbookCustomer.CustInfo.UserName = model.UpdateDataSportsbookCustomer.CustInfo.UserName.Trim();
            var result = _customerSettingService.InsertCustomer(model);
            var messages = GenerateMessages(result);

            if (result.ErrCode == 0 && model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                var serviceResult = _memberService.InsertNewMember(new InsertNewMemberAction
                {
                    UserName = model.UpdateDataSportsbookCustomer.CustInfo.UserName,
                    Password = model.UpdateDataSportsbookCustomer.CustInfo.Password,
                    IBCCustId = result.CustIdReturn,
                    IBCUserName = result.IBCUserName,
                    LASAgentId = CurrentSession.AgentInfo.AgentId,
                    FirstName = model.UpdateDataSportsbookCustomer.CustInfo.FirstName,
                    LastName = model.UpdateDataSportsbookCustomer.CustInfo.LastName,
                    Phone = model.UpdateDataSportsbookCustomer.CustInfo.Phone,
                    OtherPhone = model.UpdateDataSportsbookCustomer.CustInfo.Mobile,
                    Fax = model.UpdateDataSportsbookCustomer.CustInfo.Fax,
                    SiteName = CurrentSession.LicenseeInfo.UserName,
                    Currency = CurrentSession.AgentInfo.CurrencyId
                });
                if (serviceResult.ErrorCode != 0)
                {
                    return Json(new
                    {
                        ErrCode = serviceResult.ErrorCode,
                        Messages = serviceResult.ErrMsg
                    });
                }
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Messages = messages
            });
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        [DownLine]
        [DirectDownLineAgent]
        public JsonResult UpdateCustomer(UpdateCustomerViewModel model)
        {
            if (model == null ||
                model.SettingContext == null ||
                model.UpdateDataSportsbookCustomer == null)
            {
                return InvalidModelJsonResult();
            }

            model.SettingContext.CustId = CurrentSession.AgentInfo.AgentId;
            model.SettingContext.LoginUserId = CurrentSession.LoginUserId;
            model.UpdateDataSportsbookCustomer.CustInfo.UserName = model.UpdateDataSportsbookCustomer.CustInfo.UserName.Trim();
            var result = _customerSettingService.UpdateCustomer(model);
            var messages = GenerateMessages(result, true);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Messages = messages
            });
        }

        /// <summary>
        /// Update indirect customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        [DownLine]
        public JsonResult UpdateIndirectCustomer(UpdateCustomerViewModel model)
        {
            if (model == null ||
                model.SettingContext == null ||
                model.UpdateDataSportsbookCustomer == null)
            {
                return InvalidModelJsonResult();
            }

            model.SettingContext.CustId = CurrentSession.AgentInfo.AgentId;
            model.SettingContext.LoginUserId = CurrentSession.AgentInfo.AgentId;
            model.UpdateDataSportsbookCustomer.CustInfo.UserName = model.UpdateDataSportsbookCustomer.CustInfo.UserName.Trim();
            var result = _customerSettingService.UpdateIndirectCustomer(model);
            var messages = GenerateMessages(result, true);
            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Messages = messages
            });
        }

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        [HttpGet]
        [DirectDownLineAgent]
        public ViewResult EditAgent(EditAgentViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (CurrentSession.AgentInfo.IsLevel0 || model.AgentLevel - CurrentSession.AgentInfo.AgentLevel == 1)
                    ? FormType.Edit
                    : FormType.ReadOnly,
                TargetCustId = model.AgentId,
                TargetRoleId = ModelHelper.IsAgentLevel1(model.AgentLevel) ? (int)Role.AgentLevel1 : (int)Role.Agent,
                Ip = Request.GetClientIp()
            };

            return View("~/Views/Account/EditCustomer.cshtml", settingContext);
        }

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        [DownLine]
        public ViewResult EditMember(EditMemberViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = FormType.Edit,
                TargetCustId = model.MemberId,
                TargetRoleId = (int)Role.Member,
                Ip = Request.GetClientIp(),
                IsOnlyUpdatePT = CurrentSession.AdminInfo == null 
                                && !ModelHelper.IsLastAgent(CurrentSession.AgentInfo.AgentLevel, CurrentSession.AgentInfo.NumberOfLevel)
            };
            
            return View("~/Views/Account/EditCustomer.cshtml", settingContext);
        }

        /// <summary>
        /// Validates the username is existed.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns JSON result.
        /// </returns>
        [HttpPost]
        public JsonResult CheckExistUsername(CheckExistUserNameViewModel model)
        {
            var criteria = new CheckExistUserNameCriteria
            {
                UserName = model.UserName,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId
            };

            _customerSettingService.CheckExistUserName(criteria);
            return JsonResult(criteria.MsgId);
        }

        /// <summary>
        /// Re-update message content.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="isUpdate">If set to <c>true</c> [is update].</param>
        /// <returns>Returns a list of result.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private IList<Result> GenerateMessages(InsertResult result, bool isUpdate = false)
        {
            var msgContents = new List<Result>();
            if (result.ErrCode == 0)
            {
                msgContents.Add(new Result(Result.SuccessValue, isUpdate ? Las.UpdateSuccessfully : Las.InsertSuccessfully));
            }
            else
            {
                if (isUpdate)
                {
                    if (betSettingErrCodes.Contains((ErrCode)result.ErrCode))
                    {
                        msgContents.Add(new Result(Result.SuccessValue, Las.UpdateCommissionSuccessfully));
                    }
                    else if (positionTakingErrCodes.Contains((ErrCode)result.ErrCode))
                    {
                        msgContents.Add(new Result(Result.SuccessValue, Las.UpdateCommissionSuccessfully));
                        msgContents.Add(new Result(Result.SuccessValue, Las.UpdateBetSettingSuccessfully));
                    }
                }

                result.MinPTOutput = result.MinPTOutput / 10000;
                result.MaxPTOutput = result.MaxPTOutput / 10000;
                var msgContent = result.ErrMsg;
                switch (result.ErrCode)
                {
                    case (int)ErrCode.Msg_1003:
                    case (int)ErrCode.Msg_1004:
                    case (int)ErrCode.Msg_1005:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            DataRefWrapper.GetSportName(result.BetLimitSportType, result.BetLimitBetType));
                        break;

                    case (int)ErrCode.Msg_1006:
                    case (int)ErrCode.Msg_1007:
                    case (int)ErrCode.Msg_1008:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatInt(result.BetLimitOutput),
                            DataRefWrapper.GetSportName(result.BetLimitSportType, result.BetLimitBetType));
                        break;

                    case (int)ErrCode.Msg_1010:
                    case (int)ErrCode.Msg_1011:
                    case (int)ErrCode.Msg_1012:
                        msgContent = string.Format(
                        ResourceManager.GetErrMsg(result.ErrCode),
                            GetSportName(result.PTSportType, result.PTBetType, result.IsLiveOutput),
                            DataRefWrapper.GetBetTypeName(result.PTBetType));
                        break;

                    case (int)ErrCode.Msg_1015:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatPercent(result.MinPTOutput),
                            Formatter.FormatPercent(result.MaxPTOutput),
                            GetSportName(result.PTSportType, result.PTBetType, result.IsLiveOutput),
                            DataRefWrapper.GetBetTypeName(result.PTBetType));
                        break;

                    case (int)ErrCode.Msg_1013:
                    case (int)ErrCode.Msg_1014:
                    case (int)ErrCode.Msg_1021:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatPercent(result.MinPTOutput),
                            Formatter.FormatPercent(result.MaxPTOutput),
                            GetSportName(result.PTSportType, result.PTBetType, result.IsLiveOutput),
                            DataRefWrapper.GetBetTypeName(result.PTBetType));
                        break;

                    case (int)ErrCode.Msg_1016:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatInt(result.MaxCreditOutput));
                        break;

                    default:
                        msgContent = ResourceManager.GetErrMsg(result.ErrCode);
                        break;
                }

                msgContents.Add(new Result(result.ErrCode, msgContent));
            }

            return msgContents;
        }

        private string GetSportName(int sportType, int betType, bool isLive)
        {
            var sportName = DataRefWrapper.GetSportName(sportType, betType);
            return sportType == (int)SportType.Soccer
                ? string.Format("{0} - {1}", sportName, isLive ? Las.Live : Las.DeadBall)
                : sportName;
        }
    }
}