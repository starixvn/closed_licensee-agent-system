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
    using LicenseeAgent.Core.Configurations;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
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

        private readonly IMemberService _memberService;

        public SportsbookSettingController(
                ISportsbookSettingService customerSettingService,
                IMemberService memberService)
        {
            _customerSettingService = customerSettingService;
            _memberService = memberService;
        }

        /// <summary>
        /// Get Setting Context For Insert.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpGet]
        public ActionResult GetSettingForInsert(GetSettingForInsertViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                CustId = model.CustId,
                RoleId = model.RoleId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId,
                Ip = Request.GetClientIp()
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
        [HttpGet]
        public ActionResult GetSettingForUpdate(GetSettingForUpdateViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (FormType)model.FormId,
                CustId = model.CustId,
                RoleId = model.RoleId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId,
                Ip = Request.GetClientIp()
            };

            var sportsBookCustomer = _customerSettingService.GetSportsbookSettingForUpdate(settingContext);

            if (model.TargetRoleId == (int)Role.Member && sportsBookCustomer.CustInfo != null)
            {
                sportsBookCustomer.CustInfo.CustId = model.TargetCustId;

                _memberService.GetMemberInformation<CustomerInfo>(
                                    sportsBookCustomer.CustInfo,
                                    new GetMemberInfoAction
                                    {
                                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                                        IBCCustId = model.TargetCustId
                                    });
            }

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
            if (model == null
                || model.SettingContext == null
                || model.UpdateDataSportsbookCustomer == null
                || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            var checkUserNameResult = _memberService.CheckUserName(new CheckUserNameAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                UserName = model.UpdateDataSportsbookCustomer.CustInfo.UserName
            });

            if (checkUserNameResult.ErrCode != Result.SuccessValue)
            {
                return Json(new { ErrCode = checkUserNameResult.ErrCode, ErrMsg = checkUserNameResult.ErrMsg });
            }

            var result = _customerSettingService.InsertCustomer(model);
            if (result.ErrCode == Result.SuccessValue && model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                var insertResult = _memberService.InsertNewMember(new InsertNewMemberAction
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserId = CurrentSession.LoginUserId,
                    UserName = model.UpdateDataSportsbookCustomer.CustInfo.UserName,
                    Password = model.UpdateDataSportsbookCustomer.CustInfo.Password,
                    IBCCustId = result.CustIdReturn,
                    IBCUserName = result.IBCUserName,
                    LASAgentId = model.SettingContext.CustId,
                    FirstName = model.UpdateDataSportsbookCustomer.CustInfo.FirstName ?? string.Empty,
                    LastName = model.UpdateDataSportsbookCustomer.CustInfo.LastName ?? string.Empty,
                    Phone = model.UpdateDataSportsbookCustomer.CustInfo.Phone ?? string.Empty,
                    OtherPhone = model.UpdateDataSportsbookCustomer.CustInfo.Mobile ?? string.Empty,
                    Fax = model.UpdateDataSportsbookCustomer.CustInfo.Fax ?? string.Empty,
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    Currency = CurrentSession.AgentInfo.CurrencyId,
                    IsClosed = model.UpdateDataSportsbookCustomer.CustInfo.Status == (int)AccountStatus.Closed
                });
                if (insertResult.ErrCode != Result.SuccessValue)
                {
                    return Json(new { ErrCode = insertResult.ErrCode, ErrMsg = insertResult.ErrMsg });
                }
            }

            var messages = GenerateMessages(result);
            return Json(new { ErrCode = Result.SuccessValue, Messages = messages });
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
            if (model == null
                || model.SettingContext == null
                || model.UpdateDataSportsbookCustomer == null
                || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            InsertResult result = null;
            if (model.SettingContext.FormId == FormType.EditIndirectMember)
            {
                result = _customerSettingService.UpdateIndirectCustomer(model);
            }
            else
            {
                result = _customerSettingService.UpdateCustomer(model);
            }

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
                Ip = Request.GetClientIp()
            };

            var agentId = model.AgentId ?? CurrentSession.AgentInfo.AgentId;
            var agentLevel = model.AgentLevel ?? CurrentSession.AgentInfo.AgentLevel;
            if (!CurrentSession.IsAdmin && !CurrentSession.AgentInfo.IsLastAgent(agentLevel))
            {
                settingContext.FormId = FormType.EditIndirectMember;
                settingContext.CustId = agentId;
            }

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
            if (criteria.MsgId == Result.SuccessValue)
            {
                var serviceResult = _memberService.CheckUserName(new CheckUserNameAction
                {
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    UserName = model.UserName
                });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return Json(new
                    {
                        ErrCode = serviceResult.ErrCode,
                        ErrMsg = serviceResult.ErrMsg
                    });
                }
            }

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
        private static IList<Result> GenerateMessages(InsertResult result, bool isUpdate = false)
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

        private static string GetSportName(int sportType, int betType, bool isLive)
        {
            var sportName = DataRefWrapper.GetSportName(sportType, betType);
            return sportType == (int)SportType.Soccer
                ? string.Format("{0} - {1}", sportName, isLive ? Las.Live : Las.DeadBall)
                : sportName;
        }
    }
}