/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global numberGameSetting */
'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.numberGamePositionTakingService = {
        init: function (positionTakingSetting, language) {
            var self = this,
                groupLength = positionTakingSetting.length;

            for (var i = 0; i < groupLength; i++) {
                var currentGroup = positionTakingSetting[i];
                var ptLength = currentGroup.PositionTakings.length;
                if (self.config.breakNewLine < ptLength) {
                    ptLength = self.config.breakNewLine;
                }
                currentGroup.TitleWidth = self.config.collapseWidth;
                currentGroup.ExpandWidth = self.config.headerBoxWidth + ptLength * self.config.ptBoxWidth + ptLength;
                currentGroup.WidthStyle = {
                    width: currentGroup.TitleWidth + 'px'
                };
                currentGroup.Name = language["sport_" + currentGroup.SportId + "_" + currentGroup.IsLive + "_" + currentGroup.PositionTakings[0].BetTypeId];
                currentGroup.Expand = false;
            }
        },
        getPositionTakingUpdate: function (settingContext, simpleModeData, advancedModeData) {
            var self = this, dataLength, indexData, indexGroup, currentGroup, currentPositionTaking, positionTakingSave = [];
            if (settingContext.IsSimpleMode) {
                var updatedPt = simpleModeData;
                dataLength = numberGameSetting.PositionTakingSetting.length;
                for (indexData = 0; indexData < dataLength; indexData++) {
                    currentGroup = numberGameSetting.PositionTakingSetting[indexData];
                    if (currentGroup) {
                        var groupLength = currentGroup.PositionTakings.length;
                        for (indexGroup = 0; indexGroup < groupLength; indexGroup++) {
                            currentPositionTaking = currentGroup.PositionTakings[indexGroup];
                            self.getSimpleModePositionTaking(settingContext, updatedPt, currentPositionTaking, positionTakingSave);
                        }
                    }
                }
            } else {
                var updatedData = advancedModeData;
                dataLength = updatedData.length;
                for (indexData = 0; indexData < dataLength; indexData++) {
                    currentGroup = updatedData[indexData];
                    if (currentGroup) {
                        for (indexGroup = 0; indexGroup < currentGroup.PositionTakings.length; indexGroup++) {
                            currentPositionTaking = currentGroup.PositionTakings[indexGroup];
                            self.getAdvancedModePositionTaking(settingContext, currentPositionTaking, positionTakingSave);
                        }
                    }
                }
            }
            return positionTakingSave;
        }
    };

    angular.extend(fanex.customerSetting.numberGamePositionTakingService, fanex.customerSetting.basePositionTakingService);
}