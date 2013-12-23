// --------------------------------------------------------------------------
// <copyright file="SportsbookSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.18">Created</change>
//   <change who="Ricky.Nguyen" date="2013.11.4">Update customer object</change>
// </history>

namespace LicenseeAgent.CustomerSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utils;
    using LicenseeAgent.CustomerSetting.Criterias;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Entities.Sportsbook;
    using LicenseeAgent.CustomerSetting.Service;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;

    /// <summary>
    /// Sport books setting controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SportsbookSettingController : BaseController
    {
        private static MsgId[] betSettingMsgIds =
        {
            MsgId.Msg_1003,
            MsgId.Msg_1004,
            MsgId.Msg_1005,
            MsgId.Msg_1006,
            MsgId.Msg_1007,
            MsgId.Msg_1008,
            MsgId.Msg_1025,
            MsgId.Msg_1028
        };

        private static MsgId[] positionTakingMsgIds =
        {
            MsgId.Msg_1010,
            MsgId.Msg_1011,
            MsgId.Msg_1012,
            MsgId.Msg_1013,
            MsgId.Msg_1014,
            MsgId.Msg_1015,
            MsgId.Msg_1021,
            MsgId.Msg_1026,
            MsgId.Msg_1029
        };

        private readonly ISportsbookSettingService _customerSettingService;

        public SportsbookSettingController(ISportsbookSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService;
        }

        /// <summary>
        /// Get Setting for insert new customer.
        /// </summary>
        /// <returns>
        /// JAVASCRIPT string object.
        /// </returns>
        public ActionResult GetSettingForInsert()
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = FormType.Add
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
        public ActionResult GetSettingForUpdate(GetSettingForUpdateViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = FormType.Edit,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId
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
        /// <returns>
        /// Returns view.
        /// </returns>
        [HttpGet]
        public ActionResult InsertCustomer()
        {
            var settingContext = new SettingContext(CurrentSession);
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

            model.SettingContext.CustId = CurrentSession.AgentInfo.AgentId;
            model.SettingContext.LoginUserId = CurrentSession.LoginUserId;
            model.UpdateDataSportsbookCustomer.CustInfo.UserName = model.UpdateDataSportsbookCustomer.CustInfo.UserName.Trim();
            var result = _customerSettingService.InsertCustomer(model);
            var messages = GenerateMessages(result);
            return Json(new
            {
                MsgId = Result.SuccessValue,
                Messages = messages
            });
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
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
                MsgId = Result.SuccessValue,
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
        public ViewResult EditAgent(EditAgentViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = FormType.Edit,
                TargetCustId = model.AgentId,
                TargetRoleId = ModelHelper.IsAgentLevel1(model.AgentLevel) ? (int)Role.AgentLevel1 : (int)Role.Agent
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
        [HttpGet]
        public ViewResult EditMember(EditMemberViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = FormType.Edit,
                TargetCustId = model.MemberId,
                TargetRoleId = (int)Role.Member
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private IList<Result> GenerateMessages(InsertResult result, bool isUpdate = false)
        {
            var msgContents = new List<Result>();
            if (result.MsgId == 0)
            {
                msgContents.Add(new Result(Result.SuccessValue, isUpdate ? Las.UpdateSuccessfully : Las.InsertSuccessfully));
            }
            else
            {
                if (isUpdate)
                {
                    if (betSettingMsgIds.Contains((MsgId)result.MsgId))
                    {
                        msgContents.Add(new Result(Result.SuccessValue, Las.UpdateCommissionSuccessfully));
                    }
                    else if (positionTakingMsgIds.Contains((MsgId)result.MsgId))
                    {
                        msgContents.Add(new Result(Result.SuccessValue, Las.UpdateCommissionSuccessfully));
                        msgContents.Add(new Result(Result.SuccessValue, Las.UpdateBetSettingSuccessfully));
                    }
                }

                result.MinPTOutput = result.MinPTOutput / 10000;
                result.MaxPTOutput = result.MaxPTOutput / 10000;
                var msgContent = result.MsgContent;
                switch (result.MsgId)
                {
                    case (int)MsgId.Msg_1003:
                    case (int)MsgId.Msg_1004:
                    case (int)MsgId.Msg_1005:
                        msgContent = string.Format(
                            ResourceManager.GetErrorMsg(result.MsgId),
                            DataRefWrapper.GetSportName(result.BetLimitSportType, result.BetLimitBetType));
                        break;

                    case (int)MsgId.Msg_1006:
                    case (int)MsgId.Msg_1007:
                    case (int)MsgId.Msg_1008:
                        msgContent = string.Format(
                            ResourceManager.GetErrorMsg(result.MsgId),
                            Formatter.FormatInt(result.BetLimitOutput),
                            DataRefWrapper.GetSportName(result.BetLimitSportType, result.BetLimitBetType));
                        break;

                    case (int)MsgId.Msg_1010:
                    case (int)MsgId.Msg_1011:
                    case (int)MsgId.Msg_1012:
                        msgContent = string.Format(
                        ResourceManager.GetErrorMsg(result.MsgId),
                            DataRefWrapper.GetSportName(result.PTSportType, result.PTBetType),
                            DataRefWrapper.GetBetTypeName(result.PTBetType));
                        break;

                    case (int)MsgId.Msg_1015:
                        msgContent = string.Format(
                            ResourceManager.GetErrorMsg(result.MsgId),
                            Formatter.FormatPercent(result.MinPTOutput),
                            Formatter.FormatPercent(result.MaxPTOutput),
                            DataRefWrapper.GetSportName(result.PTSportType, result.PTBetType),
                            DataRefWrapper.GetBetTypeName(result.PTBetType));
                        break;

                    case (int)MsgId.Msg_1013:
                    case (int)MsgId.Msg_1014:
                    case (int)MsgId.Msg_1021:
                        msgContent = string.Format(
                            ResourceManager.GetErrorMsg(result.MsgId),
                            Formatter.FormatPercent(result.MinPTOutput),
                            Formatter.FormatPercent(result.MaxPTOutput),
                            DataRefWrapper.GetSportName(result.PTSportType, result.PTBetType),
                            DataRefWrapper.GetBetTypeName(result.PTBetType));
                        break;

                    case (int)MsgId.Msg_1016:
                        msgContent = string.Format(
                            ResourceManager.GetErrorMsg(result.MsgId),
                            Formatter.FormatInt(result.MaxCreditOutput));
                        break;

                    default:
                        msgContent = ResourceManager.GetErrorMsg(result.MsgId);
                        break;
                }

                msgContents.Add(new Result(result.MsgId, msgContent));
            }

            return msgContents;
        }
    }
}