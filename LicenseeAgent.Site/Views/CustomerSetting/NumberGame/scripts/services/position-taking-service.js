/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global sportsBookSetting */
'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.numberGamePositionTakingService = {
        config: {
            decNumber: 2,
            breakNewLine: 5,
            maxInput: 5,
            headerBoxWidth: 160,
            ptBoxWidth: 150,
            collapseWidth: 280
        },
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
        copyPartial: function (pt, ptGroup, breakNewLine) {
            if (!pt.actions.isValid()) {
                return;
            }

            var self = this,
                length = ptGroup.PositionTakings.length;
            for (var i = 0; i < length; i++) {
                var currentPt = ptGroup.PositionTakings[i];
                if (currentPt
                    && currentPt.SportId === pt.SportId
                    && currentPt.BetTypeId === pt.BetTypeId) {
                    var newLength = i + breakNewLine - 1;
                    newLength = Math.min(newLength, length);
                    for (var j = i + 1; j <= newLength; j++) {
                        var currentPtNeedCopy = ptGroup.PositionTakings[j];
                        self.copyAction(currentPtNeedCopy, pt);
                    }
                }
            }
        },
        copyAll: function (fromIndex, ptGroup, ptGroups, lang, scope) {
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

            if (pt.SportId === 1) {
                copyAllMessage.push(lang.soccerHandicapCopyPt);
                copyAllMessage.push('<ul>');
                copyAllMessage.push('<li>' + lang.otherBettypeOfSoccer + '</li>');
            }
            else {
                copyAllMessage.push(lang.basketballHandicapCopyPt);
                copyAllMessage.push('<ul>');
                copyAllMessage.push('<li>' + lang.otherBettypeOfBasketball + '</li>');
            }

            for (var i = fromIndex; i < groupLength; i++) {
                var currentGroup = ptGroups[i];
                if (currentGroup) {
                    var ptListLength = currentGroup.PositionTakings.length;
                    for (var j = (i === fromIndex ? 1 : 0); j < ptListLength; j++) {
                        var currentPtNeedCopy = currentGroup.PositionTakings[j];
                        self.copyAction(currentPtNeedCopy, pt);
                    }

                    if (pt.SportId !== currentGroup.SportId) {
                        copyAllMessage.push('<li>' + currentGroup.Name + '</li>');
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
        },
        copyAction: function (currentPtNeedCopy, pt) {
            if (currentPtNeedCopy) {
                currentPtNeedCopy.Preset = pt.Preset;
                if (currentPtNeedCopy.actions.onBlurPreset) {
                    currentPtNeedCopy.actions.onBlurPreset();
                }
                currentPtNeedCopy.Max = pt.Max;
                if (currentPtNeedCopy.actions.onBlurMax) {
                    currentPtNeedCopy.actions.onBlurMax();
                }
                currentPtNeedCopy.Min = pt.Min;
                if (currentPtNeedCopy.actions.onChangeMin) {
                    currentPtNeedCopy.actions.onChangeMin();
                }
                currentPtNeedCopy.Value = pt.Value;
                if (currentPtNeedCopy.actions.onChangeValue) {
                    currentPtNeedCopy.actions.onChangeValue();
                }
                currentPtNeedCopy.ApplyAll = pt.ApplyAll;
            }
        },
        showInvalidRangeMessage: function (element, message, minValue, maxValue) {
            fanex.showHint(element, message + ' ['
                                        + minValue
                                        + ' - '
                                        + maxValue
                                        + ']');
            element.focus();
        },
        showCopyAllMessage: function (element, message) {
            fanex.showHint(element, message);
        },
        onCollapseExpand: function (ptGroup) {
            var self = this;
            if (ptGroup.Expand) {
                ptGroup.TitleWidth = self.config.collapseWidth;
            }
            else {
                ptGroup.TitleWidth = ptGroup.ExpandWidth;
            }
            ptGroup.WidthStyle.width = ptGroup.TitleWidth + 'px';
            ptGroup.Expand = !ptGroup.Expand;
        },
        expandGroup: function (ptGroup) {
            if (angular.isDefined(ptGroup)) {
                if (!ptGroup.Expand) {
                    fanex.customerSetting.positionTakingService.onCollapseExpand(ptGroup);
                }
            }
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
        getSimpleModePositionTaking: function (settingContext, updatedPt, currentPositionTaking, positionTakingSave) {
            if (settingContext.TargetRoleId === fanex.customerSetting.Role.Member) {
                if (currentPositionTaking.Value !== updatedPt.Value
                || settingContext.FormId === fanex.customerSetting.FormType.Add) {
                    positionTakingSave.push({
                        Value: updatedPt.Value,
                        SportId: currentPositionTaking.SportId,
                        BetTypeId: currentPositionTaking.BetTypeId,
                        IsLive: currentPositionTaking.IsLive
                    });
                }
            }
            else {
                if (currentPositionTaking.Min !== updatedPt.Min
                    || currentPositionTaking.Max !== updatedPt.Max
                    || currentPositionTaking.Preset !== updatedPt.Preset
                    || updatedPt.ApplyAll
                    || settingContext.FormId === fanex.customerSetting.FormType.Add) {
                    positionTakingSave.push({
                        Min: updatedPt.Min,
                        Max: updatedPt.Max,
                        Preset: updatedPt.Preset,
                        SportId: currentPositionTaking.SportId,
                        BetTypeId: currentPositionTaking.BetTypeId,
                        IsLive: currentPositionTaking.IsLive,
                        ApplyAll: updatedPt.ApplyAll
                    });
                }
            }
        },
        getAdvancedModePositionTaking: function (settingContext, currentPositionTaking, positionTakingSave) {
            if (settingContext.TargetRoleId === fanex.customerSetting.Role.Member) {
                if (currentPositionTaking.Value !== currentPositionTaking.OldValue
                    || settingContext.FormId === fanex.customerSetting.FormType.Add) {
                    positionTakingSave.push({
                        Value: currentPositionTaking.Value,
                        SportId: currentPositionTaking.SportId,
                        BetTypeId: currentPositionTaking.BetTypeId,
                        IsLive: currentPositionTaking.IsLive
                    });
                }
            }
            else {
                if (currentPositionTaking.Min !== currentPositionTaking.OldMin
                    || currentPositionTaking.Max !== currentPositionTaking.OldMax
                    || currentPositionTaking.Preset !== currentPositionTaking.OldPreset
                    || currentPositionTaking.ApplyAll
                    || settingContext.FormId === fanex.customerSetting.FormType.Add) {
                    positionTakingSave.push({
                        Min: currentPositionTaking.Min,
                        Max: currentPositionTaking.Max,
                        Preset: currentPositionTaking.Preset,
                        SportId: currentPositionTaking.SportId,
                        BetTypeId: currentPositionTaking.BetTypeId,
                        IsLive: currentPositionTaking.IsLive,
                        ApplyAll: currentPositionTaking.ApplyAll
                    });
                }
            }
        },
        isValidData: function (settingContext, simpleModeData, advancedModeData) {
            if (settingContext.IsSimpleMode) {
                return simpleModeData.actions.isValid();
            }

            var updatedData = advancedModeData,
                dataLength = updatedData.length;

            for (var i = 0; i < dataLength; i++) {
                var currentGroup = updatedData[i];
                if (currentGroup) {
                    for (var j = 0; j < currentGroup.PositionTakings.length; j++) {
                        var currentPositionTaking = currentGroup.PositionTakings[j];
                        if (!currentPositionTaking.actions.isValid()) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    };
}