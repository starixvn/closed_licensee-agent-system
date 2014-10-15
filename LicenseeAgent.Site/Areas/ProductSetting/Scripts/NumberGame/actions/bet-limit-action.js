if (fanex.customerSetting) {
    fanex.customerSetting.numberGameBettingLimitActions =
    {
        actions: function (modelData, inputControl, scope, languageService) {
            'use strict';
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
                        message = languageService.getString("OnlyNumberBetweenNumber");
                    } else if (formatInputNumber(modelData.MinBet) > formatInputNumber(modelData.MaxBet)) {
                        message = languageService.getString("MinBetLessThanOrEqualMaxBet");
                    } else if (modelData.MaxMinBet > 0 && formatInputNumber(modelData.MinBet) < formatInputNumber(modelData.MaxMinBet)) {
                        message = languageService.getFormatString("MinBetMoreThanOrEqualTo", + reformatInputNumberForControl(modelData.MaxMinBet));
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
                        message = languageService.getString("OnlyNumberBetweenNumber");
                    } else if (formatInputNumber(modelData.MinBet) > formatInputNumber(modelData.MaxBet)) {
                        message = languageService.getString("MaxBetMoreThanOrEqualMinBet");
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerBall)) {
                        message = languageService.getString("MaxBetLessThanOrEqualMaxPerBall");
                    } else if (modelData.MinMaxBet > 0 && formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MinMaxBet)) {
                        message = languageService.getFormatString("MaxBetLessThanOrEqualTo", reformatInputNumberForControl(modelData.MinMaxBet));
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
                        message = languageService.getString("OnlyNumberBetweenNumber");
                    } else if (formatInputNumber(modelData.MaxPerBall) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = languageService.getString("MaxPerMatchMoreThanOrEqualMaxPerBall");
                    } else if (modelData.MinMaxPerMatch > 0 && formatInputNumber(modelData.MaxPerMatch) > formatInputNumber(modelData.MinMaxPerMatch)) {
                        message = languageService.getFormatString("MaxPerMatchLessThanOrEqualTo", reformatInputNumberForControl(modelData.MinMaxPerMatch));
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
                        message = languageService.getString("OnlyNumberBetweenNumber");
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerBall)) {
                        message = languageService.getString("MaxPerBallMoreThanOrEqualMaxBet");
                    } else if (formatInputNumber(modelData.MaxPerBall) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = languageService.getString("MaxPerBallLessThanOrEqualMaxPerMatch");
                    } else if (modelData.MinMaxPerBall > 0 && formatInputNumber(modelData.MaxPerBall) > formatInputNumber(modelData.MinMaxPerBall)) {
                        message = languageService.getFormatString("MaxPerBallLessThanOrEqualTo", reformatInputNumberForControl(modelData.MinMaxPerBall));
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