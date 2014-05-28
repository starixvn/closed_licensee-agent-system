/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */

if (fanex.customerSetting) {
    //// Edit position taking advanced mode.

    fanex.customerSetting.directive('agentEditAgentPtAdvancedMode', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl: core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode.html'),
                link: function (scope, element) {
                    //// Cache Elements.
                    var presetInput = element.find('.preset-input');
                    var maxInput = element.find('.max-input');
                    var minInput = element.find('.min-input');
                    var currentPt = scope.pt.PositionTakings[0];

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
                        formatter,
                        languageService);

                    //// Init value
                    currentPt.OldMin = currentPt.Min;
                    currentPt.OldMax = currentPt.Max;
                    currentPt.OldPreset = currentPt.Preset;
                    currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
                    currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
                    currentPt.MinRangeOfMin = 0;
                    currentPt.MaxRangeOfMin = currentPt.Max;
                    currentPt.MinRangeOfPreset = 0;
                    currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                    currentPt.actions.whenPresetChange();
                }
            };
        }
    ]);

    fanex.customerSetting.directive('agentEditAgentPtAdvancedModeReadOnly', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl:
                    core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode-readonly.html'),
                link: function (scope, element) {
                    //// Cache Elements.
                    var presetInput = element.find('.preset-input');
                    var maxInput = element.find('.max-input');
                    var minInput = element.find('.min-input');
                    var currentPt = scope.pt.PositionTakings[0];

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
                        formatter,
                        languageService);

                    //// Init value
                    currentPt.OldMin = currentPt.Min;
                    currentPt.OldMax = currentPt.Max;
                    currentPt.OldPreset = currentPt.Preset;
                    currentPt.MinRangeOfMax = Math.max(0, currentPt.AllDownLine.Max);
                    currentPt.MaxRangeOfMax = fanex.roundNumber(currentPt.Upline.Max, 2);
                    currentPt.MinRangeOfMin = 0;
                    currentPt.MaxRangeOfMin = currentPt.Max;
                    currentPt.MinRangeOfPreset = 0;
                    currentPt.MaxRangeOfPreset = fanex.roundNumber(currentPt.Upline.Max - currentPt.AllDownLine.Max, 2);
                    currentPt.actions.whenPresetChange();
                }
            };
        }
    ]);

    fanex.customerSetting.directive('licenseeEditAgentOnePtAdvancedMode', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl:
                    core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/licensee-edit-agent-one-pt-advanced-mode.html'),
                link: function (scope, element) {
                    //// Cache Elements.
                    var maxInput = element.find('.max-input');
                    var minInput = element.find('.min-input');
                    var currentPt = scope.pt.PositionTakings[0];
                    maxInput.preventSpecialCharacters("^[0-9.]+$");
                    minInput.preventSpecialCharacters("^[0-9.]+$");

                    //// Position taking Actions
                    currentPt.actions = fanex.customerSetting.positionTakingActions.licenseeEditAgentOneActions(
                        scope,
                        $timeout,
                        currentPt,
                        maxInput,
                        minInput,
                        formatter,
                        languageService);

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
        }
    ]);

    fanex.customerSetting.directive('agentEditMemberPtAdvancedMode', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl:
                    core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/agent-edit-member-advanced-mode.html'),
                link: function (scope, element) {
                    //// Cache Elements.
                    var valueInput = element.find('.value-input');
                    var currentPt = scope.pt.PositionTakings[0];
                    valueInput.preventSpecialCharacters("^[0-9.]+$");

                    //// Position taking Actions
                    currentPt.actions = fanex.customerSetting.positionTakingActions.agentEditMemberActions(
                    scope,
                    $timeout,
                    currentPt,
                    valueInput,
                    formatter,
                    languageService);

                    //// Init value
                    currentPt.OldValue = currentPt.Value;
                    currentPt.MinRangeOfValue = currentPt.Upline.Min;
                    currentPt.MaxRangeOfValue = currentPt.Upline.Max;
                }
            };
        }
    ]);

    //// Edit postion taking simple mode.
    fanex.customerSetting.directive('agentEditAgentPtSimpleMode', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl:
                    core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleMode(
                        $timeout,
                        scope,
                        element,
                        formatter,
                        languageService);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('agentEditAgentPtSimpleModeReadOnly', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl:
                    core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode-readonly.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleModeReadOnly(
                        $timeout,
                        scope,
                        element,
                        formatter,
                        languageService);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('licenseeEditAgentOnePtSimpleMode', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl:
                    core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/licensee-edit-agent-one-pt-simple-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtSimpleMode(
                        $timeout,
                        scope,
                        element,
                        formatter,
                        languageService);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('agentEditMemberPtSimpleMode', [
        '$timeout',
        'Formatter',
        'Fanex.CustomerSetting.LanguageService',
        function ($timeout, formatter, languageService) {
            'use strict';
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl:
                    core.resolveUrl('~/Views/CustomerSetting/VirtualSports/scripts/templates/positiontaking/agent-edit-member-simple-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditMemberPtSimpleMode(
                        $timeout,
                        scope,
                        element,
                        formatter,
                        languageService);
                }
            };
        }
    ]);
}