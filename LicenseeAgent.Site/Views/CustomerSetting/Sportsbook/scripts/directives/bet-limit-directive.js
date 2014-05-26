/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */
'use strict';

if (fanex.customerSetting) {
    // An agent edit another agent in the simple mode
    fanex.customerSetting.directive('bettingLimitSimpleMode',
    ['Fanex.CustomerSetting.LanguageService', function (languageService) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                modelData: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-simple-mode.html'),
            link: function (scope, element) {
                var modelData = scope.modelData;
                fanex.customerSetting.betLimitService.changeSettingValidation(modelData);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, inputControl, scope, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('bettingLimitSimpleModeReadOnly',
    ['Fanex.CustomerSetting.LanguageService', function (languageService) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                modelData: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-simple-mode-readonly.html'),
            link: function (scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = languageService.getSportName(modelData.SportId, modelData.BetTypeId);
                fanex.customerSetting.betLimitService.changeSettingValidation(modelData);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, inputControl, scope, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('bettingLimitAdvancedMode',
    ['Fanex.CustomerSetting.LanguageService', function (languageService) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-advanced-mode.html'),
            link: function (scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope, languageService);
                betSetting.sportTypeName = languageService.getSportName(betSetting.SportId, betSetting.BetTypeId);
                fanex.customerSetting.betLimitService.changeSettingValidation(betSetting);
            }
        };
    } ]);

    fanex.customerSetting.directive('bettingLimitAdvancedModeReadOnly',
    ['Fanex.CustomerSetting.LanguageService', function (languageService) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-advanced-mode-readonly.html'),
            link: function (scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope, languageService);
                betSetting.sportTypeName = languageService.getSportName(betSetting.SportId, betSetting.BetTypeId);
                fanex.customerSetting.betLimitService.changeSettingValidation(betSetting);
            }
        };
    } ]);
}