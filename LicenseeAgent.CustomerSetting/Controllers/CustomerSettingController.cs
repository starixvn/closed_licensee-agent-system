// --------------------------------------------------------------------------
// <copyright file="CustomerSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.CustomerSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// Sport books setting controller.
    /// </summary>
    public class CustomerSettingController : BaseController
    {
        private static readonly ErrCode[] BetSettingErrCodes =
        {
            ErrCode.Msg_1003,
            ErrCode.Msg_1004,
            ErrCode.Msg_1005,
            ErrCode.Msg_1006,
            ErrCode.Msg_1007,
            ErrCode.Msg_1008,
            ErrCode.Msg_1025,
            ErrCode.Msg_1028,
            ErrCode.Msg_1100,
            ErrCode.Msg_1101
        };

        private static readonly ErrCode[] PositionTakingErrCodes =
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

        private static JsonSerializerSettings jsonSerializeSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        private readonly IMemberService _memberService;
        private readonly IDownlLinePermissionService _permissionService;
        private readonly ICoreService _coreService = new CoreService();
        private readonly Services.ISettingService _numberGameService = new Services.NumberGame.SettingService();
        private readonly Services.ISettingService _sportsBookService = new Services.SportsBook.SettingService();
        private readonly Services.VirtualSports.SettingService _virtualSportsService = new Services.VirtualSports.SettingService();
        private readonly ICustomerSettingService _customerSettingService = new CustomerSettingService();

        public CustomerSettingController(
                IMemberService memberService,
                IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _memberService = memberService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Get Setting Context For Insert.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpGet]
        public JavaScriptResult GetSettingForInsert(GetSettingForInsertViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                CustId = model.CustId,
                RoleId = model.RoleId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId,
                Ip = Request.GetClientIp()
            };

            if (!CheckForInsertingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedJavascript();
            }

            var errCode = Result.FailureValue;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(FormatJavascript(settingContext, "settingContext"));
            var sportsBookSetting = _sportsBookService.GetSettingForInsert(settingContext, out errCode);
            if (errCode == Result.SuccessValue)
            {
                stringBuilder.AppendLine(FormatJavascript(sportsBookSetting, "sportsBookSetting"));
                if (_coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                {
                    var numberGameCustomer = _numberGameService.GetSettingForInsert(settingContext, out errCode);
                    if (errCode == Result.SuccessValue)
                    {
                        stringBuilder.Append(FormatJavascript(numberGameCustomer, "numberGameSetting"));
                    }
                }
            }

            if (errCode != Result.SuccessValue)
            {
                var result = new Result(errCode, ResourceManager.GetErrMsg(errCode));
                stringBuilder.Append(FormatJavascript(result, "result"));
            }

            return JavaScript(stringBuilder.ToString());
        }

        /// <summary>
        /// Get Setting for updating customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JAVASCRIPT string object.
        /// </returns>
        [HttpGet]
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public JavaScriptResult GetSettingForUpdate(GetSettingForUpdateViewModel model)
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

            if (!CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedJavascript();
            }

            var errCode = Result.FailureValue;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(FormatJavascript(settingContext, "settingContext"));
            var sportsBookSetting = _sportsBookService.GetSettingForUpdate(settingContext, out errCode);
            if (errCode == Result.SuccessValue)
            {
                if (model.TargetRoleId == (int)Role.Member && sportsBookSetting.CustInfo != null)
                {
                    sportsBookSetting.CustInfo.CustId = model.TargetCustId;
                    _memberService.GetMemberInformation(
                        sportsBookSetting.CustInfo,
                        new GetMemberInfoAction
                        {
                            SiteName = CurrentSession.AgentInfo.LicenseeName,
                            IBCCustId = model.TargetCustId
                        });
                }

                stringBuilder.AppendLine(FormatJavascript(sportsBookSetting, "sportsBookSetting"));
                if (_coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                {
                    var numberGameCustomer = _numberGameService.GetSettingForUpdate(settingContext, out errCode);
                    if (errCode == Result.SuccessValue)
                    {
                        stringBuilder.Append(FormatJavascript(numberGameCustomer, "numberGameSetting"));
                    }
                }
            }

            if (errCode != Result.SuccessValue)
            {
                var result = new Result(errCode, ResourceManager.GetErrMsg(errCode));
                stringBuilder.Append(FormatJavascript(result, "result"));
            }

            return JavaScript(stringBuilder.ToString());
        }

        /// <summary>
        /// Get Setting for updating customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JAVASCRIPT string object.
        /// </returns>
        [HttpGet]
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public JavaScriptResult GetSettingVirtualSportsForUpdate(GetSettingForUpdateViewModel model)
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

            if (!CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedJavascript();
            }

            var errCode = Result.FailureValue;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(FormatJavascript(settingContext, "settingContext"));
            var virtualSportsSetting = _virtualSportsService.GetSettingForUpdate(settingContext, out errCode);
            if (errCode == Result.SuccessValue)
            {
                if (model.TargetRoleId == (int)Role.Member && virtualSportsSetting.CustInfo != null)
                {
                    virtualSportsSetting.CustInfo.CustId = model.TargetCustId;
                    _memberService.GetMemberInformation(
                        virtualSportsSetting.CustInfo,
                        new GetMemberInfoAction
                        {
                            SiteName = CurrentSession.AgentInfo.LicenseeName,
                            IBCCustId = model.TargetCustId
                        });
                }

                stringBuilder.AppendLine(FormatJavascript(virtualSportsSetting, "sportsBookSetting"));
            }

            if (errCode != Result.SuccessValue)
            {
                var result = new Result(errCode, ResourceManager.GetErrMsg(errCode));
                stringBuilder.Append(FormatJavascript(result, "result"));
            }

            return JavaScript(stringBuilder.ToString());
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

            if (!CheckForInsertingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Views/CustomerSetting/InsertCustomer.cshtml", settingContext);
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public JsonResult InsertCustomer(InsertCustomerViewModel model)
        {
            if (!ModelState.IsValid || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            if (!CheckForInsertingCustomerSetting(model.SettingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var checkUserNameResult = _memberService.CheckUserName(new CheckUserNameAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                UserName = model.SportsBookSetting.CustInfo.UserName
            });
            if (checkUserNameResult.ErrCode != Result.SuccessValue)
            {
                return Json(new { ErrCode = checkUserNameResult.ErrCode, ErrMsg = checkUserNameResult.ErrMsg });
            }

            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            if (productInfos.Where(x => x.ProductId == (int)Product.Product_2).Count() != 1)
            {
                model.NumberGameSetting = null;
            }

            var result = _customerSettingService.InsertCustomer(model);
            if (result.ErrCode == Result.SuccessValue && model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                var insertResult = _memberService.InsertNewMember(new InsertNewMemberAction
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    UserId = CurrentSession.LoginUserId,
                    UserName = model.SportsBookSetting.CustInfo.UserName,
                    Password = model.SportsBookSetting.CustInfo.Password,
                    IBCCustId = result.CustIdReturn,
                    IBCUserName = result.IBCUserName,
                    LASAgentId = model.SettingContext.CustId,
                    FirstName = model.SportsBookSetting.CustInfo.FirstName,
                    LastName = model.SportsBookSetting.CustInfo.LastName,
                    Phone = model.SportsBookSetting.CustInfo.Phone,
                    OtherPhone = model.SportsBookSetting.CustInfo.Mobile,
                    Fax = model.SportsBookSetting.CustInfo.Fax,
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    Currency = CurrentSession.AgentInfo.CurrencyId,
                    IsClosed = model.SportsBookSetting.CustInfo.Status == (int)AccountStatus.Closed
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
        public JsonResult UpdateCustomer(UpdateCustomerViewModel model)
        {
            if (!ModelState.IsValid || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            if (!CheckForEditingCustomerSetting(model.SettingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var result = model.SettingContext.FormId == FormType.EditIndirectMember
                ? _customerSettingService.UpdateIndirectCustomer(model)
                : _customerSettingService.UpdateCustomer(model);

            var hasUpdateCommission = model.SportsBookSetting.CommissionSetting != null
                && model.SportsBookSetting.CommissionSetting.CommissionItems.Count > 0;
            var hasUpdateBetSetting = model.SportsBookSetting.BettingLimitSetting != null
                && model.SportsBookSetting.BettingLimitSetting.Count > 0;
            var messages = GenerateMessages(result, true, hasUpdateCommission, hasUpdateBetSetting);
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
        public ActionResult EditAgent(EditAgentViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (CurrentSession.AgentInfo.IsLevel0 || model.AgentLevel - CurrentSession.AgentInfo.AgentLevel == 1)
                    ? FormType.Edit
                    : FormType.ViewIndirectAgent,
                TargetCustId = model.AgentId,
                TargetRoleId = ModelHelper.IsAgentLevel1(model.AgentLevel) ? (int)Role.AgentLevel1 : (int)Role.Agent,
                Ip = Request.GetClientIp()
            };

            if (!CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Views/CustomerSetting/EditCustomer.cshtml", settingContext);
        }

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        [HttpGet]
        public ActionResult EditVirtualSportsAgent(EditAgentViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (CurrentSession.AgentInfo.IsLevel0 || model.AgentLevel - CurrentSession.AgentInfo.AgentLevel == 1)
                    ? FormType.Edit
                    : FormType.ViewIndirectAgent,
                TargetCustId = model.AgentId,
                TargetRoleId = ModelHelper.IsAgentLevel1(model.AgentLevel) ? (int)Role.AgentLevel1 : (int)Role.Agent,
                Ip = Request.GetClientIp()
            };

            if (!CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Views/CustomerSetting/VirtualSports/EditVirtualSportsCustomer.cshtml", settingContext);
        }

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        public ActionResult EditMember(EditMemberViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = FormType.Edit,
                TargetCustId = model.MemberId,
                TargetRoleId = (int)Role.Member,
                Ip = Request.GetClientIp()
            };

            if (CurrentSession.AgentInfo.IsLevel0 && (model.AgentId == null || model.AgentLevel == null))
            {
                settingContext.FormId = FormType.Edit;
            }
            else
            {
                var agentId = model.AgentId ?? CurrentSession.AgentInfo.AgentId;
                var agentLevel = model.AgentLevel ?? CurrentSession.AgentInfo.AgentLevel;
                if (!CurrentSession.AgentInfo.IsLastAgent(agentLevel))
                {
                    settingContext.FormId = FormType.EditIndirectMember;
                    settingContext.CustId = agentId;
                }
            }

            if (!CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Views/CustomerSetting/EditCustomer.cshtml", settingContext);
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
        /// <param name="hasUpdateCommission">If set to <c>true</c> [has update commission].</param>
        /// <param name="hasUpdateBetSetting">If set to <c>true</c> [has update bet setting].</param>
        /// <returns>
        /// Returns a list of result.
        /// </returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static IList<Result> GenerateMessages(
            InsertResult result,
            bool isUpdate = false,
            bool hasUpdateCommission = false,
            bool hasUpdateBetSetting = false)
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
                    if (BetSettingErrCodes.Contains((ErrCode)result.ErrCode))
                    {
                        if (hasUpdateCommission)
                        {
                            msgContents.Add(new Result(Result.SuccessValue, Las.UpdateCommissionSuccessfully));
                        }
                    }
                    else if (PositionTakingErrCodes.Contains((ErrCode)result.ErrCode))
                    {
                        if (hasUpdateCommission)
                        {
                            msgContents.Add(new Result(Result.SuccessValue, Las.UpdateCommissionSuccessfully));
                        }

                        if (hasUpdateBetSetting)
                        {
                            msgContents.Add(new Result(Result.SuccessValue, Las.UpdateBetSettingSuccessfully));
                        }
                    }
                }

                result.MinPTOutput = result.MinPTOutput / 10000;
                result.MaxPTOutput = result.MaxPTOutput / 10000;
                string msgContent;
                switch (result.ErrCode)
                {
                    case (int)ErrCode.Msg_1003:
                    case (int)ErrCode.Msg_1004:
                    case (int)ErrCode.Msg_1005:
                    case (int)ErrCode.Msg_1100:
                    case (int)ErrCode.Msg_1101:
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
                            GetBetTypeName(result.PTBetType));
                        break;

                    case (int)ErrCode.Msg_1015:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatPercent(result.MinPTOutput),
                            Formatter.FormatPercent(result.MaxPTOutput),
                            GetSportName(result.PTSportType, result.PTBetType, result.IsLiveOutput),
                            GetBetTypeName(result.PTBetType));
                        break;

                    case (int)ErrCode.Msg_1013:
                    case (int)ErrCode.Msg_1014:
                    case (int)ErrCode.Msg_1021:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatPercent(result.MinPTOutput),
                            Formatter.FormatPercent(result.MaxPTOutput),
                            GetSportName(result.PTSportType, result.PTBetType, result.IsLiveOutput),
                            GetBetTypeName(result.PTBetType));
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
            return sportType == (int)SportType.SportType_1
                ? string.Format("{0} - {1}", sportName, isLive ? Las.Live : Las.DeadBall)
                : sportName;
        }

        /// <summary>
        /// Gets the name of the bet type.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <returns>Returns name of bet type.</returns>
        private static string GetBetTypeName(int betType)
        {
            if (betType == (int)BetType.BetType_81)
            {
                return Las.FirstLastBallOverUnder;
            }
            else if (betType == (int)BetType.BetType_83)
            {
                return Las.FirstLastBallOddEven;
            }

            return DataRefWrapper.GetBetTypeName(betType);
        }

        /// <summary>
        /// Checks for editing customer setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if is valid settings.</returns>
        private bool CheckForEditingCustomerSetting(SettingContext settingContext)
        {
            var editingCustomerSettingBelonging = new CustomerSettingBelonging
            {
                FormId = settingContext.FormId,
                CustId = settingContext.CustId,
                RoleId = settingContext.RoleId,
                TargetCustId = settingContext.TargetCustId,
                TargetRoleId = settingContext.TargetRoleId
            };

            return _permissionService.CheckForEditingCustomerSetting(editingCustomerSettingBelonging);
        }

        /// <summary>
        /// Checks for inserting customer setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if is valid settings.</returns>
        private bool CheckForInsertingCustomerSetting(SettingContext settingContext)
        {
            var insertingCustomerSettingBelonging = new CustomerSettingBelonging
            {
                FormId = settingContext.FormId,
                CustId = settingContext.CustId,
                RoleId = settingContext.RoleId,
                TargetRoleId = settingContext.TargetRoleId
            };

            return _permissionService.CheckForInsertingCustomerSetting(insertingCustomerSettingBelonging);
        }

        private string FormatJavascript(object obj, string name)
        {
            if (obj != null)
            {
                return string.Format("var {0} = {1}; \n", name, JsonConvert.SerializeObject(obj, jsonSerializeSettings));
            }

            return string.Empty;
        }
    }
}