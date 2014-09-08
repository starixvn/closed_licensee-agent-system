/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */

if (fanex.customerSetting) {
    fanex.customerSetting.directive('commissionSettingLicenseeOrAgentEditAgent',
    [
    'Fanex.CustomerSetting.LanguageService',
    function (languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'CommissionController',
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/commission/licensee-or-agent-edit-agent.html'),
            link: function (scope, element) {
                var commissionSetting = scope.commissionSetting;
                var inputControl = element.find('.input');
                commissionSetting.actions = fanex.customerSetting.commissionSettingActions.licenseeOrAgentEditAgentActions(commissionSetting,
                    inputControl,
                    scope,
                    languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('commissionSettingLicenseeOrAgentEditAgentReadOnly',
    [
    'Fanex.CustomerSetting.LanguageService',
    function (languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'CommissionController',
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/commission/licensee-or-agent-edit-agent-readonly.html'),
            link: function (scope, element) {
                var commissionSetting = scope.commissionSetting;
                var inputControl = element.find('.input');
                commissionSetting.actions = fanex.customerSetting.commissionSettingActions.licenseeOrAgentEditAgentActions(commissionSetting,
                    inputControl,
                    scope,
                    languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('commissionSettingAgentEditMember',
    [
    'Fanex.CustomerSetting.LanguageService',
    function (languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'CommissionController',
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/commission/agent-edit-customer.html'),
            link: function (scope) {
                var commissionSettingsViewModel = scope.viewModel.commissionSettingsViewModel;
                commissionSettingsViewModel.actions = fanex.customerSetting.commissionSettingActions.agentEditCustomerActions(commissionSettingsViewModel,
                    scope,
                    languageService);
            }
        };
    } ]);
}