/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

'use strict';

if (fanex.customerSetting) {
   
    //// Edit position taking advanced mode.
    fanex.customerSetting.directive('numberGameAgentEditAgentPtAdvancedMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingAdvancedModeController',
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode.html',
            link: function (scope, element) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.pt;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput);
                    formatter);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditAgentPtAdvancedModeReadOnly', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingAdvancedModeController',
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode-readonly.html',
            link: function (scope, element) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.pt;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput);
                    formatter);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameLicenseeEditAgentOnePtAdvancedMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingAdvancedModeController',
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/licensee-edit-agent-one-pt-advanced-mode.html',
            link: function (scope, element) {
                //// Cache Elements.
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.pt;
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.licenseeEditAgentOneActions(
                    scope,
                    $timeout,
                    currentPt,
                    maxInput,
                    minInput);
                    formatter);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
                currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
                currentPt.MinRangeOfMin = 0;
                currentPt.MaxRangeOfMin = currentPt.Max;
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditMemberPtAdvancedMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingAdvancedModeController',
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-member-advanced-mode.html',
            link: function (scope, element) {
                //// Cache Elements.
                var valueInput = element.find('.value-input');
                var currentPt = scope.pt;
                valueInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.agentEditMemberActions(
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
    } ]);

    //// Edit postion taking simple mode.
    fanex.customerSetting.directive('numberGameAgentEditAgentPtSimpleMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode.html',
            link: function(scope, element) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.ptSimple;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput);
                    formatter);
                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditAgentPtSimpleModeReadOnly', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode-readonly.html',
            link: function(scope, element) {
                //// Cache Elements.
                var presetInput = element.find('.preset-input');
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.ptSimple;

                presetInput.preventSpecialCharacters("^[0-9.]+$");
                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.agentEditAgentActions(
                    scope,
                    $timeout,
                    currentPt,
                    presetInput,
                    maxInput,
                    minInput);
                    formatter);
                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfPreset = 0;
                currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                currentPt.actions.whenPresetChange();
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameLicenseeEditAgentOnePtSimpleMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/licensee-edit-agent-one-pt-simple-mode.html',
            link: function(scope, element) {
                //// Cache Elements.
                var maxInput = element.find('.max-input');
                var minInput = element.find('.min-input');
                var currentPt = scope.ptSimple;

                maxInput.preventSpecialCharacters("^[0-9.]+$");
                minInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.licenseeEditAgentOneActions(
                    scope,
                    $timeout,
                    currentPt,
                    maxInput,
                    minInput);
                    formatter);

                //// Init value
                currentPt.OldMin = currentPt.Min;
                currentPt.OldMax = currentPt.Max;
                currentPt.OldPreset = currentPt.Preset;
                currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
                currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
                currentPt.MinRangeOfMin = 0;
                currentPt.MaxRangeOfMin = currentPt.Max;
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditMemberPtSimpleMode', ['$timeout','Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: '/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-member-simple-mode.html',
            link: function(scope, element) {
                //// Cache Elements.
                var valueInput = element.find('.value-input');
                var currentPt = scope.ptSimple;
                valueInput.preventSpecialCharacters("^[0-9.]+$");

                //// Position taking Actions
                currentPt.actions = fanex.customerSetting.numberGamePositionTakingActions.agentEditMemberActions(
                    scope,
                    $timeout,
                    currentPt,
                    valueInput);
                    formatter);

                //// Init value
                currentPt.OldValue = currentPt.Value;
                currentPt.MinRangeOfValue = currentPt.Upline.Min;
                currentPt.MaxRangeOfValue = currentPt.Upline.Max;
            }
        };
    } ]);
}