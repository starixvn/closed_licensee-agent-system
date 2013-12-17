/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />





if (fanex.sportsbookSetting) {
    //// Helper

    fanex.sportsbookSetting.positionTakingService = {
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
                currentGroup.TitleWidth = (i < 2) ? (self.config.headerBoxWidth + ptLength * self.config.ptBoxWidth + ptLength) : self.config.collapseWidth;
                currentGroup.ExpandWidth = self.config.headerBoxWidth + ptLength * self.config.ptBoxWidth + ptLength;
                currentGroup.WidthStyle = {
                    width: currentGroup.TitleWidth + 'px'
                };

                currentGroup.Name = language["sport_" + currentGroup.SportId + "_" + currentGroup.IsLive + "_" + currentGroup.PositionTakings[0].BetTypeId];
            }

            positionTakingSetting[0].Expand = true;
            positionTakingSetting[1].Expand = true;
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
                    && currentPt.SportId == pt.SportId
                    && currentPt.BetTypeId == pt.BetTypeId) {
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

            if (pt.SportId == 1) {
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
                    for (var j = (i == fromIndex ? 1 : 0); j < ptListLength; j++) {
                        var currentPtNeedCopy = currentGroup.PositionTakings[j];
                        self.copyAction(currentPtNeedCopy, pt);
                    }

                    if (pt.SportId != currentGroup.SportId) {
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
                if (currentPtNeedCopy.actions.onChangePreset) {
                    currentPtNeedCopy.actions.onChangePreset();
                }
                currentPtNeedCopy.Max = pt.Max;
                if (currentPtNeedCopy.actions.onChangeMax) {
                    currentPtNeedCopy.actions.onChangeMax();
                }
                currentPtNeedCopy.Min = pt.Min;
                if (currentPtNeedCopy.actions.onChangeMin) {
                    currentPtNeedCopy.actions.onChangeMin();
                }
                currentPtNeedCopy.Value = pt.Value;
                if (currentPtNeedCopy.actions.onChangeValue) {
                    currentPtNeedCopy.actions.onChangeValue();
                }
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
        getPositionTakingUpdate: function (settingContext, simpleModeData, advancedModeData) {
            var self = this;
            if (settingContext.IsSimpleMode) {
                var updatedPt = simpleModeData,
                    datalength = positionTakingSetting.length,
                    positionTakingSave = [];
                for (var i = 0; i < datalength; i++) {
                    var currentGroup = positionTakingSetting[i];
                    if (currentGroup) {
                        var groupLength = currentGroup.PositionTakings.length;
                        for (var j = 0; j < groupLength; j++) {
                            var currentPositionTaking = currentGroup.PositionTakings[j];
                            self.getSimpleModePositionTaking(settingContext, updatedPt, currentPositionTaking, positionTakingSave);
                        }
                    }
                }
            }
            else {
                var updatedData = advancedModeData,
                    datalength = updatedData.length,
                    positionTakingSave = [];
                for (var i = 0; i < datalength; i++) {
                    var currentGroup = updatedData[i];
                    if (currentGroup) {
                        for (var j = 0; j < currentGroup.PositionTakings.length; j++) {
                            var currentPositionTaking = currentGroup.PositionTakings[j];
                            self.getAdvancedModePositionTaking(settingContext, currentPositionTaking, positionTakingSave);
                        }

                    }

                }
            }

            return positionTakingSave;
        },
        getSimpleModePositionTaking: function (settingContext, updatedPt, currentPositionTaking, positionTakingSave) {
            if (settingContext.TargetRoleId == fanex.sportsbookSetting.Role.Member) {
                if (currentPositionTaking.Value != updatedPt.Value
                || settingContext.FormId == fanex.sportsbookSetting.FormType.Add) {
                    positionTakingSave.push({
                        Value: updatedPt.Value,
                        SportId: currentPositionTaking.SportId,
                        BetTypeId: currentPositionTaking.BetTypeId,
                        IsLive: currentPositionTaking.IsLive
                    });
                }
            }
            else {
                if (currentPositionTaking.Min != updatedPt.Min
                    || currentPositionTaking.Max != updatedPt.Max
                    || currentPositionTaking.Preset != updatedPt.Preset
                    || settingContext.FormId == fanex.sportsbookSetting.FormType.Add) {
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
            if (settingContext.TargetRoleId == fanex.sportsbookSetting.Role.Member) {
                if (currentPositionTaking.Value != currentPositionTaking.OldValue
                    || settingContext.FormId == fanex.sportsbookSetting.FormType.Add) {
                    positionTakingSave.push({
                        Value: currentPositionTaking.Value,
                        SportId: currentPositionTaking.SportId,
                        BetTypeId: currentPositionTaking.BetTypeId,
                        IsLive: currentPositionTaking.IsLive
                    });
                }
            }
            else {
                if (currentPositionTaking.Min != currentPositionTaking.OldMin
                    || currentPositionTaking.Max != currentPositionTaking.OldMax
                    || currentPositionTaking.Preset != currentPositionTaking.OldPreset
                    || settingContext.FormId == fanex.sportsbookSetting.FormType.Add) {
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
            else {

                var updatedData = advancedModeData,
                    datalength = updatedData.length,
                    positionTakingSave = [];

                for (var i = 0; i < datalength; i++) {
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
            }

            return true;
        }
    };

}

