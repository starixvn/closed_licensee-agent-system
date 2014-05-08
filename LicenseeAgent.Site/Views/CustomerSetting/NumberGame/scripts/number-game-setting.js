
'use strict';

fanex.customerSetting.numberGameSetting = {
    getNumberGameSetting: function (settingMode) {
        var commissions,
            bettingLimits,
            positionTakings,
            bettingLimitScope,
            positionTakingScope,
            commissionSettingScope;

        commissionSettingScope = angular.element('[ng-controller="CommissionController"]').scope();

        if (settingMode) {
            bettingLimitScope = angular.element('[ng-controller="NumberGameBetLimitController"]').scope();
            positionTakingScope = angular.element('[ng-controller="NumberGamePositionTakingController"]').scope();
        }
        else {
            bettingLimitScope = angular.element('[ng-controller="NumberGameBetLimitAdvancedModeController"]').scope();
            positionTakingScope = angular.element('[ng-controller="NumberGamePositionTakingAdvancedModeController"]').scope();
        }

        if (commissionSettingScope !== undefined)
        { 
            if(!commissionSettingScope.isValidData()) {
                return null;
            }
            commissions = commissionSettingScope.getNumberGameSetting();
        }

        if (bettingLimitScope !== undefined) {
            if (!bettingLimitScope.isValidData()) {
                return null;
            }
            bettingLimits = bettingLimitScope.saveData();
        }

        if (!positionTakingScope.isValidData()) {
            return null;
        }
        positionTakings = positionTakingScope.saveData();

        return {
            CommissionSetting: commissions,
            PositionTakingSetting: positionTakings,
            BettingLimitSetting: bettingLimits
        };
    }
};