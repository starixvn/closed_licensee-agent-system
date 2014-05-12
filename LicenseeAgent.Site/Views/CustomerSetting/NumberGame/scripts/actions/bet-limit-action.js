'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.numberGameBettingLimitActions =
    {
        actions: function (modelData, inputControl, scope) {
            var formatInputNumber = fanex.customerSetting.betLimitService.formatInputNumber;
            var reformatInputNumberForControl = fanex.customerSetting.betLimitService.reformatInputNumberForControl;
            return {
                isDataChanged: function () {
                    return formatInputNumber(modelData.MinBet) !== modelData.OldMinBet
                        || formatInputNumber(modelData.MaxBet) !== modelData.OldMaxBet
                        || formatInputNumber(modelData.MaxPerMatch) !== modelData.OldMaxPerMatch
                        || formatInputNumber(modelData.MaxPerBall) !== modelData.OldMaxPerBall;
                },
                onChangeMinBet: function () {
                    modelData.actions.validMinBet();
                },
                onChangeMaxBet: function () {
                    modelData.actions.validMaxBet();
                },
                onChangeMaxPerMatch: function () {
                    modelData.actions.validMaxPerMatch();
                },
                onChangeMaxPerBall: function () {
                    modelData.actions.validMaxPerBall();
                },
                isUnlimited: function () {
                    return formatInputNumber(modelData.MinBet) === 0
                        && formatInputNumber(modelData.MaxBet) === 0
                        && formatInputNumber(modelData.MaxPerMatch) === 0
                        && formatInputNumber(modelData.MaxPerBall) === 0;
                },
                validMinBet: function () {
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
                validMaxBet: function () {
                    var message = "";

                    if (!fanex.customerSetting.betLimitService.regIsDigit(modelData.MaxBet)) {
                        message = scope.language["bettingLimit_Message_OnlyNumber"];
                    } else if (formatInputNumber(modelData.MinBet) > formatInputNumber(modelData.MaxBet)) {
                        message = scope.language["bettingLimit_Message_MaxBetMoreThanOrEqualMinBet"];
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerBall)) {
                        message = scope.language["bettingLimit_Message_MaxBetLessThanOrEqualMaxPerBall"];
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
                validMaxPerMatch: function () {
                    var message = "";

                    if (!fanex.customerSetting.betLimitService.regIsDigit(modelData.MaxPerMatch)) {
                        message = scope.language["bettingLimit_Message_OnlyNumber"];
                    } else if (formatInputNumber(modelData.MaxPerBall) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = scope.language["bettingLimit_Message_MaxPerMatchMoreThanOrEqualMaxPerBall"];
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
                validMaxPerBall: function () {
                    var message = "";
                    
                    if (!fanex.customerSetting.betLimitService.regIsDigit(modelData.MaxPerBall)) {
                        message = scope.language["bettingLimit_Message_OnlyNumber"];
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerBall)) {
                        message = scope.language["bettingLimit_Message_MaxPerBallMoreThanOrEqualMaxBet"];
                    } else if (formatInputNumber(modelData.MaxPerBall) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = scope.language["bettingLimit_Message_MaxPerBallLessThanOrEqualTo"] + reformatInputNumberForControl(modelData.MaxPerMatch);
                    } else if (modelData.MinMaxPerBall > 0 && formatInputNumber(modelData.MaxPerBall) > formatInputNumber(modelData.MinMaxPerBall)) {
                        message = scope.language["bettingLimit_Message_MaxPerBallLessThanOrEqualTo"] + reformatInputNumberForControl(modelData.MinMaxPerBall);
                    }

                    if (message !== "") {
                        fanex.customerSetting.betLimitService.showInvalidMessage(inputControl[3], message);
                        return false;
                    } else {
                        fanex.hideHint();
                        return true;
                    }
                },
                isValid: function () {
                    if (modelData.actions.isUnlimited()) {
                        return true;
                    }
                    return modelData.actions.validMinBet()
                            && modelData.actions.validMaxBet()
                            && modelData.actions.validMaxPerMatch()
                            && modelData.actions.validMaxPerBall();
                }
            };
        }
    };
}