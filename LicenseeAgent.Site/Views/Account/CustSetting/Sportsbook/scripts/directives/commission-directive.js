/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />


if (fanex.sportsbookSetting) {

    fanex.sportsbookSetting.commissionSettingActions =
    {
        licenseeOrAgentEditAgentActions: function (modelData, controller, inputControl, scope) {
            return {
                onChangeDiscountValue: function () {
                    var endCharacter = modelData.discount.toString().charAt(modelData.discount.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        modelData.discount = fanex.roundNumber(modelData.discount, 2);
                    }
                    modelData.actions.isValid();
                },
                isValid: function () {
                    var isValid = modelData.discount >= modelData.minDiscount && modelData.discount <= modelData.maxDiscount;
                    if (!isValid) {
                        var message = scope.language["message_ValueIsNotInRange"] + ' [' + modelData.minDiscount + ' - ' + modelData.maxDiscount + ']';
                        fanex.sportsbookSetting.commissionSettingService.showInvalidMessage(inputControl[0], message);
                    }
                    else {
                        fanex.hideHint();
                    }

                    return isValid;
                }
            };
        },
        agentEditCustomerActions: function (commissionSettingsViewModel, controller, scope) {
            return {
                onChangeDiscountValue: function (modelData, controlId) {
                    var endCharacter = modelData.memberCommission.toString().charAt(modelData.memberCommission.toString().length - 1);
                    if (endCharacter != '.' && endCharacter != '0') {
                        modelData.memberCommission = fanex.roundNumber(modelData.memberCommission, 2);
                    }
                    commissionSettingsViewModel.actions.isValid(modelData, controlId);
                },
                isValid: function (modelData, controlId) {
                    var isValid = modelData.memberCommission >= 0 && modelData.memberCommission <= modelData.agentCommission;
                    if (!isValid) {
                        var inputControl = $('#' + controlId);
                        inputControl.preventSpecialCharacters("^[0-9.]+$");
                        var message = scope.language["message_ValueIsNotInRange"] + ' [0' + ' - ' + modelData.agentCommission + ']';
                        fanex.sportsbookSetting.commissionSettingService.showInvalidMessage(inputControl[0], message);
                    }
                    else {
                        fanex.hideHint();
                    }

                    return isValid;
                }
            };
        }
    };


    fanex.sportsbookSetting.directive('commissionSettingLicenseeOrAgentEditAgent', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'fanex.custsetting.commissioncontroller',
            templateUrl: '/Views/Account/CustSetting/Sportsbook/scripts/templates/commission/licensee-or-agent-edit-agent.html',
            link: function (scope, element, attrs, controller) {
                var commissionSetting = scope.commissionSetting;
                var inputControl = element.find('.input');
                inputControl.preventSpecialCharacters("^[0-9.]+$");
                commissionSetting.actions = fanex.sportsbookSetting.commissionSettingActions.licenseeOrAgentEditAgentActions(commissionSetting, controller, inputControl, scope);
            }
        }
    });

    fanex.sportsbookSetting.directive('commissionSettingAgentEditMember', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: 'fanex.custsetting.commissioncontroller',
            templateUrl: '/Views/Account/CustSetting/Sportsbook/scripts/templates/commission/agent-edit-customer.html',
            link: function (scope, element, attrs, controller) {
                var commissionSettingsViewModel = scope.viewModel.commissionSettingsViewModel;
                commissionSettingsViewModel.actions = fanex.sportsbookSetting.commissionSettingActions.agentEditCustomerActions(commissionSettingsViewModel, controller, scope);
            }
        }
    });
}