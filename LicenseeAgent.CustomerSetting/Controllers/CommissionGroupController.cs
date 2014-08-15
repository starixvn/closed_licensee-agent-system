namespace LicenseeAgent.CustomerSetting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Enumerations;
    using LicenseeAgent.Core.Services;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Enumerations;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// Sport books setting controller.
    /// </summary>
    [AllowedCommission]
    public class CommissionGroupController : BaseController
    {
        private readonly IGroupCommissionService _groupCommissionService;
        private readonly ICoreService _coreService;

        public CommissionGroupController()
            : this(null, null)
        {
        }

        public CommissionGroupController(IGroupCommissionService groupCommissionService, ICoreService coreService)
        {
            _groupCommissionService = groupCommissionService ?? new GroupCommissionService();
            _coreService = coreService ?? new CoreService();
        }

        #region List Commission Group

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>The View Result.</returns>
        [HttpGet]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public ViewResult Index()
        {
            var licenseeId = CurrentSession.AgentInfo.LicenseeId;
            var model = new CommissionGroupBaseViewModel
            {
                HasSportbook = _coreService.HasProductOffered(licenseeId, Product.Product_0),
                HasLiveCasino = _coreService.HasProductOffered(licenseeId, Product.Product_4),
                HasNumberGame = _coreService.HasProductOffered(licenseeId, Product.Product_2),
                HasRacing = _coreService.HasProductOffered(licenseeId, Product.Product_1),
                HasRNGCasino = _coreService.HasProductOffered(licenseeId, Product.Product_6)
            };
            return View(model);
        }

        /// <summary>
        /// Gets the commission group.
        /// </summary>
        /// <returns>The JSON Result.</returns>
        [HttpPost]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public JsonResult CommissionGroupList()
        {
            var result = _groupCommissionService.GetGroupCommissions(new GroupCommissionSelCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                CurrencyId = CurrentSession.AgentInfo.CurrencyId
            });

            var defaultGroup = result.Where(p => p.IsDefault).FirstOrDefault();
            var defaultGroupId = defaultGroup != null ? defaultGroup.GroupCommId : 0;

            return Json(new
            {
                ErrCode = Result.SuccessValue,
                DefaultGroupId = defaultGroupId,
                List = result
            });
        }

        #endregion List Commission Group

        #region Add

        /// <summary>
        /// Adds the specified default group identifier.
        /// </summary>
        /// <param name="defaultGroupId">The default group identifier.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public ActionResult Add(int defaultGroupId)
        {
            var licenseeId = CurrentSession.AgentInfo.LicenseeId;
            var model = new CommissionGroupViewModel
            {
                HasSportbook = _coreService.HasProductOffered(licenseeId, Product.Product_0),
                HasLiveCasino = _coreService.HasProductOffered(licenseeId, Product.Product_4),
                HasNumberGame = _coreService.HasProductOffered(licenseeId, Product.Product_2),
                HasRacing = _coreService.HasProductOffered(licenseeId, Product.Product_1),
                HasRNGCasino = _coreService.HasProductOffered(licenseeId, Product.Product_6),
                ProductCount = CountProductSupportCommission(licenseeId),
                DefaultGroupId = defaultGroupId
            };
            return View(model);
        }

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public ActionResult Add(CommissionGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new GroupCommInsCriteria
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    AdminId = CurrentSession.LoginUserId,
                    IP = Request.GetClientIp(),
                    Langkey = CurrentSession.Language,
                    GroupName = model.GroupName,
                    IsDefault = model.IsDefault,
                    SBComm = model.HasSportbook ? CombineComission(model.SBComm1, model.SBComm2, model.SBComm3) : 0,
                    RacingComm = model.HasRacing ? CombineComission(model.RCComm1, model.RCComm2, model.RCComm3) : 0,
                    NGComm = model.HasNumberGame ? CombineComission(model.NGComm1, model.NGComm2, model.NGComm3) : 0,
                    LCComm = model.HasLiveCasino ? CombineComission(model.LCComm1, model.LCComm2, model.LCComm3) : 0,
                    RNGComm = model.HasRNGCasino ? CombineComission(model.RNGComm1, model.RNGComm2, model.RNGComm3) : 0,
                    IsResetDefault = model.IsResetDefault
                };
                _groupCommissionService.GroupCommIns(criteria);
                if (criteria.ErrCode != 0)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    model.DefaultGroupId = model.IsResetDefault ? model.GroupCommId : model.DefaultGroupId;
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View(model);
        }

        #endregion Add

        #region Edit

        /// <summary>
        /// Edits the specified group commission identifier.
        /// </summary>
        /// <param name="groupCommId">The group commission identifier.</param>
        /// <param name="defaultGroupId">The default group identifier.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public ActionResult Edit(int groupCommId, int defaultGroupId = 0)
        {
            var licenseeId = CurrentSession.AgentInfo.LicenseeId;
            var result = _groupCommissionService.GetGroupCommissionById(new GroupCommGetByIdCriteria
            {
                GroupCommId = groupCommId,
                LicenseeId = licenseeId
            });

            if (result == null)
            {
                return RedirectToAccessDeniedPage();
            }

            var model = new CommissionGroupViewModel
            {
                GroupName = result.GroupName,
                GroupCommId = groupCommId,
                IsDefault = result.IsDefault,
                HasSportbook = _coreService.HasProductOffered(licenseeId, Product.Product_0),
                HasLiveCasino = _coreService.HasProductOffered(licenseeId, Product.Product_4),
                HasNumberGame = _coreService.HasProductOffered(licenseeId, Product.Product_2),
                HasRacing = _coreService.HasProductOffered(licenseeId, Product.Product_1),
                HasRNGCasino = _coreService.HasProductOffered(licenseeId, Product.Product_6),
                LCComm1 = GenCommission(result.LCComm, 0),
                LCComm2 = GenCommission(result.LCComm, 1),
                LCComm3 = GenCommission(result.LCComm, 2),
                NGComm1 = GenCommission(result.NGComm, 0),
                NGComm2 = GenCommission(result.NGComm, 1),
                NGComm3 = GenCommission(result.NGComm, 2),
                RCComm1 = GenCommission(result.RacingComm, 0),
                RCComm2 = GenCommission(result.RacingComm, 1),
                RCComm3 = GenCommission(result.RacingComm, 2),
                RNGComm1 = GenCommission(result.RNGComm, 0),
                RNGComm2 = GenCommission(result.RNGComm, 1),
                RNGComm3 = GenCommission(result.RNGComm, 2),
                SBComm1 = GenCommission(result.SBComm, 0),
                SBComm2 = GenCommission(result.SBComm, 1),
                SBComm3 = GenCommission(result.SBComm, 2),
                ProductCount = CountProductSupportCommission(licenseeId),
                DefaultGroupId = defaultGroupId
            };

            return View(model);
        }

        /// <summary>
        /// Edits the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public ActionResult Edit(CommissionGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new GroupCommUpdCriteria
                {
                    LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                    AdminId = CurrentSession.LoginUserId,
                    IP = Request.GetClientIp(),
                    Langkey = CurrentSession.Language,
                    GroupName = model.GroupName,
                    GroupCommId = model.GroupCommId,
                    IsDefault = model.IsDefault,
                    SBComm = model.HasSportbook ? CombineComission(model.SBComm1, model.SBComm2, model.SBComm3) : 0,
                    RacingComm = model.HasRacing ? CombineComission(model.RCComm1, model.RCComm2, model.RCComm3) : 0,
                    NGComm = model.HasNumberGame ? CombineComission(model.NGComm1, model.NGComm2, model.NGComm3) : 0,
                    LCComm = model.HasLiveCasino ? CombineComission(model.LCComm1, model.LCComm2, model.LCComm3) : 0,
                    RNGComm = model.HasRNGCasino ? CombineComission(model.RNGComm1, model.RNGComm2, model.RNGComm3) : 0,
                    IsUpdateLC = model.HasLiveCasino,
                    IsUpdateNG = model.HasNumberGame,
                    IsUpdateRacing = model.HasRacing,
                    IsUpdateRNG = model.HasRNGCasino,
                    IsResetDefault = model.IsResetDefault
                };
                _groupCommissionService.GroupCommUpd(criteria);
                if (criteria.ErrCode != 0)
                {
                    ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
                }
                else
                {
                    model.DefaultGroupId = model.IsResetDefault ? model.GroupCommId : model.DefaultGroupId;
                    ViewBag.Result = Las.UpdateSuccessfully;
                }
            }

            return View(model);
        }

        #endregion Edit

        #region Delete

        [HttpGet]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public ActionResult Delete(int groupCommId, bool isDefault = false)
        {
            var model = new CommissionGroupDeleteViewModel
            {
                GroupCommId = groupCommId,
                Mode = DeleteMode.KeepCurrentCommission,
                IsDefault = isDefault
            };
            return View(model);
        }

        /// <summary>
        /// Deletes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [AccountType(AccountType.Admin)]
        [LegendType(LegendType.Deposit)]
        public ActionResult Delete(CommissionGroupDeleteViewModel model)
        {
            var criteria = new GroupCommDelCriteria
            {
                LicenseeId = CurrentSession.AgentInfo.LicenseeId,
                Currency = CurrentSession.AgentInfo.CurrencyId,
                AdminId = CurrentSession.LoginUserId,
                IP = Request.GetClientIp(),
                Langkey = CurrentSession.Language,
                GroupCommId = model.GroupCommId,
                IsRemoveMemberComm = model.Mode == DeleteMode.ClearMemberComission,
                IsResetDefault = model.IsDefault
            };
            _groupCommissionService.GroupCommDel(criteria);

            if (criteria.ErrCode != 0)
            {
                ModelState.AddModelError(string.Empty, GetErrMsg(criteria.ErrCode));
            }
            else
            {
                ViewBag.Result = Las.DeleteSuccessfully;
            }

            return View(model);
        }

        #endregion Delete

        /// <summary>
        /// Checks the availability.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult CheckAvailability(string groupName)
        {
            var criteria = new GroupCommCheckByNameCriteria
            {
                GroupName = groupName,
                LicenseeID = CurrentSession.AgentInfo.LicenseeId
            };
            _groupCommissionService.GroupCommCheckByName(criteria);
            return Json(new
            {
                IsAvailable = criteria.IsAvailable
            });
        }

        /// <summary>
        /// Gens the commission.
        /// </summary>
        /// <param name="commission">The commission.</param>
        /// <param name="position">The position.</param>
        /// <returns>Commission Number.</returns>
        private int GenCommission(decimal commission, int position)
        {
            var tempCommission = Convert.ToInt32(commission * 10000);
            switch (position)
            {
                case 0:
                    return tempCommission / 100;
                case 1:
                    return (tempCommission % 100) / 10;
                case 2:
                    return tempCommission % 10;
            }

            return 0;
        }

        private decimal CombineComission(int commission1, int commission2, int commission3)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");
            return Convert.ToDecimal(string.Format(cultureInfo, Constant.FormatCommission, commission1, commission2, commission3)) / Constant.StepMulDiv;
        }

        /// <summary>
        /// Counts the product support commission.
        /// </summary>
        /// <param name="licenseeId">The licensee identifier.</param>
        /// <returns>Number of product.</returns>
        private int CountProductSupportCommission(int licenseeId)
        {
            var listProducts = _coreService.GetProducts(licenseeId);
            var listSupportedProducts = new List<int>
            {
                (int)Product.Product_0,
                (int)Product.Product_1,
                (int)Product.Product_2,
                (int)Product.Product_4,
                (int)Product.Product_6
            };
            return listProducts.Where(p => listSupportedProducts.Contains(p.ProductId)).Count();
        }
    }
}