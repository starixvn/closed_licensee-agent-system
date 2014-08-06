namespace LicenseeAgent.CustomerSetting.Controllers
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using LicenseeAgent.Core.Entities;
    using LicenseeAgent.Core.Utility;
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Mvc;

    /// <summary>
    /// MemberCommissionController class.
    /// </summary>
    public class MemberCommissionController : BaseController
    {
        private readonly IMemberCommissionService _memberCommissionService;

        private readonly IGroupCommissionService _groupCommissionService;

        public MemberCommissionController()
            : this(null, null)
        {
        }

        public MemberCommissionController(IMemberCommissionService memberCommissionService, IGroupCommissionService groupCommissionService)
        {
            _memberCommissionService = memberCommissionService ?? new MemberCommissionService();
            _groupCommissionService = groupCommissionService ?? new GroupCommissionService();
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
                new LicenseeGetGroupCommissionCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId
                });

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
            var groupCommissions = _groupCommissionService.GetGroupCommissions(
                new LicenseeGetGroupCommissionCriteria
                {
                    LicenseeId = CurrentSession.OnLicenseeId,
                });

            return View(groupCommissions);
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
        /// <returns>View Result.</returns>
        [HttpGet]
        public ActionResult UpdateMemberCommission()
        {
            return View();
        }

        /// <summary>
        /// Update Member Commission.
        /// </summary>
        /// <param name="model">Update Member Commission View Model.</param>
        /// <returns>Member Commission List.</returns>
        [HttpPost]
        public ActionResult UpdateMemberCommission(UpdateMemberCommissionViewModel model)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("en-US");

            var criteria = new LicenseeUpdateMemberCommissionCriteria
            {
                LicenseeId = CurrentSession.OnLicenseeId,
                AdminId = CurrentSession.LoginUserId,
                LanguageKey = CurrentSession.Language,
                OrginalCustomer = model.MemberIds,
                Ip = Request.GetClientIp(),
                IsUpdateNG = true,
                SBCommission = Convert.ToDecimal(string.Format(cultureInfo, Constant.FormatCommission, model.SBComm1, model.SBComm2, model.SBComm3)) / Constant.StepMulDiv,
                NGCommission = Convert.ToDecimal(string.Format(cultureInfo, Constant.FormatCommission, model.NGComm1, model.NGComm2, model.NGComm3)) / Constant.StepMulDiv
            };

            _memberCommissionService.UpdateMember(criteria);

            if (criteria.ErrCode != Result.SuccessValue)
            {
                var errorMessage = GetErrMsg(criteria.ErrCode);
                return JsonResult(criteria.ErrCode, string.Format(errorMessage, Constant.MaxComm));
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
    }
}
