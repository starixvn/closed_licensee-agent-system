'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.commissionSettingActions =
    {
        licenseeOrAgentEditAgentActions: function(modelData, inputControl, scope) {
            return {
                onChangeDiscountValue: function() {
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
        agentEditCustomerActions: function(commissionSettingsViewModel, scope) {
            return {
                onChangeDiscountValue: function(modelData, controlId) {
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
}