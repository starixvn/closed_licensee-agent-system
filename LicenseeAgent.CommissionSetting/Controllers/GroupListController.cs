namespace LicenseeAgent.CommissionSetting.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.CommissionSetting.Criteria;
    using LicenseeAgent.CommissionSetting.Criteria.GroupList;
    using LicenseeAgent.CommissionSetting.Entities;
    using LicenseeAgent.CommissionSetting.Enumerations;
    using LicenseeAgent.CommissionSetting.Services;
    using LicenseeAgent.CommissionSetting.ViewModels.GroupList;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Commission Group List.
    /// </summary>
    public class GroupListController : CommissionSettingController
    {
        #region Variables

        private readonly IGroupListService _groupListService;

        #endregion Variables

        #region Constructors

        public GroupListController()
            : this(null, null)
        {
        }

        public GroupListController(IGroupListService groupListService, ICoreService coreService)
            : base(coreService)
        {
            _groupListService = groupListService ?? new GroupListService();
        }

        #endregion Constructors

        #region Index

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>The View Result.</returns>
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the commission group.
        /// </summary>
        /// <returns>The JSON Result.</returns>
        [HttpPost]
        [ActionName("Index")]
        public JsonResult IndexPost()
        {
            var commissionGroups = _groupListService.GetCommissionGroups(new GroupCommissionSelCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId
            });

            var defaultGroup = commissionGroups.Where(p => p.IsDefault).FirstOrDefault();
            var defaultGroupId = defaultGroup != null ? defaultGroup.GroupCommId : 0;

            return Json(new
            {
                DefaultGroupId = defaultGroupId,
                List = commissionGroups,
                ProductColumns = OfferedColumns
            });
        }

        #endregion Index

        #region Add group

        /// <summary>
        /// Adds the specified default group identifier.
        /// </summary>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpGet]
        public ActionResult Add()
        {
            var columns = OfferedColumns;
            var productCommissions = columns.Select(x => new ProductCommission
            {
                ProductId = x.ProductId,
                Commission = 0
            }).ToList();
            var model = new CommissionGroupAddViewModel
            {
                ProductCommissions = productCommissions
            };

            return View(model);
        }

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CommissionGroupAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var columns = OfferedColumns;
                var criteria = new GroupCommInsCriteria
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    AdminId = CurrentSession.LoginUserId,
                    IP = Request.GetClientIp(),
                    GroupName = model.GroupName,
                    IsDefault = model.IsDefault,
                    SBComm = model.GetProductCommission(columns, ProductCode.Product_0),
                    RacingComm = model.GetProductCommission(columns, ProductCode.Product_1),
                    NGComm = model.GetProductCommission(columns, ProductCode.Product_2),
                    LCComm = model.GetProductCommission(columns, ProductCode.Product_4),
                    RNGComm = model.GetProductCommission(columns, ProductCode.Product_6)
                };
                _groupListService.InsertGroup(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ViewBag.Result = Las.CreatedSuccessfully;
                }
            }

            return View(model);
        }

        #endregion Add group

        #region Edit group

        /// <summary>
        /// Edits the specified group commission identifier.
        /// </summary>
        /// <param name="groupCommId">The group commission identifier.</param>
        /// <param name="defaultGroupId">The default group identifier.</param>
        /// <returns>
        /// Action Result.
        /// </returns>
        [HttpGet]
        public ActionResult Edit(int groupCommId, int? defaultGroupId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAccessDeniedPage();
            }

            var commissionGroup = _groupListService.GetCommissionGroup(new GroupCommGetByIdCriteria
            {
                GroupCommId = groupCommId,
                LicenseeId = CurrentSession.AgentInfo.LicenseeId
            });
            if (commissionGroup == null)
            {
                return RedirectToAccessDeniedPage();
            }

            var columns = OfferedColumns;
            var newModel = new CommissionGroupEditViewModel
            {
                GroupCommId = groupCommId,
                GroupName = commissionGroup.GroupName,
                DefaultGroupId = defaultGroupId,
                IsDefault = commissionGroup.IsDefault,
                ProductCommissions = commissionGroup.GenerateProductCommissions(columns)
            };
            return View(newModel);
        }

        /// <summary>
        /// Edits the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommissionGroupEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var columns = OfferedColumns;
                var criteria = new GroupCommUpdCriteria
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    AdminId = CurrentSession.LoginUserId,
                    IP = Request.GetClientIp(),
                    GroupName = model.GroupName,
                    GroupCommId = model.GroupCommId,
                    IsDefault = model.IsDefault,
                    SBComm = model.GetProductCommission(columns, ProductCode.Product_0),
                    RacingComm = model.GetProductCommission(columns, ProductCode.Product_1),
                    NGComm = model.GetProductCommission(columns, ProductCode.Product_2),
                    LCComm = model.GetProductCommission(columns, ProductCode.Product_4),
                    RNGComm = model.GetProductCommission(columns, ProductCode.Product_6),
                    IsUpdateRacing = model.HasProductCommission(ProductCode.Product_1),
                    IsUpdateNG = model.HasProductCommission(ProductCode.Product_2),
                    IsUpdateLC = model.HasProductCommission(ProductCode.Product_4),
                    IsUpdateRNG = model.HasProductCommission(ProductCode.Product_6),
                    IsResetDefault = model.IsResetDefault
                };
                _groupListService.UpdateGroup(criteria);
                if (criteria.ErrCode != Result.SuccessValue)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    ModelState.Remove("DefaultGroupId");
                    model.DefaultGroupId = model.IsResetDefault ? model.GroupCommId : model.DefaultGroupId;
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View(model);
        }

        #endregion Edit group

        #region Delete group

        [HttpGet]
        public ActionResult Delete(int groupCommId, bool isDefault)
        {
            var model = new CommissionGroupDeleteViewModel
            {
                GroupCommId = groupCommId,
                IsDefault = isDefault,
                Mode = DeleteMode.KeepCurrentCommission
            };

            return View(model);
        }

        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CommissionGroupDeleteViewModel model)
        {
            var criteria = new GroupCommDelCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                Currency = CurrentSession.AgentInfo.CurrencyId,
                AdminId = CurrentSession.LoginUserId,
                IP = Request.GetClientIp(),
                GroupCommId = model.GroupCommId,
                IsRemoveMemberComm = model.Mode == DeleteMode.ClearMemberComission,
                IsResetDefault = model.IsDefault
            };
            _groupListService.DeleteGroup(criteria);
            if (criteria.ErrCode != Result.SuccessValue)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
            }
            else
            {
                ViewBag.Result = Las.DeleteSuccessfully;
            }

            return View(model);
        }

        #endregion Delete group

        #region Common

        /// <summary>
        /// Checks the availability.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult CheckGroupAvailability(string groupName)
        {
            var criteria = new GroupCommCheckByNameCriteria
            {
                GroupName = groupName,
                LicenseeID = CurrentSession.AgentInfo.LicenseeId
            };
            _groupListService.CheckGroupAvailability(criteria);
            var errCode = criteria.IsAvailable ? Result.SuccessValue : ErrCode.Msg_2093;
            return JsonResult(errCode);
        }

        #endregion Common
    }
}