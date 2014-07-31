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
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
    }
}