namespace LicenseeAgent.ProductSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.ProductSetting.Criteria;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// SportsBookController class.
    /// </summary>
    public class SportsBookController : ProductBaseController
    {
        #region Constructors

        public SportsBookController()
            : base(null, null, null, null, null, null, null, null)
        {
        }

        #endregion Constructors

        #region Utility

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

            CustomerSettingService.CheckExistUserName(criteria);
            if (criteria.ErrCode == Result.SuccessValue)
            {
                var serviceResult = MemberService.CheckUserName(new CheckUserNameAction
                {
                    SiteName = CurrentSession.AgentInfo.LicenseeName,
                    UserName = model.UserName
                });

                if (serviceResult.ErrCode != Result.SuccessValue)
                {
                    return JsonResult(serviceResult);
                }
            }

            return JsonResult(criteria.ErrCode);
        }

        #endregion Utility

        #region Get setting

        /// <summary>
        /// Get Setting Context For Insert.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpGet]
        public JavaScriptResult GetSetting(GetSettingForInsertViewModel model)
        {
            var settingContext = new SettingContext(CurrentSession)
            {
                CustId = model.CustId,
                RoleId = model.RoleId,
                TargetCustId = model.TargetCustId,
                TargetRoleId = model.TargetRoleId,
                Ip = Request.GetClientIp()
            };

            if (!CheckPermissionForInserting(settingContext))
            {
                return RedirectToAccessDeniedJavascript();
            }

            var result = new Result { ErrCode = Result.FailureValue };
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(Formatter.FormatJson("settingContext", settingContext));
            var sportsBookSetting = SportsBookService.GetSettingForInsert(settingContext, result);

            if (result.ErrCode == Result.SuccessValue)
            {
                stringBuilder.AppendLine(Formatter.FormatJson("sportsBookSetting", sportsBookSetting));
                if (CoreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, (int)ProductCode.Product_2))
                {
                    var numberGameCustomer = NumberGameService.GetSettingForInsert(settingContext, result);
                    if (result.ErrCode == Result.SuccessValue)
                    {
                        stringBuilder.Append(Formatter.FormatJson("numberGameSetting", numberGameCustomer));
                    }
                }
            }

            if (result.ErrCode != Result.SuccessValue)
            {
                stringBuilder.Append(Formatter.FormatJson("result", new Result(result.ErrCode, ResourceManager.GetErrMsg(result.ErrCode))));
            }

            return JavaScript(stringBuilder.ToString());
        }

        #endregion Get setting

        #region Insert customer

        /// <summary>
        /// Insert a new customer.
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
                || !CheckPermissionForInserting(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View(settingContext);
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

            if (!CheckPermissionForInserting(model.SettingContext))
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
                return JsonResult(checkUserNameResult);
            }

            var productInfos = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            if (productInfos.Where(x => x.ProductId == (int)ProductCode.Product_2).Count() != 1)
            {
                model.NumberGameSetting = null;
            }

            var products3RD = GetProducts3RD(productInfos);
            var hasOffered3RD = products3RD.Count >= 1;
            var result = SportsBookService.InsertCustomer(model);
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
                    return JsonResult(insertResult);
                }
            }

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                Messages = new List<Result> { GenerateMessages(result) },
                CustId = result.CustIdReturn,
                HasOffered3RD = hasOffered3RD,
                CustomerLevel = ModelHelper.IsAgentLevel1(model.SettingContext.TargetRoleId - 1) ?
                    (int)Role.Agent : CurrentSession.AgentInfo.IsLevel0 ? (int)Role.AgentLevel1 : CurrentSession.AgentInfo.AgentLevel + 1
            });
        }

        #endregion Insert customer

        #region Update customer

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
            if (!ModelState.IsValid)
            {
                return InvalidModelViewResult();
            }

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

            if (!CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("EditCustomer", settingContext);
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
            if (!ModelState.IsValid)
            {
                return InvalidModelViewResult();
            }

            var settingContext = GetEditMemberSettingContext(model);

            if (!CheckPermissionForEditing(settingContext))
            {
                return RedirectToAccessDeniedPage();
            }

            return View("EditCustomer", settingContext);
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

            if (!CheckPermissionForEditing(model.SettingContext))
            {
                return RedirectToAccessDeniedJson();
            }

            var results = model.SettingContext.FormId == FormType.EditIndirectMember
                ? SportsBookService.UpdateIndirectCustomer(model)
                : SportsBookService.UpdateCustomer(model);
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

        #endregion Update customer
    }
}