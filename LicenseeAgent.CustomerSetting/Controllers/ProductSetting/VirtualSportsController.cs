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
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// VirtualSportsController class.
    /// </summary>
    public class VirtualSportsController : ProductSettingController
    {
        private readonly IVirtualSportsSettingService _virtualSportsService;

        public VirtualSportsController()
            : this(null, null, null, null, null)
        {
        }

        public VirtualSportsController(
            IMemberService memberService,
            ICoreService coreService,
            ICustomerSettingService customerSettingService,
            IDownLinePermissionService downLinePermissionService,
            IVirtualSportsSettingService virtualSportsService)
            : base(memberService, coreService, customerSettingService, downLinePermissionService)
        {
            _virtualSportsService = virtualSportsService ?? new VirtualSportsSettingService();
        }

        /// <summary>
        /// Inserts the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns JSON result.</returns>
        [HttpPost]
        public JsonResult UpdateCustomer(UpdateVirtualSportsCustomerViewModel model)
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

            var productInfos = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            if (!productInfos.Where(x => x.ProductId == (int)Product.Product_5).Any() || !CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Areas/CustomerSetting/Views/ProductSetting/VirtualSports/EditCustomer.cshtml", settingContext);
        }

        /// <summary>
        /// Edits the agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns view.
        /// </returns>
        [HttpGet]
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

            var productInfos = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);

            if (!productInfos.Where(x => x.ProductId == (int)Product.Product_5).Any() || !CheckForEditingCustomerSetting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("~/Areas/CustomerSetting/Views/ProductSetting/VirtualSports/EditCustomer.cshtml", settingContext);
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

            var productSetting = _virtualSportsService.GetSettingForUpdate(settingContext, out errCode);
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

                stringBuilder.AppendLine(Formatter.FormatJson(_virtualSportsService.Name, productSetting));
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
            var productSetting = _virtualSportsService.GetSettingForUpdate(settingContext, out errCode);
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

            return Json(new
            {
                settingContext = settingContext,
                productSetting = productSetting,
                productSetting3RD = numberGameSetting
            });
        }
    }
}
