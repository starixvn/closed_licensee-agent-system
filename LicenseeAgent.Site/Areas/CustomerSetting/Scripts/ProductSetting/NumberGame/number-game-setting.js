/*global settingContext */

fanex.customerSetting.numberGameSetting = {
    getNumberGameSetting: function (isDeposit) {
        'use strict';
        var commissions = null,
            bettingLimits = null,
            positionTakings = null,
            bettingLimitScope,
            positionTakingScope,
            commissionSettingScope;

        if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
            if (!isDeposit) {
                commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
                commissions = commissionSettingScope.getNumberGameSetting();
            }
            bettingLimitScope = angular.element('[data-ng-controller="NumberGameBetLimitController"]').scope();
            if (!bettingLimitScope.isValidData()) {
                return null;
            }
            bettingLimits = bettingLimitScope.saveData();
        }

        positionTakingScope = angular.element('[data-ng-controller="NumberGamePositionTakingController"]').scope();

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
        'use strict';
        var bettingLimitScope,
            positionTakingScope;

        if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
            bettingLimitScope = angular.element('[data-ng-controller="NumberGameBetLimitController"]').scope();
            if (bettingLimitScope !== undefined) {
                bettingLimitScope.updateSettingModel();
            }
        }

        positionTakingScope = angular.element('[data-ng-controller="NumberGamePositionTakingController"]').scope();
        if (positionTakingScope !== undefined) {
            positionTakingScope.updateSettingModel();
        }
    }
};