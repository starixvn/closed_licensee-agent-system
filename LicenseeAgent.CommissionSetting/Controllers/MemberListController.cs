namespace LicenseeAgent.CommissionSetting.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.CommissionSetting.Criteria;
    using LicenseeAgent.CommissionSetting.Criteria.MemberList;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.CommissionSetting.Enumerations;
    using LicenseeAgent.CommissionSetting.Services;
    using LicenseeAgent.CommissionSetting.ViewModels.MemberList;
    using LicenseeAgent.CommissionSetting.ViewResults;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// MemberController class.
    /// </summary>
    public class MemberListController : CommissionSettingController
    {
        #region Variables

        private readonly IMemberListService _memberListService;
        private readonly IGroupListService _groupListService;

        #endregion Variables

        #region Constructors

        public MemberListController()
            : this(null, null, null)
        {
        }

        public MemberListController(
            IMemberListService memberCommissionService,
            IGroupListService groupCommissionService,
            ICoreService coreService)
            : base(coreService)
        {
            _memberListService = memberCommissionService ?? new MemberListService();
            _groupListService = groupCommissionService ?? new GroupListService();
        }

        #endregion Constructors

        #region Index

        /// <summary>
        /// Index Member Commission.
        /// </summary>
        /// <param name="model">Member Commission View Model.</param>
        /// <returns>View Result.</returns>
        [HttpGet]
        public ActionResult Index(IndexViewModel model)
        {
            var commissionGroups = _groupListService.GetCommissionGroups(
                new GroupCommissionSelCriteria
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    CurrencyId = CurrentSession.AgentInfo.CurrencyId
                });
            commissionGroups.Insert(0, new CommissionGroup(-1, Las.All));
            commissionGroups.Insert(1, new CommissionGroup(0, Las.None));
            model.CommissionGroups = commissionGroups;
            return View(model);
        }

        /// <summary>
        /// Get Member Commission.
        /// </summary>
        /// <param name="model">Member Commission View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        [ActionName("Index")]
        public JsonResult IndexPost(IndexViewModel model)
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

            var memberCommissions = _memberListService.GetMemberCommissions(criteria);
            return Json(new
            {
                List = memberCommissions,
                ItemsPerPage = model.PageSize,
                CurrentPage = model.PageIndex,
                TotalItem = criteria.TotalRows,
                ProductColumns = OfferedColumns
            });
        }

        #endregion Index

        #region Move to group

        /// <summary>
        /// Move To Group.
        /// </summary>
        /// <param name="custIds">The customer ids.</param>
        /// <returns>
        /// View result Move To Group.
        /// </returns>
        [HttpGet]
        public ActionResult MoveToGroup(string custIds)
        {
            if (string.IsNullOrEmpty(custIds))
            {
                return RedirectToAccessDeniedPage();
            }

            var commissionGroups = _groupListService.GetCommissionGroups(
                new GroupCommissionSelCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                    CurrencyId = CurrentSession.AgentInfo.CurrencyId
                });
            var model = new MoveToGroupViewModel
            {
                CommissionGroups = commissionGroups,
                ProductColumns = OfferedColumns,
                CustIds = custIds
            };

            return View(model);
        }

        /// <summary>
        /// Move To Group.
        /// </summary>
        /// <param name="model">Update Move To Group View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveToGroup(MoveToGroupViewModel model)
        {
            var productColumns = OfferedColumns;
            var commissionGroups = _groupListService.GetCommissionGroups(
                new GroupCommissionSelCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                    CurrencyId = CurrentSession.AgentInfo.CurrencyId
                });
            model.CommissionGroups = commissionGroups;
            model.ProductColumns = productColumns;

            if (ModelState.IsValid)
            {
                var criteria = new MemberMoveGroupCommissionCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                    AdminId = CurrentSession.LoginUserId,
                    Currency = CurrentSession.AgentInfo.CurrencyId,
                    OrginalCustomer = model.CustIds,
                    Ip = Request.GetClientIp(),
                    GroupCommId = model.GroupCommId,
                    IsUpdateNG = productColumns.Where(x => x.ProductId == (int)ProductCode.Product_2).Any(),
                    IsUpdateRacing = productColumns.Where(x => x.ProductId == (int)ProductCode.Product_1).Any(),
                    IsUpdateLC = productColumns.Where(x => x.ProductId == (int)ProductCode.Product_4).Any(),
                    IsUpdateRNG = productColumns.Where(x => x.ProductId == (int)ProductCode.Product_6).Any()
                };

                _memberListService.MoveGroup(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    var affectedMemberCommissions = _memberListService.GetMemberCommissions(new GetLicenseeMembersByIdsCriteria
                    {
                        ListCustId = criteria.EffectedCustomers
                    });
                    ViewBag.Result = new UpdateMemberCommissionViewResult
                    {
                        Message = Las.MovedSuccessfully,
                        MemberCommissions = affectedMemberCommissions
                    };
                }
            }

            return View(model);
        }

        #endregion Move to group

        #region Remove from group

        /// <summary>
        /// Remove to group.
        /// </summary>
        /// <param name="custIds">The customer ids.</param>
        /// <returns>
        /// View Result.
        /// </returns>
        [HttpGet]
        public ActionResult RemoveFromGroup(string custIds)
        {
            if (string.IsNullOrEmpty(custIds))
            {
                return RedirectToAccessDeniedPage();
            }

            var model = new RemoveFromGroupViewModel
            {
                CustIds = custIds
            };
            return View(model);
        }

        /// <summary>
        /// Remove From Group.
        /// </summary>
        /// <param name="model">Remove Group View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromGroup(RemoveFromGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new MemberRemoveGroupCommissionCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                    AdminId = CurrentSession.LoginUserId,
                    Currency = CurrentSession.AgentInfo.CurrencyId,
                    OrginalCustomer = model.CustIds,
                    Ip = Request.GetClientIp(),
                    IsRemoveMemberComm = model.Mode == DeleteMode.ClearMemberComission
                };

                _memberListService.MemberRemoveFromGroup(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    var affectedMemberCommissions = _memberListService.GetMemberCommissions(new GetLicenseeMembersByIdsCriteria
                    {
                        ListCustId = criteria.EffectedCustomers
                    });
                    ViewBag.Result = new UpdateMemberCommissionViewResult
                    {
                        Message = Las.UpdateSuccessfully,
                        MemberCommissions = affectedMemberCommissions
                    };
                }
            }

            return View(model);
        }

        #endregion Remove from group

        #region Update member commission

        /// <summary>
        /// Update Member Commission.
        /// </summary>
        /// <param name="custIds">The customer ids.</param>
        /// <returns>
        /// View Result.
        /// </returns>
        [HttpGet]
        public ActionResult UpdateMemberCommission(string custIds)
        {
            if (string.IsNullOrEmpty(custIds))
            {
                return RedirectToAccessDeniedPage();
            }

            var columns = OfferedColumns;
            var productCommissions = columns.Select(x => new ProductCommission
            {
                ProductId = x.ProductId,
                HeaderResource = x.HeaderResource,
                Commission = 0
            }).ToList();
            var model = new UpdateMemberCommissionViewModel
            {
                CustIds = custIds,
                ProductCommissions = productCommissions
            };

            return View(model);
        }

        /// <summary>
        /// Update Member Commission.
        /// </summary>
        /// <param name="model">Update Member Commission View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMemberCommission(UpdateMemberCommissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var columns = OfferedColumns;
                var criteria = new LicenseeUpdateMemberCommissionCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                    AdminId = CurrentSession.LoginUserId,
                    Currency = CurrentSession.AgentInfo.CurrencyId,
                    OrginalCustomer = model.CustIds,
                    Ip = Request.GetClientIp(),
                    IsUpdateRacing = model.HasProductCommission(ProductCode.Product_1),
                    IsUpdateNG = model.HasProductCommission(ProductCode.Product_2),
                    IsUpdateLC = model.HasProductCommission(ProductCode.Product_4),
                    IsUpdateRNG = model.HasProductCommission(ProductCode.Product_6),
                    SBCommission = model.GetProductCommission(columns, ProductCode.Product_0),
                    RacingCommission = model.GetProductCommission(columns, ProductCode.Product_1),
                    NGCommission = model.GetProductCommission(columns, ProductCode.Product_2),
                    LCCommission = model.GetProductCommission(columns, ProductCode.Product_4),
                    RNGCommission = model.GetProductCommission(columns, ProductCode.Product_6)
                };

                _memberListService.UpdateMember(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    var affectedMemberCommissions = _memberListService.GetMemberCommissions(new GetLicenseeMembersByIdsCriteria
                    {
                        ListCustId = criteria.EffectedCustomers
                    });
                    ViewBag.Result = new UpdateMemberCommissionViewResult
                    {
                        Message = Las.UpdateSuccessfully,
                        MemberCommissions = affectedMemberCommissions
                    };
                }
            }

            return View(model);
        }

        #endregion Update member commission
    }
}