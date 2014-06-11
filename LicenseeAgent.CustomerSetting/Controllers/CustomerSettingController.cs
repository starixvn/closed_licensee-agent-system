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
    using LicenseeAgent.CustomerSetting.Core;
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
        private static JsonSerializerSettings jsonSerializeSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        private static List<int> _virtualSportsTypeIds;
        private readonly IMemberService _memberService;
        private readonly IDownlLinePermissionService _permissionService;
        private readonly ICoreService _coreService = new CoreService();
        private readonly Services.ISettingService _numberGameService = new Services.NumberGame.SettingService();
        private readonly Services.ISettingService _sportsBookService = new Services.SportsBook.SettingService();
        private readonly Services.VirtualSports.SettingService _virtualSportsService = new Services.VirtualSports.SettingService();
        private readonly ICustomerSettingService _customerSettingService = new CustomerSettingService();

        static CustomerSettingController()
        {
            _virtualSportsTypeIds = SettingConfigurations.GetProductPtSettings((int)Product.Product_5).SportType.Select(x => x.Id).ToList();
        }

        public CustomerSettingController(
                IMemberService memberService,
                IDownlLinePermissionService permissionService)
            : base(permissionService)
        {
            _memberService = memberService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Edits the product agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view to edit product settings.</returns>
        [HttpGet]
        public ViewResult EditProductAgent(EditProductAgentViewModel model)
        {
            var editProductItems = new List<EditProductItem>();
            var products = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            var listProducts3rd = GetProducts3rd(products);
            foreach (var product in listProducts3rd)
            {
                var url = string.Empty;
                switch (product.ProductId)
                {
                    case (int)Product.Product_5:
                        url = Url.Content(string.Format("~/CustomerSetting/EditVirtualSportsAgent?agentId={0}&agentLevel={1}", model.AgentId, model.AgentLevel));
                        break;
                }

                if (!string.IsNullOrEmpty(url))
                {
                    editProductItems.Add(new EditProductItem
                    {
                        ProductId = product.ProductId,
                        ProductName = DataRefWrapper.GetProductName(product.ProductId),
                        Url = url
                    });
                }
            }

            var viewModel = new EditProductViewModel
            {
                ProductId = editProductItems.FirstOrDefault().ProductId,
                Products = editProductItems
            };

            return View("~/Views/CustomerSetting/EditProduct.cshtml", viewModel);
        }

        /// <summary>
        /// Edits the product member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view to edit product settings.</returns>
        [HttpGet]
        public ViewResult EditProductMember(EditProductMemberViewModel model)
        {
            var editProductItems = new List<EditProductItem>();
            var products = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            var listProducts3rd = GetProducts3rd(products);
            foreach (var product in listProducts3rd)
            {
                var url = string.Empty;
                switch (product.ProductId)
                {
                    case (int)Product.Product_5:
                        url = Url.Content(string.Format("~/CustomerSetting/EditVirtualSportsMember?memberId={0}", model.CustId));
                        break;
                }

                if (!string.IsNullOrEmpty(url))
                {
                    editProductItems.Add(new EditProductItem
                    {
                        ProductId = product.ProductId,
                        ProductName = DataRefWrapper.GetProductName(product.ProductId),
                        Url = url
                    });
                }
            }

            var viewModel = new EditProductViewModel
            {
                ProductId = editProductItems.FirstOrDefault().ProductId,
                Products = editProductItems
            };

            return View("~/Views/CustomerSetting/EditProduct.cshtml", viewModel);
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

            var settingService = GetSettingsService(model.ProductId);
            var productSetting = settingService.GetSettingForUpdate(settingContext, out errCode);
            if (errCode == Result.SuccessValue)
            {
                if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
                {
                    productSetting.CustInfo.CustId = model.TargetCustId;
                    _memberService.GetMemberInformation(
                        productSetting.CustInfo,
                        new GetMemberInfoAction
                        {
                            SiteName = CurrentSession.AgentInfo.LicenseeName,
                            IBCCustId = model.TargetCustId
                        });
                }

                stringBuilder.AppendLine(FormatJavascript(productSetting, settingService.Name));
                if (model.ProductId == Product.Product_0)
                {
                    if (_coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                    {
                        var numberGameSetting = _numberGameService.GetSettingForUpdate(settingContext, out errCode);
                        if (errCode == Result.SuccessValue)
                        {
                            stringBuilder.Append(FormatJavascript(numberGameSetting, _numberGameService.Name));
                        }
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
        /// Get Setting after updating customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>JSON object.</returns>
        [HttpPost]
        public JsonResult GetUpdatedSetting(GetSettingForUpdateViewModel model)
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
                return RedirectToAccessDeniedJson();
            }

            var errCode = Result.FailureValue;
            var settingService = GetSettingsService(model.ProductId);
            var productSetting = settingService.GetSettingForUpdate(settingContext, out errCode);
            CustomerSetting2 numberGameSetting = null;
            if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
            {
                productSetting.CustInfo.CustId = model.TargetCustId;
                _memberService.GetMemberInformation(
                    productSetting.CustInfo,
                    new GetMemberInfoAction
                    {
                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                        IBCCustId = model.TargetCustId
                    });
            }

            if (model.ProductId == Product.Product_0)
            {
                if (_coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                {
                    numberGameSetting = _numberGameService.GetSettingForUpdate(settingContext, out errCode);
                }
            }

            return Json(new
            {
                settingContext = settingContext,
                productSetting = productSetting,
                productSetting3rd = numberGameSetting
            });
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
            var hasOffered3rd = false;
            if (productInfos.Where(x => x.ProductId == (int)Product.Product_2).Count() != 1)
            {
                model.NumberGameSetting = null;
            }

            if (productInfos.Where(x => x.ProductId == (int)Product.Product_5 || x.ProductId == (int)Product.Product_4 || x.ProductId == (int)Product.Product_8).Any())
            {
                hasOffered3rd = true;
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

            var messages = new List<Result> { GenerateMessages(result) };

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Messages = messages,
                CustId = result.CustIdReturn,
                HasOffered3rd = hasOffered3rd,
                CustomerLevel = CurrentSession.AgentInfo.AgentLevel + 1
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
            if (!ModelState.IsValid || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            if (!CheckForEditingCustomerSetting(model.SettingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var results = model.SettingContext.FormId == FormType.EditIndirectMember
                ? _customerSettingService.UpdateIndirectCustomer(model)
                : _customerSettingService.UpdateCustomer(model);
            var messages = new List<Result>();

            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    messages.Add(GenerateMessages(result));
                }
            }
            else
            {
                messages.Add(new Result(Result.NoChange, Las.NoChangeToUpdate));
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
        public JsonResult UpdateVirtualSportsCustomer(UpdateVirtualSportsCustomerViewModel model)
        {
            if (!ModelState.IsValid || model.SettingContext.LoginUserId != CurrentSession.LoginUserId)
            {
                return InvalidModelJsonResult();
            }

            if (!CheckForEditingCustomerSetting(model.SettingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var results = _virtualSportsService.UpdateCustomer(model);

            var messages = new List<Result>();

            foreach (var result in results)
            {
                messages.Add(GenerateMessages(result));
            }

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

            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            if (!productInfos.Where(x => x.ProductId == (int)Product.Product_5).Any() || !CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Views/CustomerSetting/VirtualSports/EditCustomer.cshtml", settingContext);
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
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        public ActionResult EditVirtualSportsMember(EditMemberViewModel model)
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

            var productInfos = _coreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            if (!productInfos.Where(x => x.ProductId == (int)Product.Product_5).Any() || !CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Views/CustomerSetting/VirtualSports/EditCustomer.cshtml", settingContext);
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
        /// <returns>
        /// Returns a list of result.
        /// </returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static Result GenerateMessages(ISettingResult result)
        {
            if ((int)result.ErrCode == Result.SuccessValue)
            {
                return result is UpdateResult ? GetUpdateMessageSuccess((UpdateResult)result) : new Result(Result.SuccessValue, Las.InsertSuccessfully);
            }
            else
            {
                result.MinPT = result.MinPT / Constant.PT_RATE;
                result.MaxPT = result.MaxPT / Constant.PT_RATE;
                string msgContent;
                switch ((int)result.ErrCode)
                {
                    case (int)ErrCode.Msg_1003:
                    case (int)ErrCode.Msg_1004:
                    case (int)ErrCode.Msg_1005:
                    case (int)ErrCode.Msg_1100:
                    case (int)ErrCode.Msg_1101:
                    case (int)ErrCode.Msg_1102:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            DataRefWrapper.GetSportName(result.SportType, result.BetType));
                        break;

                    case (int)ErrCode.Msg_1006:
                    case (int)ErrCode.Msg_1007:
                    case (int)ErrCode.Msg_1008:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatInt(result.BetLimit),
                            DataRefWrapper.GetSportName(result.SportType, result.BetType));
                        break;

                    case (int)ErrCode.Msg_1010:
                    case (int)ErrCode.Msg_1011:
                    case (int)ErrCode.Msg_1012:
                        msgContent = string.Format(
                        ResourceManager.GetErrMsg(result.ErrCode),
                            GetSportName(result.SportType, result.BetType, result.IsLive),
                            GetBetTypeName(result.BetType));
                        break;

                    case (int)ErrCode.Msg_1015:
                        msgContent = string.Format(
                            GetErrMsg(result.ErrCode, result.SportType),
                            Formatter.FormatPercent(result.MinPT),
                            Formatter.FormatPercent(result.MaxPT),
                            GetSportName(result.SportType, result.BetType, result.IsLive),
                            GetBetTypeName(result.BetType));
                        break;

                    case (int)ErrCode.Msg_1013:
                    case (int)ErrCode.Msg_1014:
                    case (int)ErrCode.Msg_1021:
                        msgContent = string.Format(
                            GetErrMsg(result.ErrCode, result.SportType),
                            Formatter.FormatPercent(result.MinPT),
                            Formatter.FormatPercent(result.MaxPT),
                            GetSportName(result.SportType, result.BetType, result.IsLive),
                            GetBetTypeName(result.BetType));
                        break;

                    case (int)ErrCode.Msg_1016:
                        msgContent = string.Format(
                            ResourceManager.GetErrMsg(result.ErrCode),
                            Formatter.FormatInt(result.MaxCredit));
                        break;

                    default:
                        msgContent = ResourceManager.GetErrMsg(result.ErrCode);
                        break;
                }

                return new Result(result.ErrCode, msgContent);
            }
        }

        /// <summary>
        /// Get update message success.
        /// </summary>
        /// <param name="result">Insert or Update Result.</param>
        /// <returns>Result Information.</returns>
        private static Result GetUpdateMessageSuccess(UpdateResult result)
        {
            var productId = DataRefWrapper.GetProductIdBySportType((int)result.SportType);
            var productName = DataRefWrapper.GetProductName(productId);

            var msgContent = string.Empty;

            switch ((ErrorType)result.ErrType)
            {
                case ErrorType.Commission:
                    msgContent = string.Format(Las.UpdateCommissionSuccessfully, productName);
                    break;

                case ErrorType.BetSetting:
                    msgContent = string.Format(GetSuccessMsg(productId, Las.UpdateBetSettingSuccessfully), productName);
                    break;

                case ErrorType.PositionTaking:
                    msgContent = string.Format(GetSuccessMsg(productId, Las.UpdatePositionTakingSuccessfully), productName);
                    break;
            }

            return new Result(Result.SuccessValue, msgContent);
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

        private static string GetErrMsg(int errCode, int sportType)
        {
            var errMsg = ResourceManager.GetErrMsg(errCode);

            if (_virtualSportsTypeIds.Contains(sportType))
            {
                errMsg = errMsg.Replace(" - {3}", string.Empty);
            }

            return errMsg;
        }

        private static string GetSuccessMsg(int productId, string message)
        {
            var msgSuccess = message;

            if (productId == (int)Product.Product_5)
            {
                msgSuccess = msgSuccess.Replace("{0} - ", string.Empty);
            }

            return msgSuccess;
        }

        /// <summary>
        /// Gets the settings service.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>Setting Service.</returns>
        private ISettingService GetSettingsService(Product productId)
        {
            switch (productId)
            {
                case Product.Product_0:
                    return _sportsBookService;

                case Product.Product_2:
                    return _numberGameService;

                case Product.Product_5:
                    return _virtualSportsService;
            }

            return null;
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

        /// <summary>
        /// Get products 3RD.
        /// </summary>
        /// <param name="productInfos">List products info.</param>
        /// <returns>List products 3RD.</returns>
        private List<ProductInfo> GetProducts3rd(IList<ProductInfo> productInfos)
        {
            var listProducts3rd = new List<ProductInfo>();
            var product = productInfos.Where(x => x.ProductId == (int)Product.Product_1);
            if (product.Any())
            {
                listProducts3rd.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_6);
            if (product.Any())
            {
                listProducts3rd.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_7);
            if (product.Any())
            {
                listProducts3rd.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_4);
            if (product.Any())
            {
                listProducts3rd.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_5);
            if (product.Any())
            {
                listProducts3rd.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_8);
            if (product.Any())
            {
                listProducts3rd.Add(product.FirstOrDefault());
            }

            return listProducts3rd;
        }
    }
}