/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.numberGameBetLimitService = {
        config: {
            maxInput: 15
        },
        copyPartial: function(betSetting, index, bettingLimitsModel) {
            if (!betSetting.actions.isValid()) {
                return;
            }

            var self = this;
            for (var i = index + 1; i < bettingLimitsModel.length; i++) {
                self.copyAction(betSetting, bettingLimitsModel[i]);
            }
        },
        copyAction: function(source, destination) {
            destination.MinBet = fanex.customerSetting.numberGameBetLimitService.reformatInputNumberForControl(source.MinBet);
            destination.actions.validMinBet();

            destination.MaxBet = fanex.customerSetting.numberGameBetLimitService.reformatInputNumberForControl(source.MaxBet);
            destination.actions.validMaxBet();

            destination.MaxPerMatch = fanex.customerSetting.numberGameBetLimitService.reformatInputNumberForControl(source.MaxPerMatch);
            destination.actions.validMaxPerMatch();
        },
        showInvalidMessage: function(element, message) {
            fanex.showHint(element, message);
        },
        getbettingLimits: function(settingContext, bettingLimitsData, firstRecordData) {
            var bettingLimitsSave = [], self = this, index, bettingLimit;

            if (settingContext.IsSimpleMode) {
                for (index = 0; index < bettingLimitsData.length; index++) {
                    bettingLimit = bettingLimitsData[index];
                    if (self.formatInputNumber(bettingLimit.MinBet) !== self.formatInputNumber(firstRecordData.MinBet)
                        || self.formatInputNumber(bettingLimit.MaxBet) !== self.formatInputNumber(firstRecordData.MaxBet)
                        || self.formatInputNumber(bettingLimit.MaxPerMatch) !== self.formatInputNumber(firstRecordData.MaxPerMatch)
                        || settingContext.FormId === fanex.customerSetting.FormType.Add) {
                        bettingLimitsSave.push({
                            MinBet: self.formatInputNumber(firstRecordData.MinBet),
                            MaxBet: self.formatInputNumber(firstRecordData.MaxBet),
                            MaxPerMatch: self.formatInputNumber(firstRecordData.MaxPerMatch),
                            SportId: bettingLimit.SportId,
                            BetTypeId: bettingLimit.BetTypeId
                        });
                    }
                }
            } else {
                for (index = 0; index < bettingLimitsData.length; index++) {
                    bettingLimit = bettingLimitsData[index];
                    if (bettingLimit.actions.isDataChanged()
                        || settingContext.FormId === fanex.customerSetting.FormType.Add) {
                        bettingLimitsSave.push({
                            MinBet: self.formatInputNumber(bettingLimit.MinBet),
                            MaxBet: self.formatInputNumber(bettingLimit.MaxBet),
                            MaxPerMatch: self.formatInputNumber(bettingLimit.MaxPerMatch),
                            SportId: bettingLimit.SportId,
                            BetTypeId: bettingLimit.BetTypeId
                        });
                    }
                }
            }

            return bettingLimitsSave;
        },
        isValidData: function(settingContext, bettingLimitsData, firstRecordData) {
            if (settingContext.IsSimpleMode) {
                return firstRecordData.actions.isValid();
            } else {
                var datalength = bettingLimitsData.length;

                for (var i = 0; i < datalength; i++) {
                    var bettingLimit = bettingLimitsData[i];
                    if (!bettingLimit.actions.isValid()) {
                        return false;
                    }
                }
            }

            return true;
        },
        regIsDigit: function(input) {
            var reg = new RegExp("^[0-9,]+$");
            return (reg.test(input));
        },
        formatInputNumber: function(input) {
            return parseInt(input.toString().replace(/,/gi, ''));
        },
        reformatInputNumberForControl: function(input) {
            var result = "";
            if (input === undefined) {
                return result;
            }
            input = input.toString().replace(/,/gi, '');
            if (input.length > 3) {
                for (var i = input.length - 3; i > 0; i = i - 3) {
                    var end = i + 3;
                    result = ',' + input.substring(i, end) + result;
                }

                result = input.substring(0, i + 3) + result;
            } else {
                result = input;
            }
            return result;
        }
    };
}