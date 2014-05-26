/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */
'use strict';

if (fanex.customerSetting) {
    //// Edit position taking advanced mode.
    
    fanex.customerSetting.directive('agentEditAgentPtAdvancedMode', [
        '$timeout',
        'Formatter',
        function ($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedMode($timeout, scope, element, formatter);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('agentEditAgentPtAdvancedModeReadOnly', [
        '$timeout',
        'Formatter', 
        function ($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl: 
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-advanced-mode-readonly.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtAdvancedModeReadOnly($timeout, scope, element, formatter);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('licenseeEditAgentOnePtAdvancedMode', [
        '$timeout',
        'Formatter', 
        function ($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl: 
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/licensee-edit-agent-one-pt-advanced-mode.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtAdvancedMode($timeout, scope, element, formatter);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('agentEditMemberPtAdvancedMode', [
        '$timeout',
        'Formatter', 
        function ($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                templateUrl: 
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-member-advanced-mode.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditMemberPtAdvancedMode($timeout, scope, element, formatter);
                }
            };
        }
    ]);

    //// Edit postion taking simple mode.
    fanex.customerSetting.directive('agentEditAgentPtSimpleMode', [
        '$timeout',
        'Formatter', 
        function ($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl: 
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleMode($timeout, scope, element, formatter);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('agentEditAgentPtSimpleModeReadOnly', [
        '$timeout',
        'Formatter', 
        function($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl: 
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-agent-pt-simple-mode-readonly.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditAgentPtSimpleModeReadOnly($timeout, scope, element, formatter);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('licenseeEditAgentOnePtSimpleMode', [
        '$timeout',
        'Formatter', 
        function ($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl: 
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/licensee-edit-agent-one-pt-simple-mode.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.licenseeEditAgentOnePtSimpleMode($timeout, scope, element, formatter);
                }
            };
        }
    ]);

    fanex.customerSetting.directive('agentEditMemberPtSimpleMode', [
        '$timeout',
        'Formatter', 
        function ($timeout, formatter) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PositionTakingController',
                scope: {
                    ptSimple: '='
                },
                templateUrl: 
                    core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/positiontaking/agent-edit-member-simple-mode.html'),
                link: function(scope, element) {
                    fanex.customerSetting.positionTakingDirective.agentEditMemberPtSimpleMode($timeout, scope, element, formatter);
                }
            };
        }
    ]);
}