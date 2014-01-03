// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>

namespace LicenseeAgent.Site
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// MVC Bundle Configuration.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCore(bundles);
            RegisterComponents(bundles);
            RegisterCustomerSetting(bundles);
            RegisterPages(bundles);
            bundles.FileSetOrderList.Clear();
        }

        #region Core

        /// <summary>
        /// Registers the core.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCore(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/core").Include(
                "~/scripts/placeholder/jquery.placeholder.js",
                "~/scripts/blockui/jquery.blockui.js",
                "~/scripts/config.js",
                "~/scripts/utils.js",
                "~/scripts/language.js",
                "~/scripts/core.js",
                "~/scripts/popup.js"));

            bundles.Add(new LessBundle("~/css/core").Include(
                "~/content/sprite.less",
                "~/content/core.less"));
        }

        #endregion Core

        #region Components

        /// <summary>
        /// Registers the JAVASCRIPT component bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static void RegisterComponents(BundleCollection bundles)
        {
            // Jquery, Jquery-UI, Jquery Validation
            RegisterJquery(bundles);

            // Bootstrap, Bootstrap Datetimepicker.
            RegisterBootstrap(bundles);

            // Angular JS
            bundles.Add(new ScriptBundle("~/js/angular").Include(
                "~/scripts/angular/angular.js"));

            // Moment JS
            bundles.Add(new ScriptBundle("~/js/moment").Include(
                "~/scripts/MomentJS/moment.js",
                "~/scripts/MomentJS/moment-with-langs.js"));

            // Datetimepicker
            bundles.Add(new ScriptBundle("~/js/datetimepicker").Include(
                "~/scripts/moment-datepicker/moment-datepicker.js"));
            bundles.Add(new LessBundle("~/css/datetimepicker").Include(
                "~/content/moment-datepicker/datepicker.css"));

            // Fanex.Grid
            bundles.Add(new ScriptBundle("~/js/fanexgrid").Include(
                "~/scripts/fanexgrid/Fanex.Grid.js",
                "~/scripts/fanexgrid/Fanex.Pagination.js"));
            bundles.Add(new LessBundle("~/css/fanexgrid").Include(
                "~/content/fanexgrid/Fanex.Grid.css",
                "~/content/fanexgrid/Fanex.Pagination.css"));

            // Combobox
            bundles.Add(new ScriptBundle("~/js/bootstrap-select").Include(
                "~/scripts/bootstrap-select/bootstrap-select.js"));
            bundles.Add(new LessBundle("~/css/bootstrap-select").Include(
                "~/content/bootstrap-select/bootstrap-select.css"));

            // Contex Menu
            bundles.Add(new ScriptBundle("~/js/contextmenu").Include(
                "~/scripts/contextmenu/jquery.contextMenu.js"));
            bundles.Add(new LessBundle("~/css/contextmenu").Include(
                "~/content/contextmenu/jquery.contextMenu.css"));

            // Month Picker
            bundles.Add(new ScriptBundle("~/js/monthpicker").Include(
                "~/scripts/MonthPicker/monPickr-0.0.3.js"));
            bundles.Add(new LessBundle("~/css/monthpicker").Include(
                "~/scripts/MonthPicker/monPickr-0.0.3.css"));

            // For Bet List
            bundles.Add(new ScriptBundle("~/js/angulargrid").Include(
                "~/scripts/ng-grid/ng-grid.js",
                "~/scripts/ng-grid/ng-grid-flexible-height.js",
                "~/scripts/Angular/angular-sanitize.js"));
            bundles.Add(new LessBundle("~/css/angulargrid").Include(
                "~/content/ng-grid/ng-grid.css",
                "~/content/betlist/betlist.core.css",
                "~/content/betlist/betList.css"));
        }

        /// <summary>
        /// Registers the JQUERY.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterJquery(BundleCollection bundles)
        {
            // Jquery & Jquery validation
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                "~/scripts/jquery/jquery.js",
                "~/scripts/jquery-validate/jquery.validate.js",
                "~/scripts/jquery-validate/jquery.validate.unobtrusive.js",
                "~/scripts/jquery-form/jquery.form.min.js"));

            // Jquery UI
            bundles.Add(new ScriptBundle("~/js/jqueryui").Include(
                "~/scripts/jquery-ui/jquery-ui.js",
                "~/scripts/clickout/jquery.clickout.js"));
            bundles.Add(new LessBundle("~/css/jqueryui").Include(
                "~/content/jquery-ui/jquery-ui.css"));
        }

        /// <summary>
        /// Registers the Bootstrap.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBootstrap(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                "~/scripts/bootstrap/bootstrap-dropdown.js").Include(
                "~/scripts/bootstrap/bootstrap-select.js").Include(
                "~/scripts/bootstrap/bootstrap-switch.js",
                "~/scripts/bootstrap/bootstrap-paginator.js",
                "~/scripts/bootstrap/bootstrap-tab.js"));
            bundles.Add(new LessBundle("~/css/bootstrap").Include(
                "~/content/bootstrap/bootstrap.less"));
        }

        #endregion Components

        #region Pages

        /// <summary>
        /// Registers the pages.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterPages(BundleCollection bundles)
        {
            RegisterLoginPage(bundles);
            RegisterHomePage(bundles);
            RegisterAccountPage(bundles);
            RegisterCustomerListPage(bundles);
            RegisterTotalBetsPages(bundles);
            RegisterPluginControl(bundles);
        }

        /// <summary>
        /// Registers the login page.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLoginPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/login").Include(
                "~/scripts/login.js"));
            bundles.Add(new LessBundle("~/css/login").Include(
                "~/content/language.less",
                "~/content/login.less"));
            bundles.Add(new LessBundle("~/css/login/internal").Include(
                "~/content/internal.less"));
        }

        /// <summary>
        /// Registers the home page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterHomePage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/home").Include(
                "~/scripts/HeadJS/head.js",
                "~/scripts/dotdotdot/jquery.dotdotdot.js",
                "~/scripts/menu.js",
                "~/scripts/header.js",
                "~/Views/AccountInformation/Resources/index.js",
                "~/scripts/scroller/jquery.nicescroll.min.js"));

            bundles.Add(new ScriptBundle("~/js/home/internal").Include(
                "~/Views/Internal/Resources/Header.js"));

            bundles.Add(new LessBundle("~/css/home").Include(
                "~/Views/Home/Resources/index.less",
                "~/Views/AccountInformation/Resources/index.less"));
        }

        /// <summary>
        /// Register customer setting bundle.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterCustomerSetting(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/customersetting").IncludeDirectory(
                "~/Views/Account/CustSetting/Resources/scripts", "*.js", true).Include(
                "~/scripts/autoNumeric/autoNumeric.js").Include(
                "~/scripts/poshytip/jquery.poshytip.js"));
            bundles.Add(new LessBundle("~/css/customersetting").Include(
                "~/Views/Account/Resources/MemberInfo.less").Include(
                "~/content/poshytip/tip-twitter/tip-twitter.css"));

            bundles.Add(new ScriptBundle("~/js/customersetting.sportsbook").IncludeDirectory(
                "~/Views/Account/CustSetting/Sportsbook/scripts", "*.js", true));
            bundles.Add(new LessBundle("~/css/customersetting.sportsbook").Include(
                "~/Views/Account/CustSetting/Sportsbook/styles/fanex.sportsbookSetting.less"));
        }

        /// <summary>
        /// Registers the account page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountPage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/account").Include(
                "~/Views/Account/Resources/account.less"));
        }

        /// <summary>
        /// Registers the agent list page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerListPage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/customerlist").Include(
                "~/Views/Customer/Resources/customerlist.less"));

            bundles.Add(new ScriptBundle("~/js/customerlist").Include(
                "~/Views/Customer/Resources/customerlist.js"));
        }

        /// <summary>
        /// Registers the total bets pages bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsPages(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/totalbets-soccer").Include(
                "~/Views/TotalBets/Resources/soccer.css"));

            bundles.Add(new ScriptBundle("~/js/totalbets-soccer").Include(
                "~/Views/TotalBets/Resources/soccer.js"));

            bundles.Add(new ScriptBundle("~/js/totalbets-basketball").Include(
                "~/Views/TotalBets/Resources/basketball.js"));

            bundles.Add(new ScriptBundle("~/js/totalbets-OtherSports").Include(
                "~/Views/TotalBets/Resources/OtherSports.js"));

            bundles.Add(new ScriptBundle("~/js/totalbets-outright").Include(
                "~/Views/TotalBets/Resources/outright.js"));
        }

        /// <summary>
        /// Registers the plugin bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterPluginControl(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/plugin").Include(
                "~/content/fanex.multiSelect/fanex.multiSelect.css").Include(
                "~/content/refresh.countdown/plugin.refresh.countdown.less"));

            bundles.Add(new ScriptBundle("~/js/plugin").Include(
                "~/scripts/fanex.multiSelect/fanex.multiSelect.js").Include(
                "~/scripts/refresh.countdown/plugin.refresh.countdown.js"));
        }

        #endregion Pages
    }
}