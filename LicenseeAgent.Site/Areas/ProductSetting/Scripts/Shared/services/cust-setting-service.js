/*global CORE, settingContext */

if (fanex.customerSetting) {
    fanex.customerSetting.service('CustomerSettingService', ['$http', function ($http) {
        'use strict';
        this.updateSettingModel = function () {
            $http({
                method: 'POST',
                url: CORE.resolveUrl($('#custSetting').data('updated-href')),
                headers: { 'Content-Type': 'application/json' },
                data: {
                    ProductId: $('#custSetting').data('product'),
                    CustId: settingContext.CustId,
                    RoleId: settingContext.RoleId,
                    TargetCustId: settingContext.TargetCustId,
                    TargetRoleId: settingContext.TargetRoleId,
                    FormId: settingContext.FormId
                }
            }).
            success(function (data) {
                CORE.unblockUI();

                var customerSettingScope,
                    commissionSettingScope,
                    bettingLimitScope,
                    positionTakingScope;

                customerSettingScope = angular.element('[data-ng-controller="CustomerSettingController"]').scope();
                customerSettingScope.updateProductSetting(data);

                if (settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember) {
                    commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
                    bettingLimitScope = angular.element('[data-ng-controller="BetLimitController"]').scope();

                    if (commissionSettingScope) {
                        commissionSettingScope.updateSettingModel();
                    }

                    bettingLimitScope.updateSettingModel();
                }

                positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                positionTakingScope.updateSettingModel();

                if (customerSettingScope.hasOfferedNumberGame) {
                    fanex.customerSetting.numberGameSetting.updateSettingModel();
                }
            }).
            error(function (data) {
                CORE.unblockUI();
                alert(data.ErrMsg);
            });
        };
    } ]);
}