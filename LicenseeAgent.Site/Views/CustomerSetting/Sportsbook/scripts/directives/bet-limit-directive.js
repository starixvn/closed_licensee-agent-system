/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */
'use strict';

if (fanex.customerSetting) {

    // An agent edit another agent in the simple mode
    fanex.customerSetting.directive('bettingLimitSimpleMode', function () {
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
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, inputControl, scope);
            }
        };
    });

    fanex.customerSetting.directive('bettingLimitSimpleModeReadOnly', function() {
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                modelData: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-simple-mode-readonly.html'),
            link: function(scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = scope.language["sport_" + modelData.SportId + "_" + modelData.BetTypeId];
                fanex.customerSetting.betLimitService.changeSettingValidation(modelData);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, inputControl, scope);
            }
        };
    });

    fanex.customerSetting.directive('bettingLimitAdvancedMode', function() {
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
            link: function(scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                fanex.customerSetting.betLimitService.changeSettingValidation(betSetting);
            }
        };
    });

    fanex.customerSetting.directive('bettingLimitAdvancedModeReadOnly', function() {
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
            link: function(scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                fanex.customerSetting.betLimitService.changeSettingValidation(betSetting);
            }
        };
    });
}