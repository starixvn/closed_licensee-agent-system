namespace LicenseeAgent.CustomerSetting.Controllers.ProductSetting
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
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.Services.ProductSetting;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// SportsBookController class.
    /// </summary>
    public class SportsBookController : ProductSettingController
    {
        private readonly ISportsBookSettingService _sportsBookService;

        private readonly ISettingService _numberGameService;

        public SportsBookController()
            : this(null, null, null, null, null, null)
        {
        }

        public SportsBookController(
            IMemberService memberService,
            ICoreService coreService,
            ICustomerSettingService customerSettingService,
            IDownLinePermissionService downLinePermissionService,
            ISportsBookSettingService sportsBookService,
            ISettingService numberGameService)
            : base(memberService, coreService, customerSettingService, downLinePermissionService)
        {
            _sportsBookService = sportsBookService ?? new SportsBookSettingService();
            _numberGameService = numberGameService ?? new NumberGameSettingService();
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
            stringBuilder.AppendLine(Formatter.FormatJson("settingContext", settingContext));
            var sportsBookSetting = _sportsBookService.GetSettingForInsert(settingContext, out errCode);
            if (errCode == Result.SuccessValue)
            {
                stringBuilder.AppendLine(Formatter.FormatJson("sportsBookSetting", sportsBookSetting));
                if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                {
                    var numberGameCustomer = _numberGameService.GetSettingForInsert(settingContext, out errCode);
                    if (errCode == Result.SuccessValue)
                    {
                        stringBuilder.Append(Formatter.FormatJson("numberGameSetting", numberGameCustomer));
                    }
                }
            }

            if (errCode != Result.SuccessValue)
            {
                var result = new Result(errCode, ResourceManager.GetErrMsg(errCode));
                stringBuilder.Append(Formatter.FormatJson("result", result));
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
            stringBuilder.AppendLine(Formatter.FormatJson("settingContext", settingContext));

            var productSetting = _sportsBookService.GetSettingForUpdate(settingContext, out errCode);
            if (errCode == Result.SuccessValue)
            {
                if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
                {
                    productSetting.CustInfo.CustId = model.TargetCustId;
                    MemberService.GetMemberInformation(
                        productSetting.CustInfo,
                        new GetMemberInfoAction
                        {
                            SiteName = CurrentSession.AgentInfo.LicenseeName,
                            IBCCustId = model.TargetCustId
                        });
                }

                stringBuilder.AppendLine(Formatter.FormatJson(_sportsBookService.Name, productSetting));
                if (model.ProductId == Product.Product_0)
                {
                    if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                    {
                        var numberGameSetting = _numberGameService.GetSettingForUpdate(settingContext, out errCode);
                        if (errCode == Result.SuccessValue)
                        {
                            stringBuilder.Append(Formatter.FormatJson(_numberGameService.Name, numberGameSetting));
                        }
                    }
                }
            }

            if (errCode != Result.SuccessValue)
            {
                var result = new Result(errCode, ResourceManager.GetErrMsg(errCode));
                stringBuilder.Append(Formatter.FormatJson("result", result));
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
            var productSetting = _sportsBookService.GetSettingForUpdate(settingContext, out errCode);
            CustomerSetting2 numberGameSetting = null;
            if (model.TargetRoleId == (int)Role.Member && productSetting.CustInfo != null)
            {
                productSetting.CustInfo.CustId = model.TargetCustId;
                MemberService.GetMemberInformation(
                    productSetting.CustInfo,
                    new GetMemberInfoAction
                    {
                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                        IBCCustId = model.TargetCustId
                    });
            }

            if (model.ProductId == Product.Product_0)
            {
                if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)Product.Product_2))
                {
                    numberGameSetting = _numberGameService.GetSettingForUpdate(settingContext, out errCode);
                }
            }

            return Json(new
            {
                settingContext = settingContext,
                productSetting = productSetting,
                productSetting3RD = numberGameSetting
            });
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpGet]
        public ActionResult InsertCustomer(int? agentId)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                Ip = Request.GetClientIp()
            };

            var isAgent = agentId != null;
            agentId = agentId ?? CurrentSession.AgentInfo.AgentId;
            var agentInfo = CoreService.GetAgentInfo(agentId.Value);
            settingContext.AgentLevel = agentInfo.AgentLevel;
            settingContext.UserName = agentInfo.UserName ?? CurrentSession.AgentInfo.LicenseeName;

            if (isAgent)
            {
                settingContext.CustId = agentId.Value;
                settingContext.RoleId = (int)Role.Agent;
                settingContext.TargetRoleId = CurrentSession.AgentInfo.IsLastAgent(agentInfo.AgentLevel)
                    ? (int)Role.Member
                    : (int)Role.Agent;
            }

            if ((settingContext.TargetRoleId == (int)Role.Member && LegendTypeAttribute.Authorize(LegendType.Deposit))
                || !CheckForInsertingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Areas/CustomerSetting/Views/ProductSetting/InsertCustomer.cshtml", settingContext);
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

            if (!CheckForInsertingCustomerSetting(model.SettingContext)
                || (model.SettingContext.TargetRoleId == (int)Role.Member && LegendTypeAttribute.Authorize(LegendType.Deposit)))
            {
                return RedirectToAccessDeniedJson();
            }

            var checkUserNameResult = MemberService.CheckUserName(new CheckUserNameAction
            {
                SiteName = CurrentSession.AgentInfo.LicenseeName,
                UserName = model.SportsBookSetting.CustInfo.UserName
            });
            if (checkUserNameResult.ErrCode != Result.SuccessValue)
            {
                return Json(new { ErrCode = checkUserNameResult.ErrCode, ErrMsg = checkUserNameResult.ErrMsg });
            }

            var productInfos = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            var hasOffered3RD = false;
            if (productInfos.Where(x => x.ProductId == (int)Product.Product_2).Count() != 1)
            {
                model.NumberGameSetting = null;
            }

            if (productInfos.Where(x => x.ProductId == (int)Product.Product_5 || x.ProductId == (int)Product.Product_4 || x.ProductId == (int)Product.Product_8).Any())
            {
                hasOffered3RD = true;
            }

            var result = _sportsBookService.InsertCustomer(model);
            if (result.ErrCode == Result.SuccessValue && model.SettingContext.TargetRoleId == (int)Role.Member)
            {
                var insertResult = MemberService.InsertNewMember(new InsertNewMemberAction
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
                HasOffered3RD = hasOffered3RD,
                CustomerLevel = ModelHelper.IsAgentLevel1(model.SettingContext.TargetRoleId - 1) ?
                                            (int)Role.Agent : CurrentSession.AgentInfo.IsLevel0 ? (int)Role.AgentLevel1 : CurrentSession.AgentInfo.AgentLevel + 1
            });
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        public JsonResult UpdateCustomer(UpdateSportsBookCustomerViewModel model)
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
                ? _sportsBookService.UpdateIndirectCustomer(model)
                : _sportsBookService.UpdateCustomer(model);
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
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        [HttpGet]
        public ActionResult EditAgent(EditAgentViewModel model)
        {
            var agentInfo = CoreService.GetAgentInfo(model.AgentId);
            var settingContext = new SettingContext(CurrentSession)
            {
                FormId = (CurrentSession.AgentInfo.IsLevel0 || agentInfo.AgentLevel - CurrentSession.AgentInfo.AgentLevel == 1)
                    ? FormType.Edit
                    : FormType.ViewIndirectAgent,
                TargetCustId = model.AgentId,
                TargetRoleId = ModelHelper.IsAgentLevel1(agentInfo.AgentLevel) ? (int)Role.AgentLevel1 : (int)Role.Agent,
                Ip = Request.GetClientIp()
            };

            if (!CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Areas/CustomerSetting/Views/ProductSetting/SportsBook/EditCustomer.cshtml", settingContext);
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

            return View("~/Areas/CustomerSetting/Views/ProductSetting/SportsBook/EditCustomer.cshtml", settingContext);
        }
    }
}
