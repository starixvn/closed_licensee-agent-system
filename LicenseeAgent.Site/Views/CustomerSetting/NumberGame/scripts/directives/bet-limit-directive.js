/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />

/* global core */

'use strict';

if (fanex.customerSetting) {

    // An agent edit another agent in the simple mode
    fanex.customerSetting.directive('numberGameBettingLimitSimpleMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGameBetLimitController',
            scope: {
                modelData: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-simple-mode.html'),
            link: function (scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = scope.language["sport_" + modelData.SportId + "_" + modelData.BetTypeId];
                fanex.customerSetting.numberGameBetLimitService.changeSettingValidation(modelData);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.numberGameBettingLimitActions.actions(modelData, inputControl, scope);
            }
        };
    });

    fanex.customerSetting.directive('numberGameBettingLimitSimpleModeReadOnly', function() {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGameBetLimitController',
            scope: {
                modelData: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-simple-mode-readonly.html'),
            link: function(scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = scope.language["sport_" + modelData.SportId + "_" + modelData.BetTypeId];
                fanex.customerSetting.numberGameBetLimitService.changeSettingValidation(modelData);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.numberGameBettingLimitActions.actions(modelData, inputControl, scope);
            }
        };
    });

    fanex.customerSetting.directive('numberGameBettingLimitAdvancedMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGameBetLimitController',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-advanced-mode.html'),
            link: function (scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.numberGameBettingLimitActions.actions(betSetting, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                fanex.customerSetting.numberGameBetLimitService.changeSettingValidation(betSetting);
            }
        };
    });

    fanex.customerSetting.directive('numberGameBettingLimitAdvancedModeReadOnly', function() {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGameBetLimitController',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-advanced-mode-readonly.html'),
            link: function(scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.numberGameBettingLimitActions.actions(betSetting, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                fanex.customerSetting.numberGameBetLimitService.changeSettingValidation(betSetting);
            }
        };
    });
}