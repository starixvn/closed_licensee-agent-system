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
            currentPt.OldMin = currentPt.Min;
            currentPt.OldMax = currentPt.Max;
            currentPt.OldPreset = currentPt.Preset;
            currentPt.MinRangeOfPreset = 0;
            currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
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
            currentPt.OldMin = currentPt.Min;
            currentPt.OldMax = currentPt.Max;
            currentPt.OldPreset = currentPt.Preset;
            currentPt.MinRangeOfPreset = 0;
            currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
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
            currentPt.OldMin = currentPt.Min;
            currentPt.OldMax = currentPt.Max;
            currentPt.OldPreset = currentPt.Preset;
            currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
            currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
            currentPt.MinRangeOfMin = 0;
            currentPt.MaxRangeOfMin = currentPt.Max;
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
            currentPt.OldValue = currentPt.Value;
            currentPt.MinRangeOfValue = currentPt.Upline.Min;
            currentPt.MaxRangeOfValue = currentPt.Upline.Max;
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
            currentPt.OldMin = currentPt.Min;
            currentPt.OldMax = currentPt.Max;
            currentPt.OldPreset = currentPt.Preset;
            currentPt.MinRangeOfPreset = 0;
            currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
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
            currentPt.OldMin = currentPt.Min;
            currentPt.OldMax = currentPt.Max;
            currentPt.OldPreset = currentPt.Preset;
            currentPt.MinRangeOfPreset = 0;
            currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
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
            currentPt.OldMin = currentPt.Min;
            currentPt.OldMax = currentPt.Max;
            currentPt.OldPreset = currentPt.Preset;
            currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
            currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
            currentPt.MinRangeOfMin = 0;
            currentPt.MaxRangeOfMin = currentPt.Max;
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
            currentPt.OldValue = currentPt.Value;
            currentPt.MinRangeOfValue = currentPt.Upline.Min;
            currentPt.MaxRangeOfValue = currentPt.Upline.Max;
        }
    };
}