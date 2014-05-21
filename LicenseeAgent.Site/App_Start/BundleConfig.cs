// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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
                "~/scripts/headjs/head.core.js",
                "~/scripts/mutate/events.js",
                "~/scripts/mutate/mutate.js",
                "~/scripts/nicescroll/jquery.nicescroll.js",
                "~/scripts/core.js",
                "~/scripts/utility.js",
                "~/scripts/popup.js",
                "~/Views/Report/Resources/report-base.js"));
            bundles.Add(new LessBundle("~/css/core").Include(
                "~/content/sprite.less",
                "~/content/core.less"));

            bundles.Add(new ScriptBundle("~/js/language").Include(
                "~/scripts/language.js").IncludeDirectory(
                "~/scripts/ResourcesJs", "*.js", true));
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
                "~/scripts/angularjs/angular.js"));

            // Moment JS
            bundles.Add(new ScriptBundle("~/js/moment").Include(
                "~/scripts/MomentJS/moment-with-langs.js"));

            // Datetimepicker
            bundles.Add(new ScriptBundle("~/js/datetimepicker").Include(
                "~/scripts/moment-datepicker/moment-datepicker.js"));
            bundles.Add(new LessBundle("~/css/datetimepicker").Include(
                "~/content/moment-datepicker/datepicker.css"));

            // Fanex.Grid
            bundles.Add(new ScriptBundle("~/js/fanexgrid").IncludeDirectory(
                "~/scripts/fanexgrid", "*.js", true));
            bundles.Add(new LessBundle("~/css/fanexgrid").Include(
                "~/content/fanexgrid/Fanex.Grid.css",
                "~/content/fanexgrid/Fanex.Pagination.css",
                "~/content/customizeFanexGrid.less"));

            // Context Menu
            bundles.Add(new ScriptBundle("~/js/contextmenu").Include(
                "~/scripts/contextmenu/context.js"));

            // Auto numeric
            bundles.Add(new ScriptBundle("~/js/autonumeric").Include(
                "~/scripts/autonumeric/autonumeric.js"));
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
                "~/scripts/jquery-form/jquery.form.js"));
        }

        /// <summary>
        /// Registers the Bootstrap.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBootstrap(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                "~/scripts/bootstrap/bootstrap-modal.js",
                "~/scripts/bootstrap/bootstrap-dropdown.js",
                "~/scripts/bootstrap/bootstrap-select.js",
                "~/scripts/bootstrap/bootstrap-switch.js",
                "~/scripts/bootstrap/bootstrap-tooltip.js",
                "~/scripts/bootstrap/bootstrap-popover.js",
                "~/scripts/bootstrap/bootstrap-paginator.js",
                "~/scripts/bootstrap/bootstrap-tab.js",
                "~/scripts/bootstrap/bootstrap-typeahead.js"));
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
            RegisterErrorPage(bundles);
            RegisterLoginPage(bundles);
            RegisterHomePage(bundles);
            RegisterAccountPage(bundles);
            RegisterCustomerListPage(bundles);
            RegisterBetListPage(bundles);
            RegisterReportPage(bundles);
            RegisterTotalBetsPages(bundles);
            RegisterTransferPage(bundles);
            RegisterPluginControl(bundles);
            RegisterCreditBalancePage(bundles);
            RegisterMessagePage(bundles);
            RegisterToolPage(bundles);
        }

        private static void RegisterErrorPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/error/UnderMaintenance").Include(
                "~/views/error/resources/under-maintenance-error.js"));
            bundles.Add(new LessBundle("~/css/error").Include(
                "~/views/error/resources/Index.less"));
        }

        /// <summary>
        /// Registers the login page.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterLoginPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/login").Include(
                "~/scripts/cookie/jquery.cookie.js",
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
                "~/scripts/headjs/head.core.js",
                "~/scripts/dotdotdot/jquery.dotdotdot.js",
                "~/scripts/cookie/jquery.cookie.js",
                "~/scripts/marquee/jquery.marquee.js",
                "~/views/home/resources/home-menu.js",
                "~/views/home/resources/home-header.js"));

            bundles.Add(new ScriptBundle("~/js/home/accountinfo").Include(
                "~/views/accountinformation/resources/account-information.js"));

            bundles.Add(new ScriptBundle("~/js/home/internal").Include(
                "~/views/internal/resources/internal-header.js"));

            bundles.Add(new LessBundle("~/css/home").Include(
                "~/views/home/resources/home.less"));

            bundles.Add(new LessBundle("~/css/home/accountinfo").Include(
                "~/views/accountinformation/resources/account-information.less"));
        }

        /// <summary>
        /// Register customer setting bundle.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void RegisterCustomerSetting(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/customersetting").IncludeDirectory(
                "~/views/customersetting/resources/scripts", "*.js", true));
            bundles.Add(new ScriptBundle("~/js/customersetting/sportsbook").IncludeDirectory(
                "~/views/customersetting/sportsbook/scripts", "*.js", true).IncludeDirectory(
                "~/views/customersetting/numbergame/scripts", "*.js", true));
            bundles.Add(new ScriptBundle("~/js/customersetting/virtualsports").IncludeDirectory(
               "~/views/customersetting/virtualsports/scripts", "*.js", true));
            bundles.Add(new LessBundle("~/css/customersetting").Include(
                "~/views/customersetting/resources/styles/fanex.customerSetting.less"));

            bundles.Add(new ScriptBundle("~/js/customersetting/editproduct").Include(
                "~/views/customersetting/resources/edit-product.js"));
            bundles.Add(new LessBundle("~/css/customersetting/editproduct").Include(
                "~/views/customersetting/resources/edit-product.less"));
        }

        /// <summary>
        /// Registers the account page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountPage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/home-page").Include(
                "~/views/account/resources/home-page.less"));

            bundles.Add(new ScriptBundle("~/js/home-page").Include(
                "~/views/account/resources/home-page.js"));
        }

        /// <summary>
        /// Registers the agent list page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerListPage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/customerlist").Include(
                "~/views/customerlist/resources/customer-list.less"));

            bundles.Add(new ScriptBundle("~/js/customerlist").Include(
                "~/views/customerList/resources/customer-list.js"));

            bundles.Add(new ScriptBundle("~/js/resetpassword").Include(
                "~/views/customerlist/resources/reset-password.js"));
        }

        /// <summary>
        /// Registers the bet list page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterBetListPage(BundleCollection bundles)
        {
            // Month Picker
            bundles.Add(new ScriptBundle("~/js/monthpicker").Include(
                "~/scripts/MonthPicker/monPickr-0.0.3.js"));
            bundles.Add(new LessBundle("~/css/monthpicker").Include(
                "~/scripts/MonthPicker/monPickr-0.0.3.css"));

            // For Bet List
            bundles.Add(new ScriptBundle("~/js/angulargrid").Include(
                "~/scripts/angularjs/angular-sanitize.js"));
            bundles.Add(new LessBundle("~/css/betlist").Include(
                "~/content/betlist/betlist.core.css",
                "~/content/betlist/betlist.css",
                "~/content/betlist/system-parlay-detail.css",
                "~/Content/Results/fanex.result.css",
                "~/Content/Results/results.css"));
            bundles.Add(new LessBundle("~/css/totalbets/parlay").Include(
                "~/Views/BetList/Resources/totalbets.parlay.less"));
            bundles.Add(new ScriptBundle("~/js/betlist").Include(
                "~/Views/BetList/Resources/betlist-core.js",
                "~/Views/BetList/Resources/betlist-manager.js"));
            bundles.Add(new ScriptBundle("~/js/totalbets/parlay").Include(
                "~/Views/BetList/Resources/betlist-core.js",
                "~/Views/BetList/Resources/totalbets-parlay.js"));
            bundles.Add(new ScriptBundle("~/js/betlistwinlost").Include(
                "~/Views/BetList/Resources/betlist-core.js",
                "~/Views/BetList/Resources/betlist-winlost.js"));
        }

        /// <summary>
        /// Registers the report page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/resultsreport").Include(
                "~/Views/Report/Resources/result-report.js"));
            bundles.Add(new LessBundle("~/css/resultsreport").Include(
                "~/Content/Results/fanex.result.css",
                "~/Content/Results/results.css"));

            bundles.Add(new LessBundle("~/css/outstandingreport").Include(
                 "~/Views/Report/Resources/Outstanding.less"));
            bundles.Add(new ScriptBundle("~/js/outstandingreport").Include(
                "~/Views/BetList/Resources/betlist-core.js",
                "~/Views/Report/Resources/outstanding-report.js"));

            bundles.Add(new ScriptBundle("~/js/agentwinlostreport").Include(
                "~/Views/Report/Resources/agent-winloss-report.js"));
            bundles.Add(new ScriptBundle("~/js/statement").Include(
                "~/Views/Report/Resources/statement-report.js"));
            bundles.Add(new ScriptBundle("~/js/statementdetail").Include(
                "~/Views/Report/Resources/statement-detail-report.js"));
            bundles.Add(new ScriptBundle("~/js/transferdetail").Include(
                "~/Views/Report/Resources/transfer-detail.js"));
        }

        /// <summary>
        /// Registers the total bets pages bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterTotalBetsPages(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/totalbets").Include(
                "~/Views/TotalBets/Resources/total-bets.less"));

            bundles.Add(new ScriptBundle("~/js/totalbets-soccer").Include(
                "~/Views/TotalBets/Resources/soccer.js",
                "~/Views/BetList/Resources/betlist-core.js"));

            bundles.Add(new ScriptBundle("~/js/totalbets-soccer-betlist").Include(
                "~/Views/TotalBets/Resources/soccer.betlist.js",
                "~/Views/BetList/Resources/betlist-core.js"));

            bundles.Add(new ScriptBundle("~/js/totalbets-basketball").Include(
                "~/Views/TotalBets/Resources/basketball.js",
                "~/Views/BetList/Resources/betlist-core.js"));

            bundles.Add(new ScriptBundle("~/js/totalbets-OtherSports").Include(
                "~/Views/TotalBets/Resources/OtherSports.js",
                "~/Views/BetList/Resources/betlist-core.js"));

            bundles.Add(new ScriptBundle("~/js/totalbets-outright").Include(
                "~/Views/TotalBets/Resources/outright.js",
                "~/Views/BetList/Resources/betlist-core.js"));
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

        private static void RegisterTransferPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/transfer-list").Include(
                "~/views/transfer/resources/transfer-list.js"));

            bundles.Add(new ScriptBundle("~/js/transfer-popup").Include(
                "~/views/transfer/resources/transfer-popup.js"));
        }

        private static void RegisterCreditBalancePage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/creditbalance-list").Include(
                "~/views/creditbalance/resources/creditbalance-list.js"));

            bundles.Add(new ScriptBundle("~/js/creditbalance-popup").Include(
                "~/views/creditbalance/resources/creditbalance-popup.js"));
        }

        private static void RegisterMessagePage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/message").Include(
                "~/views/message/resources/message.less"));

            bundles.Add(new ScriptBundle("~/js/message").Include(
                "~/views/message/resources/message.js"));
        }

        private static void RegisterToolPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/tool/password").Include(
                "~/views/tool/resources/password-tool.js"));
            bundles.Add(new ScriptBundle("~/js/tool/sync-monitoring").Include(
                "~/views/tool/resources/sync-monitoring.js"));
        }

        #endregion Pages
    }
}