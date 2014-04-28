
'use strict';

fanex.customerSetting.numberGameSetting = {
    getNumberGameSetting: function(isSimpleMode) {
        var commissions,
            bettingLimits,
            positionTakings,
            bettingLimitScope,
            positionTakingScope,
            commissionSettingScope;

        commissionSettingScope = angular.element('[ng-controller="CommissionController"]').scope();
        commissions = commissionSettingScope.getNumberGameSetting();

        if (isSimpleMode) {
            bettingLimitScope = angular.element('[ng-controller="NumberGameBetLimitController"]').scope();
            positionTakingScope = angular.element('[ng-controller="NumberGamePositionTakingController"]').scope();
        } else {
            bettingLimitScope = angular.element('[ng-controller="NumberGameBetLimitAdvancedModeController"]').scope();
            positionTakingScope = angular.element('[ng-controller="NumberGamePositionTakingAdvancedModeController"]').scope();
        }

        if (!bettingLimitScope.isValidData() || !positionTakingScope.isValidData()) {
            return null;
        }
        bettingLimits = bettingLimitScope.saveData();
        positionTakings = positionTakingScope.saveData();

        return {
            CommissionSetting: commissions,
            PositionTakingSetting: positionTakings,
            BettingLimits: bettingLimits
        };
    }
};