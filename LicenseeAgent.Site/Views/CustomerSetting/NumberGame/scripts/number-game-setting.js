/*global settingContext */

'use strict';

fanex.customerSetting.numberGameSetting = {
    getNumberGameSetting: function (settingMode) {
        var commissions = null,
            bettingLimits = null,
            positionTakings = null,
            bettingLimitScope,
            positionTakingScope,
            commissionSettingScope;

        if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
            commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
            commissions = commissionSettingScope.getNumberGameSetting();

            bettingLimitScope = settingMode ? angular.element('[data-ng-controller="NumberGameBetLimitController"]').scope()
                                        : angular.element('[data-ng-controller="NumberGameBetLimitAdvancedModeController"]').scope();

            if (!bettingLimitScope.isValidData()) {
                return null;
            }
            bettingLimits = bettingLimitScope.saveData();
        }

        positionTakingScope = settingMode ? angular.element('[data-ng-controller="NumberGamePositionTakingController"]').scope()
                                           : angular.element('[data-ng-controller="NumberGamePositionTakingAdvancedModeController"]').scope();

        if (positionTakingScope !== undefined) {
            if (!positionTakingScope.isValidData()) {
                return null;
            }
            positionTakings = positionTakingScope.saveData();
        }

        return {
            CommissionSetting: commissions,
            PositionTakingSetting: positionTakings,
            BettingLimitSetting: bettingLimits
        };
    },
    updateSettingModel: function () {
        var bettingLimitSimpleModeScope,
            positionTakingSimpleModeScope,
            bettingLimitAdvancedModeScope,
            positionTakingAdvancedModeScope;

        if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
            bettingLimitSimpleModeScope = angular.element('[data-ng-controller="NumberGameBetLimitController"]').scope();
            bettingLimitAdvancedModeScope = angular.element('[data-ng-controller="NumberGameBetLimitAdvancedModeController"]').scope();
            if (bettingLimitSimpleModeScope !== undefined) {
                bettingLimitSimpleModeScope.updateSettingModel();
            }
            if (bettingLimitAdvancedModeScope !== undefined) {
                bettingLimitAdvancedModeScope.updateSettingModel();
            }
        }

        positionTakingSimpleModeScope = angular.element('[data-ng-controller="NumberGamePositionTakingController"]').scope();
        positionTakingAdvancedModeScope = angular.element('[data-ng-controller="NumberGamePositionTakingAdvancedModeController"]').scope();
        if (positionTakingSimpleModeScope !== undefined) {
            positionTakingSimpleModeScope.updateSettingModel();
        }
        if (positionTakingAdvancedModeScope !== undefined) {
            positionTakingAdvancedModeScope.updateSettingModel();
        }
    }
};