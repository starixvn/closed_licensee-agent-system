/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />

'use strict';

if (fanex.customerSetting) {

    //// Edit position taking advanced mode.
    fanex.customerSetting.directive('numberGameAgentEditAgentPtAdvancedMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedMode($timeout, scope, element, formatter);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditAgentPtAdvancedModeReadOnly', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode-readonly.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedModeReadOnly($timeout, scope, element, formatter);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameLicenseeEditAgentOnePtAdvancedMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/licensee-edit-agent-one-pt-advanced-mode.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtAdvancedMode($timeout, scope, element, formatter);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditMemberPtAdvancedMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-member-advanced-mode.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditMemberPtAdvancedMode($timeout, scope, element, formatter);
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
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleMode($timeout, scope, element, formatter);
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
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode-readonly.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleModeReadOnly($timeout, scope, element, formatter);
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
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/licensee-edit-agent-one-pt-simple-mode.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtSimpleMode($timeout, scope, element, formatter);
            }
        };
    } ]);

    fanex.customerSetting.directive('numberGameAgentEditMemberPtSimpleMode', ['$timeout', 'Formatter', function ($timeout, formatter) {
        return {
            restrict: 'E',
            replace: true,
            controller: 'NumberGamePositionTakingController',
            scope: {
                ptSimple: '='
            },
            templateUrl: core.resolveUrl('/Views/CustomerSetting/NumberGame/scripts/templates/positiontaking/agent-edit-member-simple-mode.html'),
            link: function (scope, element) {
                fanex.customerSetting.positionTakingDirective.agentEditMemberPtSimpleMode($timeout, scope, element, formatter);
            }
        };
    } ]);
}