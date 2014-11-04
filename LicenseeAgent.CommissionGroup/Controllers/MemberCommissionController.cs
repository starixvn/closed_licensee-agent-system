namespace LicenseeAgent.CommissionGroup.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.CommissionGroup.Criteria;
    using LicenseeAgent.CommissionGroup.Services;
    using LicenseeAgent.CommissionGroup.Utils;
    using LicenseeAgent.CommissionGroup.ViewModels;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// MemberController class.
    /// </summary>
    [AllowedCommission]
    public class MemberController : BaseController
    {
        private readonly ICoreService _coreService;

        private readonly IMemberCommissionService _memberCommissionService;

        private readonly IGroupCommissionService _groupCommissionService;

        public MemberController()
            : this(null, null, null)
        {
        }

        public MemberController(
                IMemberCommissionService memberCommissionService, 
                IGroupCommissionService groupCommissionService,
                ICoreService coreService)
        {
            _memberCommissionService = memberCommissionService ?? new MemberCommissionService();
            _groupCommissionService = groupCommissionService ?? new GroupCommissionService();
            _coreService = coreService ?? new CoreService();
        }

        /// <summary>
        /// Index Member Commission.
        /// </summary>
        /// <param name="model">Member Commission View Model.</param>
        /// <returns>View Result.</returns>
        [HttpGet]
        public ActionResult Index(MemberCommissionViewModel model)
        {
            model.GroupCommissions = _groupCommissionService.GetGroupCommissions(
                new GroupCommissionSelCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                });
            UpdateOfferProduct(model);

            return View(model);
        }

        /// <summary>
        /// Get Member Commission.
        /// </summary>
        /// <param name="model">Member Commission View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        public JsonResult GetMemberCommissions(GetMemberCommissionsViewModel model)
        {
            var criteria = new LicenseeGetMemberCommissionCriteria
            {
                LicenseeId = CurrentSession.OnLicenseeId,
                AdminId = CurrentSession.LoginUserId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId,
                UserName = model.UserName,
                GroupCommId = model.GroupCommId,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };

            var memberCommissions = _memberCommissionService.GetMemberCommissions(criteria);

            return Json(new
            {
                List = memberCommissions,
                ItemsPerPage = model.PageSize,
                CurrentPage = model.PageIndex,
                TotalItem = criteria.TotalRows,
                ErrCode = Result.SuccessValue
            });
        }

        /// <summary>
        /// Move To Group.
        /// </summary>
        /// <returns>View result Move To Group.</returns>
        [HttpGet]
        public ActionResult MoveToGroup()
        {
            var model = new MemberCommissionViewModel
            {
                GroupCommissions = _groupCommissionService.GetGroupCommissions(
                new GroupCommissionSelCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                })
            };

            UpdateOfferProduct(model);

            return View(model);
        }

        /// <summary>
        /// Move To Group.
        /// </summary>
        /// <param name="model">Update Move To Group View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        public JsonResult MoveToGroup(MoveGroupViewModel model)
        {
            var criteria = new MemberMoveGroupCommissionCriteria 
            {
                LicenseeId = CurrentSession.OnLicenseeId,
                AdminId = CurrentSession.LoginUserId,
                Currency = CurrentSession.AgentInfo.CurrencyId,
                LanguageKey = CurrentSession.Language,
                OrginalCustomer = model.MemberIds,
                Ip = Request.GetClientIp(),
                GroupCommId = model.GroupCommId,
                IsUpdateNG = true
            };

            _memberCommissionService.MoveGroup(criteria);

            if (criteria.ErrCode != Result.SuccessValue)
            {
                return JsonResult(criteria.ErrCode);
            }

            return Json(new
            {
                ErrCode = criteria.ErrCode,
                ErrMsg = Las.MovedSuccessfully,
                Members = _memberCommissionService.GetEffectedMemberCommissions(new GetLicenseeMembersByIdsCriteria { ListCustId = model.MemberIds })
            });
        }

        /// <summary>
        /// Update Member Commission.
        /// </summary>
        /// <param name="model">Member commission view model.</param>
        /// <returns>View Result.</returns>
        [HttpGet]
        public ActionResult UpdateMemberCommission(MemberCommissionViewModel model)
        {
            UpdateOfferProduct(model);

            return View(model);
        }

        /// <summary>
        /// Update Member Commission.
        /// </summary>
        /// <param name="model">Update Member Commission View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        public ActionResult UpdateMemberCommission(UpdateMemberCommissionViewModel model)
        {
            var criteria = new LicenseeUpdateMemberCommissionCriteria
            {
                LicenseeId = CurrentSession.OnLicenseeId,
                AdminId = CurrentSession.LoginUserId,
                Currency = CurrentSession.AgentInfo.CurrencyId,
                LanguageKey = CurrentSession.Language,
                OrginalCustomer = model.MemberIds,
                Ip = Request.GetClientIp(),
                IsUpdateRacing = model.HasRacing,
                IsUpdateNG = model.HasNumberGame,
                IsUpdateLC = model.HasLiveCasino,
                IsUpdateRNG = model.HasRNGCasino,
                SBCommission = Commission.Combine(model.SBComm1, model.SBComm2, model.SBComm3),
                RacingCommission = Commission.Combine(model.RacingComm1, model.RacingComm2, model.RacingComm3),
                NGCommission = Commission.Combine(model.NGComm1, model.NGComm2, model.NGComm3),
                LCCommission = Commission.Combine(model.LCComm1, model.LCComm2, model.LCComm3),
                RNGCommission = Commission.Combine(model.RNGComm1, model.RNGComm2, model.RNGComm3)
            };

            _memberCommissionService.UpdateMember(criteria);

            if (criteria.ErrCode != Result.SuccessValue)
            {
                return JsonResult(criteria.ErrCode, GetErrMsg(criteria.ErrCode));
            }

            return Json(new
            {
                ErrCode = criteria.ErrCode,
                ErrMsg = Las.UpdateSuccessfully,
                Members = _memberCommissionService.GetEffectedMemberCommissions(new GetLicenseeMembersByIdsCriteria { ListCustId = model.MemberIds })
            });
        }

        /// <summary>
        /// Remove to group.
        /// </summary>
        /// <returns>View Result.</returns>
        [HttpGet]
        public ActionResult RemoveFromGroup()
        {
            return View();
        }

        /// <summary>
        /// Remove From Group.
        /// </summary>
        /// <param name="model">Remove Group View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        public ActionResult RemoveFromGroup(RemoveGroupViewModel model)
        {
            var criteria = new MemberRemoveGroupCommissionCriteria
            {
                LicenseeId = CurrentSession.OnLicenseeId,
                AdminId = CurrentSession.LoginUserId,
                Currency = CurrentSession.AgentInfo.CurrencyId,
                LanguageKey = CurrentSession.Language,
                OrginalCustomer = model.MemberIds,
                Ip = Request.GetClientIp(),
                IsRemoveMemberComm = model.SelectedConfirm
            };

            _memberCommissionService.MemberRemoveFromGroup(criteria);

            if (criteria.ErrCode != Result.SuccessValue)
            {
                return JsonResult(criteria.ErrCode);
            }

            return Json(new
            {
                ErrCode = criteria.ErrCode,
                ErrMsg = Las.UpdateSuccessfully,
                Members = _memberCommissionService.GetEffectedMemberCommissions(new GetLicenseeMembersByIdsCriteria { ListCustId = model.MemberIds })
            });
        }

        /// <summary>
        /// Update offer product.
        /// </summary>
        /// <param name="model">Member commission view model.</param>
        [NonAction]
        public void UpdateOfferProduct(MemberCommissionViewModel model)
        {
            model.HasSportbook = _coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, ProductCode.Product_0);
            model.HasLiveCasino = _coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, ProductCode.Product_4);
            model.HasNumberGame = _coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, ProductCode.Product_2);
            model.HasRacing = _coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, ProductCode.Product_1);
            model.HasRNGCasino = _coreService.HasProductOffered(CurrentSession.AgentInfo.LicenseeId, ProductCode.Product_6);

            var products = _coreService.GetProducts(CurrentSession.OnLicenseeId);
            var supportedProducts = new List<int>
            {
                (int)ProductCode.Product_0,
                (int)ProductCode.Product_1,
                (int)ProductCode.Product_2,
                (int)ProductCode.Product_4,
                (int)ProductCode.Product_6
            };

            model.ProductCount = products.Where(p => supportedProducts.Contains(p.ProductId)).Count();
        }
    }
}
