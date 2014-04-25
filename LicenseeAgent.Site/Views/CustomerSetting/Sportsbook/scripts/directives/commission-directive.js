/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global core */
'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.commissionSettingActions =
    {
        licenseeOrAgentEditAgentActions: function(modelData, controller, inputControl, scope) {
            return {
                onChangeDiscountValue: function() {
                    var endCharacter = modelData.discount.toString().charAt(modelData.discount.toString().length - 1);
                    if (endCharacter !== '.' && endCharacter !== '0') {
                        modelData.discount = fanex.roundNumber(modelData.discount, 2);
                    }
                    modelData.actions.isValid();
                },
                isValid: function() {
                    var isValid = modelData.discount >= modelData.minDiscount && modelData.discount <= modelData.maxDiscount;
                    if (!isValid) {
                        var message = scope.language["message_ValueIsNotInRange"] + ' [' + modelData.minDiscount + ' - ' + modelData.maxDiscount + ']';
                        fanex.customerSetting.commissionSettingService.showInvalidMessage(inputControl[0], message);
                    } else {
                        fanex.hideHint();
                    }

                    return isValid;
                }
            };
        },
        agentEditCustomerActions: function(commissionSettingsViewModel, controller, scope) {
            return {
                onChangeDiscountValue: function(modelData, controlId) {
                    var endCharacter = modelData.memberCommission.toString().charAt(modelData.memberCommission.toString().length - 1);
                    if (endCharacter !== '.' && endCharacter !== '0') {
                        modelData.memberCommission = fanex.roundNumber(modelData.memberCommission, 2);
                    }
                    commissionSettingsViewModel.actions.isValidLine(modelData, controlId);
                },
                isValidLine: function(modelData, controlId) {
                    var isValid = modelData.memberCommission >= 0 && modelData.memberCommission <= modelData.agentCommission;
                    if (!isValid) {
                        var inputControl = $('#' + controlId);
                        inputControl.preventSpecialCharacters("^[0-9.]+$");
                        var message = scope.language["message_ValueIsNotInRange"] + ' [0' + ' - ' + modelData.agentCommission + ']';
                        fanex.customerSetting.commissionSettingService.showInvalidMessage(inputControl[0], message);
                    } else {
                        fanex.hideHint();
                    }

                    return isValid;
                },
                isValid: function() {
                    if (!commissionSettingsViewModel.actions.isValidLine(commissionSettingsViewModel.line1, 'line1')) {
                        return false;
                    }
                    if (!commissionSettingsViewModel.actions.isValidLine(commissionSettingsViewModel.line2, 'line2')) {
                        return false;
                    }
                    if (!commissionSettingsViewModel.actions.isValidLine(commissionSettingsViewModel.line3, 'line3')) {
                        return false;
                    }

                    return true;
                }
            };
        }
    };

    fanex.customerSetting.directive('commissionSettingLicenseeOrAgentEditAgent', function() {
        return {
            restrict: 'E',
            replace: true,
            controller: 'CommissionController',
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/commission/licensee-or-agent-edit-agent.html'),
            link: function(scope, element, attrs, controller) {
                var commissionSetting = scope.commissionSetting;
                var inputControl = element.find('.input');
                inputControl.preventSpecialCharacters("^[0-9.]+$");
                commissionSetting.actions = fanex.customerSetting.commissionSettingActions.licenseeOrAgentEditAgentActions(commissionSetting,
                    controller,
                    inputControl,
                    scope);
            }
        };
    });

    fanex.customerSetting.directive('commissionSettingLicenseeOrAgentEditAgentReadOnly', function() {
        return {
            restrict: 'E',
            replace: true,
            controller: 'CommissionController',
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/commission/licensee-or-agent-edit-agent-readonly.html'),
            link: function(scope, element, attrs, controller) {
                var commissionSetting = scope.commissionSetting;
                var inputControl = element.find('.input');
                inputControl.preventSpecialCharacters("^[0-9.]+$");
                commissionSetting.actions = fanex.customerSetting.commissionSettingActions.licenseeOrAgentEditAgentActions(commissionSetting,
                    controller,
                    inputControl,
                    scope);
            }
        };
    });

    fanex.customerSetting.directive('commissionSettingAgentEditMember', function() {
        return {
            restrict: 'E',
            replace: true,
            controller: 'CommissionController',
            templateUrl: core.resolveUrl('~/Views/CustomerSetting/Sportsbook/scripts/templates/commission/agent-edit-customer.html'),
            link: function(scope, element, attrs, controller) {
                var commissionSettingsViewModel = scope.viewModel.commissionSettingsViewModel;
                commissionSettingsViewModel.actions = fanex.customerSetting.commissionSettingActions.agentEditCustomerActions(commissionSettingsViewModel,
                    controller,
                    scope);
            }
        };
    });
}