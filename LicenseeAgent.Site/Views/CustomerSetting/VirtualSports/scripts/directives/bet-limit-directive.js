/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */

if (fanex.customerSetting) {
    // An agent edit another agent in the simple mode
    fanex.customerSetting.directive('bettingLimitSimpleMode',
    [
    'Fanex.CustomerSetting.LanguageService',
    function (languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                modelData: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/betsetting/bet-setting-simple-mode.html'),
            link: function (scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = languageService.getSportName(modelData.SportId, modelData.BetTypeId);
                modelData.OldMinBet = modelData.MinBet;
                modelData.OldMaxBet = modelData.MaxBet;
                modelData.OldMaxPerMatch = modelData.MaxPerMatch;
                modelData.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MinBet);
                modelData.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxBet);
                modelData.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerMatch);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, inputControl, scope, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('bettingLimitSimpleModeReadOnly',
    [
    'Fanex.CustomerSetting.LanguageService',
    function (languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                modelData: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/betsetting/bet-setting-simple-mode-readonly.html'),
            link: function (scope, element) {
                var modelData = scope.modelData;
                modelData.sportTypeName = languageService.getSportName(modelData.SportId, modelData.BetTypeId);
                modelData.OldMinBet = modelData.MinBet;
                modelData.OldMaxBet = modelData.MaxBet;
                modelData.OldMaxPerMatch = modelData.MaxPerMatch;
                modelData.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MinBet);
                modelData.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxBet);
                modelData.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerMatch);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, inputControl, scope, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('bettingLimitAdvancedMode',
    [
    'Fanex.CustomerSetting.LanguageService',
    function (languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/betsetting/bet-setting-advanced-mode.html'),
            link: function (scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope, languageService);
                betSetting.sportTypeName = languageService.getSportName(betSetting.SportId, betSetting.BetTypeId);
                betSetting.OldMinBet = betSetting.MinBet;
                betSetting.OldMaxBet = betSetting.MaxBet;
                betSetting.OldMaxPerMatch = betSetting.MaxPerMatch;
                betSetting.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MinBet);
                betSetting.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxBet);
                betSetting.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerMatch);
            }
        };
    } ]);

    fanex.customerSetting.directive('bettingLimitAdvancedModeReadOnly',
    [
    'Fanex.CustomerSetting.LanguageService',
    function (languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'BetLimitController',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/betsetting/bet-setting-advanced-mode-readonly.html'),
            link: function (scope, element) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, inputControl, scope, languageService);
                betSetting.sportTypeName = languageService.getSportName(betSetting.SportId, betSetting.BetTypeId);
                betSetting.OldMinBet = betSetting.MinBet;
                betSetting.OldMaxBet = betSetting.MaxBet;
                betSetting.OldMaxPerMatch = betSetting.MaxPerMatch;
                betSetting.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MinBet);
                betSetting.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxBet);
                betSetting.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerMatch);
            }
        };
    } ]);
}