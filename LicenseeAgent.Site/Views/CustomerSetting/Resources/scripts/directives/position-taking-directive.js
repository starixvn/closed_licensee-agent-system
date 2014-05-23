'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.positionTakingDirective = {
        agentEditAgentPtAdvancedMode: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var presetInput = element.find('.preset-input');
            var maxInput = element.find('.max-input');
            var minInput = element.find('.min-input');
            var currentPt = scope.pt;

            presetInput.preventSpecialCharacters("^[0-9.]+$");
            maxInput.preventSpecialCharacters("^[0-9.]+$");
            minInput.preventSpecialCharacters("^[0-9.]+$");

            //// Position taking Actions
            currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput,
                    formatter);

            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
            currentPt.actions.whenPresetChange();
        },
        agentEditAgentPtAdvancedModeReadOnly: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var presetInput = element.find('.preset-input');
            var maxInput = element.find('.max-input');
            var minInput = element.find('.min-input');
            var currentPt = scope.pt;

            presetInput.preventSpecialCharacters("^[0-9.]+$");
            maxInput.preventSpecialCharacters("^[0-9.]+$");
            minInput.preventSpecialCharacters("^[0-9.]+$");

            //// Position taking Actions
            currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput,
                    formatter);

            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
            currentPt.actions.whenPresetChange();
        },
        licenseeEditAgentOnePtAdvancedMode: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var maxInput = element.find('.max-input');
            var minInput = element.find('.min-input');
            var currentPt = scope.pt;
            maxInput.preventSpecialCharacters("^[0-9.]+$");
            minInput.preventSpecialCharacters("^[0-9.]+$");

            //// Position taking Actions
            currentPt.actions = fanex.customerSetting.positionTakingActions.licenseeEditAgentOneActions(
                    scope,
                    $timeout,
                    currentPt,
                    maxInput,
                    minInput,
                    formatter);

            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
        },
        agentEditMemberPtAdvancedMode: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var valueInput = element.find('.value-input');
            var currentPt = scope.pt;
            valueInput.preventSpecialCharacters("^[0-9.]+$");

            //// Position taking Actions
            currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditMemberActions(
                    scope,
                    $timeout,
                    currentPt,
                    valueInput,
                    formatter);

            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
        },
        agentEditAgentPtSimpleMode: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var presetInput = element.find('.preset-input');
            var maxInput = element.find('.max-input');
            var minInput = element.find('.min-input');
            var currentPt = scope.ptSimple;

            presetInput.preventSpecialCharacters("^[0-9.]+$");
            maxInput.preventSpecialCharacters("^[0-9.]+$");
            minInput.preventSpecialCharacters("^[0-9.]+$");

            currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput,
                    formatter);
            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
            currentPt.actions.whenPresetChange();
        },
        agentEditAgentPtSimpleModeReadOnly: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var presetInput = element.find('.preset-input');
            var maxInput = element.find('.max-input');
            var minInput = element.find('.min-input');
            var currentPt = scope.ptSimple;

            presetInput.preventSpecialCharacters("^[0-9.]+$");
            maxInput.preventSpecialCharacters("^[0-9.]+$");
            minInput.preventSpecialCharacters("^[0-9.]+$");

            currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput,
                    formatter);
            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
            currentPt.actions.whenPresetChange();
        },
        licenseeEditAgentOnePtSimpleMode: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var maxInput = element.find('.max-input');
            var minInput = element.find('.min-input');
            var currentPt = scope.ptSimple;

            maxInput.preventSpecialCharacters("^[0-9.]+$");
            minInput.preventSpecialCharacters("^[0-9.]+$");

            //// Position taking Actions
            currentPt.actions = fanex.customerSetting.positionTakingActions.licenseeEditAgentOneActions(
                    scope,
                    $timeout,
                    currentPt,
                    maxInput,
                    minInput,
                    formatter);

            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
        },
        agentEditMemberPtSimpleMode: function ($timeout, scope, element, formatter) {
            //// Cache Elements.
            var valueInput = element.find('.value-input');
            var currentPt = scope.ptSimple;
            valueInput.preventSpecialCharacters("^[0-9.]+$");

            //// Position taking Actions
            currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditMemberActions(
                    scope,
                    $timeout,
                    currentPt,
                    valueInput,
                    formatter);

            //// Init value
            fanex.customerSetting.positionTakingService.changeSettingValidation(currentPt);
        }
    };
}