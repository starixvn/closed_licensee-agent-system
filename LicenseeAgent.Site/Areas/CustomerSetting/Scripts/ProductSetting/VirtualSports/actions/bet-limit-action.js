if (fanex.customerSetting) {
    fanex.customerSetting.bettingLimitActions =
    {
        actions: function (modelData, inputControl, scope, languageService) {
            'use strict';
            var formatInputNumber = fanex.customerSetting.betLimitService.formatInputNumber;
            var reformatInputNumberForControl = fanex.customerSetting.betLimitService.reformatInputNumberForControl;
            return {
                isDataChanged: function () {
                    return formatInputNumber(modelData.MinBet) !== modelData.OldMinBet
                        || formatInputNumber(modelData.MaxBet) !== modelData.OldMaxBet
                        || formatInputNumber(modelData.MaxPerMatch) !== modelData.OldMaxPerMatch;
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
                isUnlimited: function () {
                    return formatInputNumber(modelData.MinBet) === 0
                        && formatInputNumber(modelData.MaxBet) === 0
                        && formatInputNumber(modelData.MaxPerMatch) === 0;
                },
                validMinBet: function () {
                    var message = "";

                    if (!fanex.customerSetting.betLimitService.regIsDigit(modelData.MinBet)) {
                        message = languageService.getString("OnlyNumberBetweenNumber");
                    } else if (formatInputNumber(modelData.MinBet) > formatInputNumber(modelData.MaxBet)) {
                        message = languageService.getString("MinBetLessThanOrEqualMaxBet");
                    } else if (modelData.MaxMinBet > 0 && formatInputNumber(modelData.MinBet) < formatInputNumber(modelData.MaxMinBet)) {
                        message = languageService.getFormatString("MinBetMoreThanOrEqualTo", reformatInputNumberForControl(modelData.MaxMinBet));
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
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = languageService.getString("MaxBetLessThanOrEqualMaxPerMatch");
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
                    } else if (formatInputNumber(modelData.MaxBet) > formatInputNumber(modelData.MaxPerMatch)) {
                        message = languageService.getString("MaxPerMatchMoreThanOrEqualMaxBet");
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
                isValid: function () {
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