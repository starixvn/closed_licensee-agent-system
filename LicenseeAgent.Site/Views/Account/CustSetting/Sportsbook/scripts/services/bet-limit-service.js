/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.sportsbookSetting) {
    //// Helper

    fanex.sportsbookSetting.betLimitService = {
        config: {
            maxInput: 15
        },
        copyPartial: function (betSetting, index, bettingLimitsModel) {
            if (!betSetting.actions.isValid()) {
                return;
            }

            var self = this;
            for (var i = index + 1; i < bettingLimitsModel.length; i++) {
                self.copyAction(betSetting, bettingLimitsModel[i]);
            }
        },
        copyAction: function (source, destination) {
            destination.MinBet = fanex.sportsbookSetting.betLimitService.reformatInputNumberForControl(source.MinBet);
            destination.actions.validMinBet();

            destination.MaxBet = fanex.sportsbookSetting.betLimitService.reformatInputNumberForControl(source.MaxBet);
            destination.actions.validMaxBet();

            destination.MaxPerMatch = fanex.sportsbookSetting.betLimitService.reformatInputNumberForControl(source.MaxPerMatch);
            destination.actions.validMaxPerMatch();
        },
        showInvalidMessage: function (element, message) {
            fanex.showHint(element, message);
        },
        refreshData: function (settingContext, bettingLimitsData, firstRecordData) {
            if (settingContext.IsSimpleMode) {
                for (var i = 0; i < bettingLimitsData.length; i++) {
                    var bettingLimit = bettingLimitsData[i];
                    if (typeof firstRecordData.MinBet != 'undefined') firstRecordData.MinBet = bettingLimit.MinBet;
                    if (typeof firstRecordData.MaxBet != 'undefined') firstRecordData.MaxBet = bettingLimit.MaxBet;
                    if (typeof firstRecordData.MaxPerMatch != 'undefined') firstRecordData.MaxPerMatch = bettingLimit.MaxPerMatch;
                }
            }
            else {
                for (var i = 0; i < bettingLimitsData.length; i++) {
                    var bettingLimit = bettingLimitsData[i];
                    if (typeof bettingLimit.OldMinBet != 'undefined') bettingLimit.OldMinBet = bettingLimit.MinBet;
                    if (typeof bettingLimit.OldMaxBet != 'undefined') bettingLimit.OldMaxBet = bettingLimit.MaxBet;
                    if (typeof bettingLimit.OldMaxPerMatch != 'undefined') bettingLimit.OldMaxPerMatch = bettingLimit.MaxPerMatch;
                }
            }
        },
        getbettingLimits: function (settingContext, bettingLimitsData, firstRecordData) {
            var bettingLimitsSave = [];
            var self = this;

            if (settingContext.IsSimpleMode) {
                for (var i = 0; i < bettingLimitsData.length; i++) {
                    var bettingLimit = bettingLimitsData[i];
                    if (self.formatInputNumber(bettingLimit.MinBet) != self.formatInputNumber(firstRecordData.MinBet)
                    || self.formatInputNumber(bettingLimit.MaxBet) != self.formatInputNumber(firstRecordData.MaxBet)
                    || self.formatInputNumber(bettingLimit.MaxPerMatch) != self.formatInputNumber(firstRecordData.MaxPerMatch)
                    || settingContext.FormId == fanex.sportsbookSetting.FormType.Add) {
                        bettingLimitsSave.push({
                            MinBet: self.formatInputNumber(firstRecordData.MinBet),
                            MaxBet: self.formatInputNumber(firstRecordData.MaxBet),
                            MaxPerMatch: self.formatInputNumber(firstRecordData.MaxPerMatch),
                            SportId: bettingLimit.SportId,
                            BetTypeId: bettingLimit.BetTypeId
                        });
                    }
                }
            }
            else {
                for (var i = 0; i < bettingLimitsData.length; i++) {
                    var bettingLimit = bettingLimitsData[i];
                    if (bettingLimit.actions.isDataChanged()
                    || settingContext.FormId == fanex.sportsbookSetting.FormType.Add) {
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
        isValidData: function (settingContext, bettingLimitsData, firstRecordData) {
            if (settingContext.IsSimpleMode) {
                return firstRecordData.actions.isValid();
            }
            else {
                var updatedData = bettingLimitsData,
                    datalength = bettingLimitsData.length;

                for (var i = 0; i < datalength; i++) {
                    var bettingLimit = bettingLimitsData[i];
                    if (!bettingLimit.actions.isValid()) {
                        return false;
                    }
                }
            }

            return true;
        },
        regIsDigit: function (input) {
            var reg = new RegExp("^[0-9,]+$");
            return (reg.test(input));
        },
        formatInputNumber: function (input) {
            return parseInt(input.toString().replace(/,/gi, ''));
        },
        reformatInputNumberForControl: function (input) {
            input = input.toString().replace(/,/gi, '');
            var result = "";

            if (input.length > 3) {
                for (var i = input.length - 3; i > 0; i = i - 3) {
                    var end = i + 3;
                    result = ',' + input.substring(i, end) + result;
                }

                result = input.substring(0, i + 3) + result;
            }
            else {
                result = input;
            }

            return result;
        }
    };
}