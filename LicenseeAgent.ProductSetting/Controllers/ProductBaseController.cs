namespace LicenseeAgent.ProductSetting.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.ProductSetting.Core;
    using LicenseeAgent.ProductSetting.Entities;
    using LicenseeAgent.ProductSetting.Services;
    using LicenseeAgent.ProductSetting.Services.ProductSetting;
    using LicenseeAgent.ProductSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Results;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;

    /// <summary>
    /// ProductSettingController class.
    /// </summary>
    public class ProductBaseController : BaseController
    {
        #region Variables

        private readonly IMemberService _memberService;
        private readonly ICoreService _coreService;
        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly ICustomerSettingService _customerSettingService;
        private readonly ISportsBookSettingService _sportsBookService;
        private readonly ISettingService _numberGameService;
        private readonly IVirtualSportsSettingService _virtualSportsService;
        private readonly IKenoSettingService _kenoService;

        #endregion Variables

        #region Constructors

        protected ProductBaseController()
            : this(null, null, null, null, null, null, null, null)
        {
        }

        protected ProductBaseController(
            IMemberService memberService,
            ICoreService coreService,
            IDownLinePermissionService downLinePermissionService,
            ICustomerSettingService customerSettingService,
            ISportsBookSettingService sportsBookService,
            ISettingService numberGameService,
            IVirtualSportsSettingService virtualSportsService,
            IKenoSettingService kenoService)
        {
            _memberService = memberService ?? new MemberService();
            _coreService = coreService ?? new CoreService();
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
            _sportsBookService = sportsBookService ?? new SportsBookSettingService();
            _numberGameService = numberGameService ?? new NumberGameSettingService();
            _virtualSportsService = virtualSportsService ?? new VirtualSportsSettingService();
            _kenoService = kenoService ?? new KenoSettingService();
        }

        #endregion Constructors

        #region Properties

        protected IMemberService MemberService
        {
            get { return _memberService; }
        }

        protected ICoreService CoreService
        {
            get { return _coreService; }
        }

        protected IDownLinePermissionService DownLinePermissionService
        {
            get { return _downLinePermissionService; }
        }

        protected ICustomerSettingService CustomerSettingService
        {
            get { return _customerSettingService; }
        }

        protected ISportsBookSettingService SportsBookService
        {
            get { return _sportsBookService; }
        }

        protected ISettingService NumberGameService
        {
            get { return _numberGameService; }
        }

        protected IVirtualSportsSettingService VirtualSportsService
        {
            get { return _virtualSportsService; }
        }

        protected IKenoSettingService KenoService
        {
            get { return _kenoService; }
        }

        #endregion Properties

        #region Common

        /// <summary>
        /// Get products 3RD.
        /// </summary>
        /// <param name="products">List products info.</param>
        /// <returns>List products 3RD.</returns>
        protected List<ProductInfo> GetProducts3RD(IList<ProductInfo> products)
        {
            var listProducts3RD = new List<ProductInfo>();
            foreach (var product in SettingConfiguration.Products3RD)
            {
                var query = products.Where(x => x.ProductId == (int)product);
                if (query.Any())
                {
                    listProducts3RD.Add(query.FirstOrDefault());
                }
            }

            return listProducts3RD;
        }

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <param name="custInfo">The customer information.</param>
        /// <returns>
        /// Returns response result.
        /// </returns>
        protected ResponseResult GetMemberInfo(CustomerInfo custInfo)
        {
            if (CurrentSession.AgentInfo.IsDeposit)
            {
                return MemberService.GetDepositMemberInfo(
                    custInfo,
                    new GetDepositMemberInfoAction
                    {
                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                        UserName = custInfo.UserName
                    });
            }
            else if (CurrentSession.AgentInfo.IsCredit)
            {
                return MemberService.GetMemberInfo(
                    custInfo,
                    new GetMemberInfoAction
                    {
                        SiteName = CurrentSession.AgentInfo.LicenseeName,
                        IBCCustId = custInfo.CustId
                    });
            }

            return null;
        }

        #endregion Common

        #region Check permission

        /// <summary>
        /// Checks for inserting customer setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if is valid settings.</returns>
        protected bool CheckPermissionForInserting(SettingContext settingContext)
        {
            if (settingContext.TargetRoleId == (int)Role.Member && LegendTypeAttribute.Authorize(LegendType.Deposit))
            {
                return true;
            }

            return CurrentSession.IsAdmin || CurrentSession.AgentInfo.AgentId == settingContext.CustId;
        }

        /// <summary>
        /// Checks for editing customer setting.
        /// </summary>
        /// <param name="settingContext">The setting context.</param>
        /// <returns>Returns true if is valid settings.</returns>
        protected bool CheckPermissionForEditing(SettingContext settingContext)
        {
            var editingCustomerSettingBelonging = new CustomerSettingBelonging
            {
                FormId = settingContext.FormId,
                CustId = settingContext.CustId,
                RoleId = settingContext.RoleId,
                TargetCustId = settingContext.TargetCustId,
                TargetRoleId = settingContext.TargetRoleId
            };

            return _downLinePermissionService.CheckForEditingCustomerSetting(editingCustomerSettingBelonging);
        }

        /// <summary>
        /// Get edit member setting context.
        /// </summary>
        /// <param name="model">Edit member view model.</param>
        /// <returns>Setting context.</returns>
        protected SettingContext GetEditMemberSettingContext(EditMemberViewModel model)
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
                settingContext.CustId = agentId;

                if (DownLinePermissionService.IsDirectDownLineMember(agentId, model.MemberId))
                {
                    settingContext.FormId = FormType.Edit;
                }
                else
                {
                    settingContext.FormId = FormType.EditIndirectMember;
                }
            }

            return settingContext;
        }

        #endregion Check permission

        #region Generate messages

        /// <summary>
        /// Re-update message content.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>
        /// Returns a list of result.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        protected Result GenerateMessages(ISettingResult result)
        {
            if ((int)result.ErrCode == Result.SuccessValue)
            {
                var updateResult = result as UpdateResult;
                if (updateResult != null)
                {
                    return GetUpdateSuccessMessage(updateResult);
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
        /// Get update message success.
        /// </summary>
        /// <param name="result">Insert or Update Result.</param>
        /// <returns>Result Information.</returns>
        protected Result GetUpdateSuccessMessage(UpdateResult result)
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

        protected string GetSportName(int sportType, int betType, bool isLive)
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
        protected string GetBetTypeName(int betType)
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
        protected string GetErrMsg(int errCode, int sportType)
        {
            var errMsg = ResourceManager.GetErrMsg(errCode);
            var virtualSportsTypeIds = SettingConfiguration.VirtualSportsTypeIds;
            if (virtualSportsTypeIds.Contains(sportType))
            {
                errMsg = errMsg.Replace(" - {3}", string.Empty);
            }

            return errMsg;
        }

        protected string GetSuccessMsg(int productId, string message)
        {
            var msgSuccess = message;
            if (productId == (int)ProductCode.Product_5)
            {
                msgSuccess = msgSuccess.Replace("{0} - ", string.Empty);
            }

            return msgSuccess;
        }

        #endregion Generate messages
    }
}