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

            destination.MaxPerBall = fanex.customerSetting.numberGameBetLimitService.reformatInputNumberForControl(source.MaxPerBall);
            destination.actions.validMaxPerBall();
        },
        getbettingLimits: function(settingContext, bettingLimitsData, firstRecordData) {
            var bettingLimitsSave = [], self = this, index, bettingLimit;

            if (settingContext.IsSimpleMode) {
                for (index = 0; index < bettingLimitsData.length; index++) {
                    bettingLimit = bettingLimitsData[index];
                    if (self.formatInputNumber(bettingLimit.MinBet) !== self.formatInputNumber(firstRecordData.MinBet)
                        || self.formatInputNumber(bettingLimit.MaxBet) !== self.formatInputNumber(firstRecordData.MaxBet)
                        || self.formatInputNumber(bettingLimit.MaxPerMatch) !== self.formatInputNumber(firstRecordData.MaxPerMatch)
                        || self.formatInputNumber(bettingLimit.MaxPerBall) !== self.formatInputNumber(firstRecordData.MaxPerBall)
                        || settingContext.FormId === fanex.customerSetting.FormType.Add) {
                        bettingLimitsSave.push({
                            MinBet: self.formatInputNumber(firstRecordData.MinBet),
                            MaxBet: self.formatInputNumber(firstRecordData.MaxBet),
                            MaxPerMatch: self.formatInputNumber(firstRecordData.MaxPerMatch),
                            MaxPerBall: self.formatInputNumber(firstRecordData.MaxPerBall),
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
                            MaxPerBall: self.formatInputNumber(bettingLimit.MaxPerBall),
                            SportId: bettingLimit.SportId,
                            BetTypeId: bettingLimit.BetTypeId
                        });
                    }
                }
            }

            return bettingLimitsSave;
        }
    };

    angular.extend(fanex.customerSetting.numberGameBetLimitService, fanex.customerSetting.baseBetLimitService);
}