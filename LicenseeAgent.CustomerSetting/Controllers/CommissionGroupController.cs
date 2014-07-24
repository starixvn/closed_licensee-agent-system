namespace LicenseeAgent.CustomerSetting.Controllers
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
    using LicenseeAgent.CustomerSetting.Core;
    using LicenseeAgent.CustomerSetting.Criteria;
    using LicenseeAgent.CustomerSetting.Entities;
    using LicenseeAgent.CustomerSetting.Enumerations;
    using LicenseeAgent.CustomerSetting.Services;
    using LicenseeAgent.CustomerSetting.ViewModels;
    using LicenseeAgent.Resources;
    using LicenseeAgent.Taiwan.Actions;
    using LicenseeAgent.Taiwan.Services;
    using LicenseeAgent.Web.Controllers;
    using LicenseeAgent.Web.Entities;
    using LicenseeAgent.Web.Filters;
    using LicenseeAgent.Web.Mvc;
    using LicenseeAgent.Web.Services;
    using Newtonsoft.Json;

    /// <summary>
    /// Sport books setting controller.
    /// </summary>
    public class CommissionGroupController : BaseController
    {
        private static List<int> _virtualSportsTypeIds = SettingConfigurations.VirtualSportsTypeIds;
        private readonly IMemberService _memberService;
        private readonly IDownLinePermissionService _downLinePermissionService;
        private readonly ICoreService _coreService;
        private readonly Services.ISettingService _numberGameService;
        private readonly Services.ISettingService _sportsBookService;
        private readonly Services.VirtualSports.SettingService _virtualSportsService;
        private readonly ICustomerSettingService _customerSettingService;

        public CommissionGroupController()
            : this(null, null, null, null, null, null, null)
        {
        }

        public CommissionGroupController(
            IMemberService memberService,
            ICoreService coreService,
            ICustomerSettingService customerSettingService,
            Services.ISettingService numberGameService,
            Services.ISettingService sportsBookService,
            Services.VirtualSports.SettingService virtualSportsService,
            IDownLinePermissionService downLinePermissionService)
        {
            _memberService = memberService ?? new MemberService();
            _coreService = coreService ?? new CoreService();
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            _numberGameService = numberGameService ?? new Services.NumberGame.SettingService();
            _sportsBookService = sportsBookService ?? new Services.SportsBook.SettingService();
            _virtualSportsService = virtualSportsService ?? new Services.VirtualSports.SettingService();
            _downLinePermissionService = downLinePermissionService ?? new DownLinePermissionService();
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCommissionGroup()
        {
            return Json(new
            {
                result = string.Empty
            });
        }
        /*
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Edit()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Delete()
        {
            return View();
        }
         * */
    }
}