/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

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
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-simple-mode.html',
            link: function (scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = scope.language["sport_" + modelData.SportId + "_" + modelData.BetTypeId];
                modelData.OldMinBet = modelData.MinBet;
                modelData.OldMaxBet = modelData.MaxBet;
                modelData.OldMaxPerMatch = modelData.MaxPerMatch;
                modelData.OldMaxPerBall = modelData.MaxPerBall;
                modelData.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MinBet);
                modelData.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxBet);
                modelData.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerMatch);
                modelData.MaxPerBall = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerBall);
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
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-simple-mode-readonly.html',
            link: function(scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = scope.language["sport_" + modelData.SportId + "_" + modelData.BetTypeId];
                modelData.OldMinBet = modelData.MinBet;
                modelData.OldMaxBet = modelData.MaxBet;
                modelData.OldMaxPerMatch = modelData.MaxPerMatch;
                modelData.OldMaxPerBall = modelData.MaxPerBall;
                modelData.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MinBet);
                modelData.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxBet);
                modelData.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerMatch);
                modelData.MaxPerBall = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerBall);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.numberGameBettingLimitActions.actions(modelData, inputControl, scope);
            }
        };
    });

    fanex.customerSetting.directive('numberGameBettingLimitAdvancedMode', function() {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGameBetLimitController',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-advanced-mode.html',
            link: function(scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                betSetting.OldMinBet = betSetting.MinBet;
                betSetting.OldMaxBet = betSetting.MaxBet;
                betSetting.OldMaxPerMatch = betSetting.MaxPerMatch;
                betSetting.OldMaxPerBall = betSetting.MaxPerBall;
                betSetting.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MinBet);
                betSetting.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxBet);
                betSetting.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerMatch);
                betSetting.MaxPerBall = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerBall);
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
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/betsetting/bet-setting-advanced-mode-readonly.html',
            link: function(scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                betSetting.OldMinBet = betSetting.MinBet;
                betSetting.OldMaxBet = betSetting.MaxBet;
                betSetting.OldMaxPerMatch = betSetting.MaxPerMatch;
                betSetting.OldMaxPerBall = betSetting.MaxPerBall;
                betSetting.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MinBet);
                betSetting.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxBet);
                betSetting.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerMatch);
                betSetting.MaxPerBall = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerBall);
            }
        };
    });
}