/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.customerSetting) {
    fanex.customerSetting.positionTakingActions =
    {
        agentEditAgentActions: function (scope, currentPt, presetInput, maxInput, minInput, controller) {
            var self = this;
            return {
                isDataChanged: function () {
                    return currentPt.Min != currentPt.OldMin
                                || currentPt.Max != currentPt.OldMax
                                || currentPt.Preset != currentPt.OldPreset;
                },
                onChangePreset: function () {
                    var endCharacter = currentPt.Preset.toString().charAt(currentPt.Preset.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Preset = fanex.roundNumber(currentPt.Preset, 2);
                    }

                    if (currentPt.actions.validPreset(true)) {
                        fanex.hideHint();
                    }
                },
                onBlurPreset: function () {
                    var endCharacter = currentPt.Preset.toString().charAt(currentPt.Preset.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Preset = fanex.roundNumber(currentPt.Preset, 2);
                    }

                    if (!currentPt.actions.validPreset(false)) {
                        return;
                    }

                    currentPt.Max = fanex.roundNumber(currentPt.Upline.Max - currentPt.Preset, 2);
                    currentPt.Min = Math.min(currentPt.Min, fanex.roundNumber(currentPt.Max, 2));
                    currentPt.actions.whenPresetChange();
                    fanex.hideHint();
                },
                onChangeMax: function () {
                    var endCharacter = currentPt.Max.toString().charAt(currentPt.Max.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Max = fanex.roundNumber(currentPt.Max, 2);
                    }
                    if (currentPt.actions.validMax(true)) {
                        fanex.hideHint();
                    }
                },
                onBlurMax: function () {
                    var endCharacter = currentPt.Max.toString().charAt(currentPt.Max.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Max = fanex.roundNumber(currentPt.Max, 2);
                    }

                    if (!currentPt.actions.validMax(false)) {
                        return;
                    }

                    currentPt.Preset = fanex.roundNumber(currentPt.Preset, 2);
                    currentPt.Min = Math.min(currentPt.Min, fanex.roundNumber(currentPt.Max, 2));
                    currentPt.MaxRangeOfMin = currentPt.Max;
                    fanex.hideHint();
                },
                onChangeMin: function () {
                    var endCharacter = currentPt.Min.toString().charAt(currentPt.Min.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Min = fanex.roundNumber(currentPt.Min, 2);
                    }

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
                    var isValid = fanex.roundNumber(currentPt.Preset) >= fanex.roundNumber(currentPt.MinRangeOfPreset) && fanex.roundNumber(currentPt.Preset) <= fanex.roundNumber(currentPt.MaxRangeOfPreset);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                            presetInput,
                            scope.language["message_ValueIsNotInRange"],
                            controller.formatNumber(currentPt.MinRangeOfPreset),
                            controller.formatNumber(currentPt.MaxRangeOfPreset));
                    }
                    return isValid;
                },
                validMax: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Max) >= fanex.roundNumber(currentPt.MinRangeOfMax) && fanex.roundNumber(currentPt.Max) <= fanex.roundNumber(currentPt.MaxRangeOfMax);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                            maxInput,
                            scope.language["message_ValueIsNotInRange"],
                            controller.formatNumber(currentPt.MinRangeOfMax),
                            controller.formatNumber(currentPt.MaxRangeOfMax));
                    }
                    return isValid;
                },
                validMin: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Min) >= fanex.roundNumber(currentPt.MinRangeOfMin) && fanex.roundNumber(currentPt.Min) <= fanex.roundNumber(currentPt.MaxRangeOfMin);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                            minInput,
                            scope.language["message_ValueIsNotInRange"],
                            controller.formatNumber(currentPt.MinRangeOfMin),
                            controller.formatNumber(currentPt.MaxRangeOfMin));
                    }
                    return isValid;
                }
            };
        },
        licenseeEditAgentOneActions: function (scope, currentPt, maxInput, minInput, controller) {
            return {
                isDataChanged: function () {
                    return currentPt.Min != currentPt.OldMin
                                || currentPt.Max != currentPt.OldMax;
                },
                onChangeMax: function () {
                    var endCharacter = currentPt.Max.toString().charAt(currentPt.Max.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Max = fanex.roundNumber(currentPt.Max, 2);
                    }

                    if (!currentPt.actions.validMax(true)) {
                        return;
                    }
                    fanex.hideHint();
                },
                onBlurMax: function () {
                    var endCharacter = currentPt.Max.toString().charAt(currentPt.Max.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Max = fanex.roundNumber(currentPt.Max, 2);
                    }

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

                    var endCharacter = currentPt.Min.toString().charAt(currentPt.Min.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Min = fanex.roundNumber(currentPt.Min, 2);
                    }

                    if (!currentPt.actions.validMin(true)) {
                        return;
                    }
                    fanex.hideHint();
                },
                isValid: function () {
                    return currentPt.actions.validMax(true) && currentPt.actions.validMin(true);
                },
                validMax: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Max) >= fanex.roundNumber(currentPt.MinRangeOfMax) && fanex.roundNumber(currentPt.Max) <= fanex.roundNumber(currentPt.MaxRangeOfMax);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                            maxInput,
                            scope.language["message_ValueIsNotInRange"],
                            controller.formatNumber(currentPt.MinRangeOfMax),
                            controller.formatNumber(currentPt.MaxRangeOfMax));
                    }
                    return isValid;
                },
                validMin: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Min) >= fanex.roundNumber(currentPt.MinRangeOfMin) && fanex.roundNumber(currentPt.Min) <= fanex.roundNumber(currentPt.MaxRangeOfMin);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                            minInput,
                            scope.language["message_ValueIsNotInRange"],
                            controller.formatNumber(currentPt.MinRangeOfMin),
                            controller.formatNumber(currentPt.MaxRangeOfMin));
                    }
                    return isValid;
                }
            };
        },
        agentEditMemberActions: function (scope, currentPt, valueInput, controller) {
            return {
                isDataChanged: function () {
                    return currentPt.Value != currentPt.OldValue;
                },
                onChangeValue: function () {
                    var endCharacter = currentPt.Value.toString().charAt(currentPt.Value.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        currentPt.Value = fanex.roundNumber(currentPt.Value, 2);
                    }

                    if (!currentPt.actions.validValue(true)) {
                        return;
                    }
                    fanex.hideHint();
                },
                isValid: function () {
                    return currentPt.actions.validValue(true);
                },
                validValue: function (showHint) {
                    var isValid = fanex.roundNumber(currentPt.Value) >= fanex.roundNumber(currentPt.MinRangeOfValue) && fanex.roundNumber(currentPt.Value) <= fanex.roundNumber(currentPt.MaxRangeOfValue);
                    if (!isValid && showHint) {
                        fanex.customerSetting.positionTakingService.showInvalidRangeMessage(
                            valueInput,
                            scope.language["message_ValueIsNotInRange"],
                            controller.formatNumber(currentPt.MinRangeOfValue),
                            controller.formatNumber(currentPt.MaxRangeOfValue));
                    }
                    return isValid;
                }
            };
        }
    };

    //// Edit position taking advanced mode.
    fanex.customerSetting.directive('agentEditAgentPtAdvancedMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.pt;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(scope, currentPt, presetInput, maxInput, minInput, controller);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        }
    });

    fanex.customerSetting.directive('agentEditAgentPtAdvancedModeReadOnly', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode-readonly.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.pt;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(scope, currentPt, presetInput, maxInput, minInput, controller);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        }
    });

    fanex.customerSetting.directive('licenseeEditAgentOnePtAdvancedMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/licensee-edit-agent-one-pt-advanced-mode.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.pt;

                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.positionTakingActions.licenseeEditAgentOneActions(scope, currentPt, maxInput, minInput, controller);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
                currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
                currentPt.MinRangeOfMin = 0;
                currentPt.MaxRangeOfMin = currentPt.Max;
            }
        }
    });

    fanex.customerSetting.directive('agentEditMemberPtAdvancedMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-member-advanced-mode.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var valueInput = element.find('.value-input');
                var currentPt = scope.pt;
                valueInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditMemberActions(scope, currentPt, valueInput, controller);

                //// Init value
                currentPt.OldValue = currentPt.Value;
                currentPt.MinRangeOfValue = currentPt.Upline.Min;
                currentPt.MaxRangeOfValue = currentPt.Upline.Max;
            }
        }
    });

    //// Edit postion taking simple mode.
    fanex.customerSetting.directive('agentEditAgentPtSimpleMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.ptSimple;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(scope, currentPt, presetInput, maxInput, minInput, controller);
                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        }
    });

    fanex.customerSetting.directive('agentEditAgentPtSimpleModeReadOnly', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode-readonly.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.ptSimple;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(scope, currentPt, presetInput, maxInput, minInput, controller);
                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        }
    });

    fanex.customerSetting.directive('licenseeEditAgentOnePtSimpleMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/licensee-edit-agent-one-pt-simple-mode.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.ptSimple;

                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.positionTakingActions.licenseeEditAgentOneActions(scope, currentPt, maxInput, minInput, controller);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
                currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
                currentPt.MinRangeOfMin = 0;
                currentPt.MaxRangeOfMin = currentPt.Max;
            }
        }
    });

    fanex.customerSetting.directive('agentEditMemberPtSimpleMode', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'customerSetting.positionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-member-simple-mode.html',
            link: function (scope, element, attrs, controller) {
                //// Cache Elements.
                var valueInput = element.find('.value-input');
                var currentPt = scope.ptSimple;
                valueInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditMemberActions(scope, currentPt, valueInput, controller);

                //// Init value
                currentPt.OldValue = currentPt.Value;
                currentPt.MinRangeOfValue = currentPt.Upline.Min;
                currentPt.MaxRangeOfValue = currentPt.Upline.Max;
            }
        }
    });
}