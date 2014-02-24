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
                "~/scripts/blockui/jquery.blockui.js",
                "~/scripts/core.js",
                "~/scripts/utility.js",
                "~/scripts/language.js",
                "~/scripts/popup.js"));

            bundles.Add(new ScriptBundle("~/js/language").Include(
                "~/scripts/ResourcesJs/Las.js",
                "~/scripts/ResourcesJs/Las.zh-CN.js",
                "~/scripts/ResourcesJs/Las.zh-TW.js"));

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
            bundles.Add(new ScriptBundle("~/js/fanexgrid").Include(
                "~/scripts/fanexgrid/numeral.js",
                "~/scripts/fanexgrid/Fanex.Grid.js",
                "~/scripts/fanexgrid/Fanex.Pagination.js"));
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
                "~/scripts/jquery-form/jquery.form.min.js"));

            // Jquery UI
            bundles.Add(new ScriptBundle("~/js/jqueryui").Include(
                "~/scripts/jquery-ui/jquery.ui.core.js",
                "~/scripts/jquery-ui/jquery.ui.widget.js",
                "~/scripts/jquery-ui/jquery.ui.mouse.js",
                "~/scripts/jquery-ui/jquery.ui.position.js",
                "~/scripts/jquery-ui/jquery.ui.draggable.js",
                "~/scripts/jquery-ui/jquery.ui.droppable.js",
                "~/scripts/jquery-ui/jquery.ui.resizable.js",
                "~/scripts/jquery-ui/jquery.ui.selectable.js",
                "~/scripts/jquery-ui/jquery.ui.sortable.js",
                "~/scripts/jquery-ui/jquery.ui.button.js",
                "~/scripts/jquery-ui/jquery.ui.dialog.js",
                "~/scripts/clickout/jquery.clickout.js"));
            bundles.Add(new LessBundle("~/css/jqueryui").Include(
                "~/content/jquery-ui/jquery.ui.core.css",
                "~/content/jquery-ui/jquery.ui.theme.css",
                "~/content/jquery-ui/jquery.ui.resizable.css",
                "~/content/jquery-ui/jquery.ui.button.css",
                "~/content/jquery-ui/jquery.ui.dialog.css"));
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
                "~/scripts/headjs/head.core.js",
                "~/scripts/dotdotdot/jquery.dotdotdot.js",
                "~/views/home/resources/menu.js",
                "~/views/home/resources/header.js",
                "~/scripts/cookie/jquery.cookie.js",
                "~/scripts/layout/jquery.layout.min.js"));

            bundles.Add(new ScriptBundle("~/js/home/accountinfo").Include(
                "~/scripts/scroller/jquery.nicescroll.min.js",
                "~/views/accountinformation/resources/index.js"));

            bundles.Add(new ScriptBundle("~/js/home/internal").Include(
                "~/Views/Internal/Resources/Header.js"));

            bundles.Add(new LessBundle("~/css/home").Include(
                "~/views/home/resources/home.less"));

            bundles.Add(new LessBundle("~/css/home/accountinfo").Include(
                "~/views/accountinformation/resources/index.less"));
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
                "~/views/customersetting/sportsbook/scripts", "*.js", true));
            bundles.Add(new LessBundle("~/css/customersetting/sportsbook").Include(
                "~/views/customersetting/sportsbook/styles/fanex.sportsbookSetting.less"));
        }

        /// <summary>
        /// Registers the account page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterAccountPage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/home-page").Include(
                "~/Views/Account/Resources/home-page.less"));

            bundles.Add(new ScriptBundle("~/js/home-page").Include(
                "~/Views/Account/Resources/home-page.js"));
        }

        /// <summary>
        /// Registers the agent list page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterCustomerListPage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/customerlist").Include(
                "~/Views/CustomerList/Resources/customerlist.less"));

            bundles.Add(new ScriptBundle("~/js/customerlist").Include(
                "~/Views/CustomerList/Resources/customerlist.js"));

            bundles.Add(new ScriptBundle("~/js/resetpassword").Include(
                "~/Views/CustomerList/Resources/resetpassword.js"));
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
                "~/content/betlist/system-parlay-detail.css"));
            bundles.Add(new LessBundle("~/css/totalbets/parlay").Include(
                "~/Views/BetList/Resources/totalbets.parlay.less"));
            bundles.Add(new ScriptBundle("~/js/betlist").Include(
                "~/Views/BetList/Resources/betlist.core.js",
                "~/Views/BetList/Resources/betlist.manager.js"));
            bundles.Add(new ScriptBundle("~/js/totalbets/parlay").Include(
                "~/Views/BetList/Resources/betlist.core.js",
                "~/Views/BetList/Resources/totalbets.parlay.js"));
            bundles.Add(new ScriptBundle("~/js/betlistwinlost").Include(
                "~/Views/BetList/Resources/betlist.core.js",
                "~/Views/BetList/Resources/betlist.winlost.js"));
        }

        /// <summary>
        /// Registers the report page bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterReportPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/resultsreport").Include(
                "~/Views/Report/Resources/ResultsReport.js"));
            bundles.Add(new LessBundle("~/css/resultsreport").Include(
                "~/Content/Results/fanex.result.css",
                "~/Content/Results/results.css"));

            bundles.Add(new LessBundle("~/css/outstandingreport").Include(
                 "~/Views/Report/Resources/Outstanding.less"));
            bundles.Add(new ScriptBundle("~/js/outstandingreport").Include(
                "~/Views/BetList/Resources/betlist.core.js",
                "~/Views/Report/Resources/OutstandingReport.js"));

            bundles.Add(new LessBundle("~/css/agentwinlostreport").Include(
                 "~/Views/Report/Resources/AgentWinLoss.less"));
            bundles.Add(new ScriptBundle("~/js/agentwinlostreport").Include(
                "~/Views/Report/Resources/AgentWinlossReport.js"));
            bundles.Add(new ScriptBundle("~/js/statement").Include(
                "~/Views/Report/Resources/Statement.js"));
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

        #endregion Pages
    }
}