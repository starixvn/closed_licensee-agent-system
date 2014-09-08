namespace LicenseeAgent.CustomerSetting.Controllers.ProductSetting
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.Services.ProductSetting;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// ProductSettingController class.
    /// </summary>
    public class ProductSettingController : BaseController
    {
        public ProductSettingController()
        {
        }

        public ProductSettingController(
            IMemberService memberService,
            ICoreService coreService,
            ICustomerSettingService customerSettingService,
            IDownLinePermissionService downLinePermissionService)
        {
            MemberService = memberService ?? new MemberService();
            CoreService = coreService ?? new CoreService();
            CustomerSettingService = customerSettingService ?? new CustomerSettingService();
            DownLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        public ICoreService CoreService { get; set; }

        public ICustomerSettingService CustomerSettingService { get; set; }

        public IDownLinePermissionService DownLinePermissionService { get; set; }

        public IMemberService MemberService { get; set; }

        /// <summary>
        /// Edits the product agent.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view to edit product settings.</returns>
        [HttpGet]
        public ViewResult EditProductAgent(EditProductAgentViewModel model)
        {
            var editProductItems = new List<EditProductItem>();
            var products = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            var listProducts3RD = GetProducts3RD(products);
            foreach (var product in listProducts3RD)
            {
                var url = string.Empty;
                switch (product.ProductId)
                {
                    case (int)Product.Product_5:
                        url = Url.Content(string.Format("~/Areas/CustomerSetting/Views/ProductSetting/VirtualSports/EditAgent?agentId={0}", model.AgentId, model.AgentLevel));
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

            return View("~/Areas/CustomerSetting/Views/ProductSetting/EditProduct.cshtml", viewModel);
        }

        /// <summary>
        /// Edits the product member.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Returns view to edit product settings.</returns>
        [HttpGet]
        [LegendType(LegendType.Credit)]
        public ViewResult EditProductMember(EditProductMemberViewModel model)
        {
            var editProductItems = new List<EditProductItem>();
            var products = CoreService.GetProducts(CurrentSession.AgentInfo.LicenseeId);
            var listProducts3RD = GetProducts3RD(products);
            foreach (var product in listProducts3RD)
            {
                var url = string.Empty;
                switch (product.ProductId)
                {
                    case (int)Product.Product_5:
                        url = Url.Content(string.Format("~/Areas/CustomerSetting/Views/ProductSetting/VirtualSports/EditMember?memberId={0}", model.CustId));
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
            if (criteria.MsgId == Result.SuccessValue)
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
        protected static Result GenerateMessages(ISettingResult result)
        {
            if ((int)result.ErrCode == Result.SuccessValue)
            {
                var updateResult = result as UpdateResult;
                if (updateResult != null)
                {
                    return GetUpdateMessageSuccess(updateResult);
                }
                else
                {
                    return new Result(Result.SuccessValue, Las.InsertSuccessfully);
                }
            }
            else
            {
                result.MinPT = result.MinPT / Constant.PTRate;
                result.MaxPT = result.MaxPT / Constant.PTRate;
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
        /// Checks for inserting customer setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if is valid settings.</returns>
        protected bool CheckForInsertingCustomerSetting(SettingContext settingContext)
        {
            var insertingCustomerSettingBelonging = new CustomerSettingBelonging
            {
                FormId = settingContext.FormId,
                CustId = settingContext.CustId,
                RoleId = settingContext.RoleId,
                TargetRoleId = settingContext.TargetRoleId
            };

            return DownLinePermissionService.CheckForInsertingCustomerSetting(insertingCustomerSettingBelonging);
        }

        /// <summary>
        /// Checks for editing customer setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if is valid settings.</returns>
        protected bool CheckForEditingCustomerSetting(SettingContext settingContext)
        {
            var editingCustomerSettingBelonging = new CustomerSettingBelonging
            {
                FormId = settingContext.FormId,
                CustId = settingContext.CustId,
                RoleId = settingContext.RoleId,
                TargetCustId = settingContext.TargetCustId,
                TargetRoleId = settingContext.TargetRoleId
            };

            return DownLinePermissionService.CheckForEditingCustomerSetting(editingCustomerSettingBelonging);
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

        /// <summary>
        /// Get error message.
        /// </summary>
        /// <param name="errCode">Error code get message.</param>
        /// <param name="sportType">Sport type get message.</param>
        /// <returns>Message name.</returns>
        private static string GetErrMsg(int errCode, int sportType)
        {
            var errMsg = ResourceManager.GetErrMsg(errCode);
            var virtualSportsTypeIds = SettingConfigurations.VirtualSportsTypeIds;

            if (virtualSportsTypeIds.Contains(sportType))
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
        /// Get products 3RD.
        /// </summary>
        /// <param name="productInfos">List products info.</param>
        /// <returns>List products 3RD.</returns>
        private List<ProductInfo> GetProducts3RD(IList<ProductInfo> productInfos)
        {
            var listProducts3RD = new List<ProductInfo>();
            var product = productInfos.Where(x => x.ProductId == (int)Product.Product_1);
            if (product.Any())
            {
                listProducts3RD.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_6);
            if (product.Any())
            {
                listProducts3RD.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_7);
            if (product.Any())
            {
                listProducts3RD.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_4);
            if (product.Any())
            {
                listProducts3RD.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_5);
            if (product.Any())
            {
                listProducts3RD.Add(product.FirstOrDefault());
            }

            product = productInfos.Where(x => x.ProductId == (int)Product.Product_8);
            if (product.Any())
            {
                listProducts3RD.Add(product.FirstOrDefault());
            }

            return listProducts3RD;
        }
    }
}
