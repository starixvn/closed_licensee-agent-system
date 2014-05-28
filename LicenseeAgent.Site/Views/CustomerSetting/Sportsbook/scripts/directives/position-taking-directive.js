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
                templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedMode($timeout, scope, element, formatter, languageService);
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
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode-readonly.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedModeReadOnly($timeout, scope, element, formatter, languageService);
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
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/licensee-edit-agent-one-pt-advanced-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtAdvancedMode($timeout, scope, element, formatter, languageService);
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
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-member-advanced-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditMemberPtAdvancedMode($timeout, scope, element, formatter, languageService);
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
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleMode($timeout, scope, element, formatter, languageService);
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
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode-readonly.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleModeReadOnly($timeout, scope, element, formatter, languageService);
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
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/licensee-edit-agent-one-pt-simple-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtSimpleMode($timeout, scope, element, formatter, languageService);
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
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-member-simple-mode.html'),
                link: function (scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditMemberPtSimpleMode($timeout, scope, element, formatter, languageService);
                }
            };
        }
    ]);
}