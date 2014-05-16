/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/* global sportsBookSetting */
'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.positionTakingService = {
        init: function (positionTakingSetting, language) {
            var self = this,
                groupLength = positionTakingSetting.length;

            for (var i = 0; i < groupLength; i++) {
                var currentGroup = positionTakingSetting[i];
                var ptLength = currentGroup.PositionTakings.length;
                if (self.config.breakNewLine < ptLength) {
                    ptLength = self.config.breakNewLine;
                }
                currentGroup.TitleWidth = (i < 2) ? (self.config.headerBoxWidth + ptLength * self.config.ptBoxWidth + ptLength) : self.config.collapseWidth;
                currentGroup.ExpandWidth = self.config.headerBoxWidth + ptLength * self.config.ptBoxWidth + ptLength;
                currentGroup.WidthStyle = {
                    width: currentGroup.TitleWidth + 'px'
                };

                currentGroup.Name = language["sport_" + currentGroup.SportId + "_" + currentGroup.BetTypeId];
            }

            positionTakingSetting[0].Expand = true;
        },
        getPositionTakingUpdate: function (settingContext, simpleModeData, advancedModeData) {
            var self = this, dataLength, indexData, indexGroup, currentGroup, currentPositionTaking, positionTakingSave = [];
            if (settingContext.IsSimpleMode) {
                var updatedPt = simpleModeData;
                dataLength = sportsBookSetting.PositionTakingSetting.length;
                for (indexData = 0; indexData < dataLength; indexData++) {
                    currentGroup = sportsBookSetting.PositionTakingSetting[indexData];
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
        },
        copyVirtualSoccer: function (fromIndex, ptGroup, ptGroups, lang, scope) {
            var pt = ptGroup.PositionTakings[0];
            if (!pt.actions.isValid()) {
                return;
            }

            var self = this,
                groupLength = ptGroups.length,
                copyAllMessage = [];

            if (!ptGroup.Expand) {
                scope.positionTakingService.onCollapseExpand(ptGroup);
            }

            copyAllMessage.push(lang.virtualSoccerCopyPt);
            copyAllMessage.push('<ul>');
            copyAllMessage.push('<li>' + lang.otherSporttypeOfVirtualSoccer + '</li>');

            for (var i = fromIndex; i < groupLength; i++) {
                var currentGroup = ptGroups[i];
                if (currentGroup) {
                    var ptListLength = currentGroup.PositionTakings.length;
                    for (var j = (i === fromIndex ? 1 : 0); j < ptListLength; j++) {
                        var currentPtNeedCopy = currentGroup.PositionTakings[j];
                        self.copyAction(currentPtNeedCopy, pt);
                    }
                }
            }
            copyAllMessage.push('</ul>');

            //// Show Message Copy All
            setTimeout(function () {
                self.showCopyAllMessage(
                            $('#copy_sport_' + pt.SportId + '_bettype_' + pt.BetTypeId),
                            copyAllMessage.join(''));
            }, 100);
        }
    };

    angular.extend(fanex.customerSetting.positionTakingService, fanex.customerSetting.basePositionTakingService);
}