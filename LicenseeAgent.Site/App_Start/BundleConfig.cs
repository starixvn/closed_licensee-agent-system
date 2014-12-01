namespace LicenseeAgent.Site
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// MVC Bundle Configuration.
    /// </summary>
    public static class BundleConfig
    {
        #region Register bundles

        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCore(bundles);
            RegisterComponents(bundles);
            RegisterPages(bundles);
            bundles.FileSetOrderList.Clear();
        }

        #endregion Register bundles

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
                "~/scripts/breadcrumb.js",
                "~/scripts/page.js",
                "~/scripts/enums.js",
                "~/scripts/grid.js",
                "~/scripts/popup.js",
                "~/scripts/component.js"));
            bundles.Add(new LessBundle("~/css/core").Include(
                "~/content/sprite.less",
                "~/content/core.less",
                "~/content/core-components.less"));

            bundles.Add(new ScriptBundle("~/js/language").Include(
                "~/scripts/language.js",
                "~/scripts/ResourcesJs/Las.js",
                "~/scripts/ResourcesJs/Las.en-US.js",
                "~/scripts/ResourcesJs/Las.zh-CN.js",
                "~/scripts/ResourcesJs/Las.zh-TW.js",
                "~/scripts/ResourcesJs/SportType.js",
                "~/scripts/ResourcesJs/SportType.en-US.js",
                "~/scripts/ResourcesJs/SportType.zh-CN.js",
                "~/scripts/ResourcesJs/SportType.zh-TW.js",
                "~/scripts/ResourcesJs/BetType.js",
                "~/scripts/ResourcesJs/BetType.en-US.js",
                "~/scripts/ResourcesJs/BetType.zh-CN.js",
                "~/scripts/ResourcesJs/BetType.zh-TW.js"));
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
                "~/scripts/bootstrap-datepicker/bootstrap-datepicker.js"));
            bundles.Add(new LessBundle("~/css/datetimepicker").Include(
                "~/content/bootstrap-datepicker/datepicker.less",
                "~/content/bootstrap-datepicker/override.less"));

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

            // CryptoJS
            bundles.Add(new ScriptBundle("~/js/cryptojs").IncludeDirectory(
                "~/scripts/cryptojs", "*.js"));
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
            RegisterHomePage(bundles);
            RegisterBetListPage(bundles);
            RegisterTransferPage(bundles);
            RegisterPluginControl(bundles);
            RegisterCreditBalancePage(bundles);
            RegisterMessagePage(bundles);
        }

        private static void RegisterErrorPage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/error/under-maintenance").Include(
                "~/views/error/scripts/under-maintenance-page.js"));
            bundles.Add(new LessBundle("~/css/error").Include(
                "~/views/error/content/error-page.less"));
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
                "~/views/home/scripts/home-menu.js",
                "~/views/home/scripts/home-header.js"));
            bundles.Add(new LessBundle("~/css/home").Include(
                "~/views/home/content/home.less"));
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
            bundles.Add(new LessBundle("~/css/betlist").Include(
                "~/Views/BetList/Content/betlist.core.css",
                "~/Views/BetList/Content/betlist.css",
                "~/Views/BetList/Content/system-parlay-detail.css",
                "~/Content/Fanex.Result/fanex.result.less"));
            bundles.Add(new ScriptBundle("~/js/betlist/member").Include(
                "~/Views/BetList/Scripts/core.js",
                "~/Views/BetList/Scripts/member.js"));
            bundles.Add(new ScriptBundle("~/js/betlist/winloss").Include(
                "~/Views/BetList/Scripts/core.js",
                "~/Views/BetList/Scripts/winlost.js"));
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
            bundles.Add(new ScriptBundle("~/js/transfer").Include(
                "~/views/transfer/scripts/transfer-list.js",
                "~/views/transfer/scripts/transfer-popup.js"));
        }

        private static void RegisterCreditBalancePage(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/credit-balance").Include(
                "~/views/creditbalance/scripts/creditbalance-list.js",
                "~/views/creditbalance/scripts/creditbalance-popup.js"));
        }

        private static void RegisterMessagePage(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/message").Include(
                "~/views/message/content/message.less"));
            bundles.Add(new ScriptBundle("~/js/message").Include(
                "~/views/message/scripts/message.js"));
        }

        #endregion Pages
    }
}