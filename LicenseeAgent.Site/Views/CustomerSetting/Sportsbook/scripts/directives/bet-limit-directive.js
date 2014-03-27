/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.bettingLimitActions =
    {
        actions: function (modelData, controller, inputControl, scope) {
            var formatInputNumber = fanex.customerSetting.betLimitService.formatInputNumber;
            var reformatInputNumberForControl = fanex.customerSetting.betLimitService.reformatInputNumberForControl;
            return {
                isDataChanged: function() {
                    return formatInputNumber(modelData.MinBet) !== modelData.OldMinBet
                        || formatInputNumber(modelData.MaxBet) !== modelData.OldMaxBet
                        || formatInputNumber(modelData.MaxPerMatch) !== modelData.OldMaxPerMatch;
                },
                onChangeMinBet: function() {
                    modelData.actions.validMinBet();
                },
                onChangeMaxBet: function() {
                    modelData.actions.validMaxBet();
                },
                onChangeMaxPerMatch: function() {
                    modelData.actions.validMaxPerMatch();
                },
                isUnlimited: function() {
                    return formatInputNumber(modelData.MinBet) === 0 &&
                            formatInputNumber(modelData.MaxBet) === 0 && 
                            formatInputNumber(modelData.MaxPerMatch) === 0;
                },
                validMinBet: function() {
                    var message = "";

                    if (!fanex.customerSetting.betLimitService.regIsDigit(modelData.MinBet)) {
                        message = scope.language["bettingLimit_Message_OnlyNumber"];
                    } else if (formatInputNumber(modelData.MinBet) > formatInputNumber(modelData.MaxBet)) {
                        message = scope.language["bettingLimit_Message_MinBetLessThanOrEqualMaxBet"];
                    } else if (modelData.MaxMinBet > 0 && formatInputNumber(modelData.MinBet) < formatInputNumber(modelData.MaxMinBet)) {
                        message = scope.language["bettingLimit_Message_MinBetMoreThanOrEqualTo"] + reformatInputNumberForControl(modelData.MaxMinBet);
                    }

                    if (message !== "") {
                        fanex.customerSetting.betLimitService.showInvalidMessage(inputControl[0], message);
                        return false;
                    } else {
                        fanex.hideHint();
                        return true;
                    }
                },
                validMaxBet: function() {
                    var message = "";

                    if (!fanex.customerSetting.betLimitService.regIsDigit(modelData.MaxBet)) {
                        message = scope.language["bettingLimit_Message_OnlyNumber"];
                    } else if (formatInputNumber(modelData.MinBet) > formatInputNumber(modelData.MaxBet)) {
                        message = scope.language["bettingLimit_Message_MaxBetMoreThanOrEqualMinBet"];
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = scope.language["bettingLimit_Message_MaxBetLessThanOrEqualMaxPerMatch"];
                    } else if (modelData.MinMaxBet > 0 && formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MinMaxBet)) {
                        message = scope.language["bettingLimit_Message_MaxBetLessThanOrEqualTo"] + reformatInputNumberForControl(modelData.MinMaxBet);
                    }

                    if (message !== "") {
                        fanex.customerSetting.betLimitService.showInvalidMessage(inputControl[1], message);
                        return false;
                    } else {
                        fanex.hideHint();
                        return true;
                    }
                },
                validMaxPerMatch: function() {
                    var message = "";

                    if (!fanex.customerSetting.betLimitService.regIsDigit(modelData.MaxPerMatch)) {
                        message = scope.language["bettingLimit_Message_OnlyNumber"];
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = scope.language["bettingLimit_Message_MaxPerMatchMoreThanOrEqualMaxBet"];
                    } else if (modelData.MinMaxPerMatch > 0 && formatInputNumber(modelData.MaxPerMatch) > formatInputNumber(modelData.MinMaxPerMatch)) {
                        message = scope.language["bettingLimit_Message_MaxPerMatchLessThanOrEqualTo"] + reformatInputNumberForControl(modelData.MinMaxPerMatch);
                    }

                    if (message !== "") {
                        fanex.customerSetting.betLimitService.showInvalidMessage(inputControl[2], message);
                        return false;
                    } else {
                        fanex.hideHint();
                        return true;
                    }
                },
                isValid: function() {
                    if (modelData.actions.isUnlimited()) {
                        return true;
                    }
                    return modelData.actions.validMinBet() && modelData.actions.validMaxBet() && modelData.actions.validMaxPerMatch();
                }
            };
        }
    };


    // An agent edit another agent in the simple mode
    fanex.customerSetting.directive('bettingLimitSimpleMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'fanex.customersetting.betlimitcontroller',
            scope: {
                modelData: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-simple-mode.html',
            link: function(scope, element, attrs, controller) {
                var modelData = scope.modelData;
                modelData.sportTypeName = scope.language["sport_" + modelData.SportId + "_" + modelData.BetTypeId];
                modelData.OldMinBet = modelData.MinBet;
                modelData.OldMaxBet = modelData.MaxBet;
                modelData.OldMaxPerMatch = modelData.MaxPerMatch;
                modelData.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MinBet);
                modelData.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxBet);
                modelData.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerMatch);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, controller, inputControl, scope);
            }
        };
    });

    fanex.customerSetting.directive('bettingLimitSimpleModeReadOnly', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'fanex.customersetting.betlimitcontroller',
            scope: {
                modelData: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-simple-mode-readonly.html',
            link: function(scope, element, attrs, controller) {
                var modelData = scope.modelData;
                modelData.sportTypeName = scope.language["sport_" + modelData.SportId + "_" + modelData.BetTypeId];
                modelData.OldMinBet = modelData.MinBet;
                modelData.OldMaxBet = modelData.MaxBet;
                modelData.OldMaxPerMatch = modelData.MaxPerMatch;
                modelData.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MinBet);
                modelData.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxBet);
                modelData.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(modelData.MaxPerMatch);
                var inputControl = element.find('.input');
                modelData.actions = fanex.customerSetting.bettingLimitActions.actions(modelData, controller, inputControl, scope);
            }
        };
    });

    fanex.customerSetting.directive('bettingLimitAdvancedMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'fanex.customersetting.betlimitcontroller',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-advanced-mode.html',
            link: function(scope, element, attrs, controller) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, controller, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                betSetting.OldMinBet = betSetting.MinBet;
                betSetting.OldMaxBet = betSetting.MaxBet;
                betSetting.OldMaxPerMatch = betSetting.MaxPerMatch;
                betSetting.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MinBet);
                betSetting.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxBet);
                betSetting.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerMatch);
            }
        };
    });

    fanex.customerSetting.directive('bettingLimitAdvancedModeReadOnly', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'fanex.customersetting.betlimitcontroller',
            scope: {
                bettingLimitsModel: '=',
                betSetting: '=',
                currentIndex: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/betsetting/bet-setting-advanced-mode-readonly.html',
            link: function(scope, element, attrs, controller) {
                var betSetting = scope.betSetting;
                var inputControl = element.find('.input');
                betSetting.actions = fanex.customerSetting.bettingLimitActions.actions(betSetting, controller, inputControl, scope);
                betSetting.sportTypeName = scope.language["sport_" + betSetting.SportId + "_" + betSetting.BetTypeId];
                betSetting.OldMinBet = betSetting.MinBet;
                betSetting.OldMaxBet = betSetting.MaxBet;
                betSetting.OldMaxPerMatch = betSetting.MaxPerMatch;
                betSetting.MinBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MinBet);
                betSetting.MaxBet = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxBet);
                betSetting.MaxPerMatch = fanex.customerSetting.betLimitService.reformatInputNumberForControl(betSetting.MaxPerMatch);
            }
        };
    });
}