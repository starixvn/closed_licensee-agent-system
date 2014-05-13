'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.positionTakingActions =
    {
        agentEditAgentActions: function (scope, $timeout, currentPt, presetInput, maxInput, minInput, formatter) {
            return {
                isDataChanged: function () {
                    return currentPt.Min !== currentPt.OldMin
                        || currentPt.Max !== currentPt.OldMax
                        || currentPt.Preset !== currentPt.OldPreset;
                },
                onChangePreset: function () {
                    if (currentPt.actions.validPreset(true)) {
                        fanex.hideHint();
                    }
                },
                onBlurPreset: function () {
                    if (!currentPt.actions.validPreset(false)) {
                        return;
                    }

                    currentPt.Max = fanex.roundNumber(currentPt.Upline.Max - currentPt.Preset, 2);
                    currentPt.Min = Math.min(currentPt.Min, fanex.roundNumber(currentPt.Max, 2));
                    currentPt.actions.whenPresetChange();
                    fanex.hideHint();
                },
                onChangeMax: function () {
                    if (currentPt.actions.validMax(true)) {
                        fanex.hideHint();
                    }
                },
                onBlurMax: function () {
                    if (!currentPt.actions.validMax(false)) {
                        return;
                    }

                    currentPt.Preset = fanex.roundNumber(currentPt.Preset, 2);
                    currentPt.Min = Math.min(currentPt.Min, fanex.roundNumber(currentPt.Max, 2));
                    currentPt.MaxRangeOfMin = currentPt.Max;
                    fanex.hideHint();
                },
                onChangeMin: function () {
                    currentPt.Preset = fanex.roundNumber(currentPt.Preset, 2);
                    currentPt.Max = fanex.roundNumber(currentPt.Max, 2);

                    if (!currentPt.actions.validMin(true)) {
                        return;
                    }
                    fanex.hideHint();
                },
                whenPresetChange: function () {
                    currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max, currentPt.Upline.Min - currentPt.Preset);
                    currentPt.MaxRangeOfMax = fanex.roundNumber(Math.max(0, currentPt.Upline.Max - currentPt.Preset), 2);
                    currentPt.MinRangeOfMin = 0;
                    currentPt.MaxRangeOfMin = currentPt.Max;
                },
                isValid: function () {
                    return currentPt.actions.validPreset(true) && currentPt.actions.validMax(true) && currentPt.actions.validMin(true);
                },
                validPreset: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Preset,2) >= fanex.roundNumber(currentPt.MinRangeOfPreset,2) &&
                        fanex.roundNumber(currentPt.Preset,2) <= fanex.roundNumber(currentPt.MaxRangeOfPreset,2);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.expandGroup(scope.$parent.$parent.ptGroup);
                        $timeout(function () {
                            fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                                presetInput,
                                scope.language["message_ValueIsNotInRange"],
                                formatter.formatNumber(currentPt.MinRangeOfPreset),
                                formatter.formatNumber(currentPt.MaxRangeOfPreset));
                        });
                    }
                    return isValid;
                },
                validMax: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Max,2) >= fanex.roundNumber(currentPt.MinRangeOfMax,2) &&
                        fanex.roundNumber(currentPt.Max,2) <= fanex.roundNumber(currentPt.MaxRangeOfMax,2);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.expandGroup(scope.$parent.$parent.ptGroup);
                        $timeout(function () {
                            fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                                maxInput,
                                scope.language["message_ValueIsNotInRange"],
                                formatter.formatNumber(currentPt.MinRangeOfMax),
                                formatter.formatNumber(currentPt.MaxRangeOfMax));
                        });
                    }
                    return isValid;
                },
                validMin: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Min,2) >= fanex.roundNumber(currentPt.MinRangeOfMin,2) &&
                        fanex.roundNumber(currentPt.Min,2) <= fanex.roundNumber(currentPt.MaxRangeOfMin,2);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.expandGroup(scope.$parent.$parent.ptGroup);
                        $timeout(function () {
                            fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                                minInput,
                                scope.language["message_ValueIsNotInRange"],
                                formatter.formatNumber(currentPt.MinRangeOfMin),
                                formatter.formatNumber(currentPt.MaxRangeOfMin));
                        });
                    }
                    return isValid;
                }
            };
        },
        licenseeEditAgentOneActions: function (scope, $timeout, currentPt, maxInput, minInput, formatter) {
            return {
                isDataChanged: function () {
                    return currentPt.Min !== currentPt.OldMin
                        || currentPt.Max !== currentPt.OldMax;
                },
                onChangeMax: function () {
                    if (!currentPt.actions.validMax(true)) {
                        return;
                    }
                    fanex.hideHint();
                },
                onBlurMax: function () {
                    if (!currentPt.actions.validMax(false)) {
                        return;
                    }

                    currentPt.Preset = fanex.roundNumber(100 - currentPt.Max, 2);
                    currentPt.MaxRangeOfMin = currentPt.Max;
                    currentPt.Min = fanex.roundNumber(Math.min(currentPt.Min, currentPt.Max), 2);
                    fanex.hideHint();
                },
                onChangeMin: function () {
                    currentPt.Preset = fanex.roundNumber(currentPt.Preset, 2);
                    currentPt.Max = fanex.roundNumber(currentPt.Max, 2);

                    if (!currentPt.actions.validMin(true)) {
                        return;
                    }
                    fanex.hideHint();
                },
                isValid: function () {
                    return currentPt.actions.validMax(true) && currentPt.actions.validMin(true);
                },
                validMax: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Max,2) >= fanex.roundNumber(currentPt.MinRangeOfMax,2) &&
                        fanex.roundNumber(currentPt.Max,2) <= fanex.roundNumber(currentPt.MaxRangeOfMax,2);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.expandGroup(scope.$parent.$parent.ptGroup);
                        $timeout(function () {
                            fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                                maxInput,
                                scope.language["message_ValueIsNotInRange"],
                                formatter.formatNumber(currentPt.MinRangeOfMax),
                                formatter.formatNumber(currentPt.MaxRangeOfMax));
                        });
                    }
                    return isValid;
                },
                validMin: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Min,2) >= fanex.roundNumber(currentPt.MinRangeOfMin,2) &&
                        fanex.roundNumber(currentPt.Min,2) <= fanex.roundNumber(currentPt.MaxRangeOfMin,2);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.expandGroup(scope.$parent.$parent.ptGroup);
                        $timeout(function () {
                            fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                                minInput,
                                scope.language["message_ValueIsNotInRange"],
                                formatter.formatNumber(currentPt.MinRangeOfMin),
                                formatter.formatNumber(currentPt.MaxRangeOfMin));
                        });
                    }
                    return isValid;
                }
            };
        },
        agentEditMemberActions: function (scope, $timeout, currentPt, valueInput, formatter) {
            return {
                isDataChanged: function () {
                    return currentPt.Value !== currentPt.OldValue;
                },
                onChangeValue: function () {
                    if (!currentPt.actions.validValue(true)) {
                        return;
                    }
                    fanex.hideHint();
                },
                isValid: function () {
                    return currentPt.actions.validValue(true);
                },
                validValue: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Value,2) >= fanex.roundNumber(currentPt.MinRangeOfValue,2) &&
                        fanex.roundNumber(currentPt.Value,2) <= fanex.roundNumber(currentPt.MaxRangeOfValue,2);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.expandGroup(scope.$parent.$parent.ptGroup);
                        $timeout(function () {
                            fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                                valueInput,
                                scope.language["message_ValueIsNotInRange"],
                                formatter.formatNumber(currentPt.MinRangeOfValue),
                                formatter.formatNumber(currentPt.MaxRangeOfValue));
                        });
                    }
                    return isValid;
                }
            };
        }
    };
}