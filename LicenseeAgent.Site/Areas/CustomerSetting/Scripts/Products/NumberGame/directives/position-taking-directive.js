/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />

/* global core */
if (fanex.customerSetting) {
    //// Edit position taking advanced mode.
    fanex.customerSetting.directive('numberGameAgentEditAgentPtAdvancedMode',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
    function ($timeout, formatter, languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/agent-edit-agent-pt-advanced-mode.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedMode($timeout, scope, element, formatter, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditAgentPtAdvancedModeReadOnly',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
    function ($timeout, formatter, languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/agent-edit-agent-pt-advanced-mode-readonly.html'), // jshint ignore:line
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedModeReadOnly($timeout, scope, element, formatter, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameLicenseeEditAgentOnePtAdvancedMode',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
    function ($timeout, formatter, languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/licensee-edit-agent-one-pt-advanced-mode.html'), // jshint ignore:line
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtAdvancedMode($timeout, scope, element, formatter, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditMemberPtAdvancedMode',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
     function ($timeout, formatter, languageService) {
         'use strict';
         return {
             restrict: 'E',
             replace: true,
             controller: 'NumberGamePositionTakingController',
             templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/agent-edit-member-advanced-mode.html'),
             link: function (scope, element) {
                 fanex.customerSetting.positionTakingDirective.agentEditMemberPtAdvancedMode($timeout, scope, element, formatter, languageService);
             }
         };
     } ]);

    //// Edit postion taking simple mode.
    fanex.customerSetting.directive('numberGameAgentEditAgentPtSimpleMode',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
    function ($timeout, formatter, languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/agent-edit-agent-pt-simple-mode.html'), // jshint ignore:line
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleMode($timeout, scope, element, formatter, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditAgentPtSimpleModeReadOnly',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
    function ($timeout, formatter, languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/agent-edit-agent-pt-simple-mode-readonly.html'), // jshint ignore:line
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleModeReadOnly($timeout, scope, element, formatter, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameLicenseeEditAgentOnePtSimpleMode',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
    function ($timeout, formatter, languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/licensee-edit-agent-one-pt-simple-mode.html'), // jshint ignore:line
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtSimpleMode($timeout, scope, element, formatter, languageService);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditMemberPtSimpleMode',
    [
    '$timeout',
    'Formatter',
    'Fanex.CustomerSetting.LanguageService',
    function ($timeout, formatter, languageService) {
        'use strict';
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: core.resolveUrl('~/Areas/CustomerSetting/Scripts/Products/NumberGame/templates/positiontaking/agent-edit-member-simple-mode.html'), // jshint ignore:line
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditMemberPtSimpleMode($timeout, scope, element, formatter, languageService);
            }
        };
    } ]);
}