'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.bettingLimitActions =
    {
        actions: function(modelData, inputControl, scope) {
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
                    return formatInputNumber(modelData.MinBet) === 0
                        && formatInputNumber(modelData.MaxBet) === 0 
                        && formatInputNumber(modelData.MaxPerMatch) === 0;
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
                    return modelData.actions.validMinBet()
                        && modelData.actions.validMaxBet() 
                        && modelData.actions.validMaxPerMatch();
                }
            };
        }
    };
}