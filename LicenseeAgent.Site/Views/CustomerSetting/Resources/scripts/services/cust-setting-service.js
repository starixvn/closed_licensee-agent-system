
'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.service('CustomerSettingService', ['$http', "$rootScope", function ($http, $rootScope) {
        this.updateSetting = function () {
            $http({
                method: 'POST',
                url: core.resolveUrl($('#custSetting').data('updatedUrl')),
                headers: { 'Content-Type': 'application/json' },
                data: {
                    settingContext: settingContext
                }
            }).
            success(function (data) {
                core.unblockUI();

                if (data.ErrCode !== 0) {
                    $('.messages').show();
                    $scope.Messages = [{
                        ErrCode: fanex.customerSetting.Error,
                        ErrMsg: data.ErrMsg

                    }];
                    return;
                }
                var commissionSettingScope,
                    bettingLimitScope,
                    positionTakingScope,
                    isEditIndirectMember;

                isEditIndirectMember = settingContext.FormId !== fanex.customerSetting.FormType.EditIndirectMember;

                if (isEditIndirectMember) {
                    commissionSettingScope = angular.element('[data-ng-controller="CommissionController"]').scope();
                    bettingLimitScope = angular.element('[data-ng-controller="BetLimitController"]').scope();

                    commissionSettingScope.updateSetting();
                    bettingLimitScope.updateSetting();
                }

                positionTakingScope = angular.element('[data-ng-controller="PositionTakingController"]').scope();
                positionTakingScope.updateSetting();


                if ($rootScope.hasOfferedNumberGame) {
                    fanex.customerSetting.numberGameSetting.updateSetting(isEditIndirectMember);
                }
            }).
            error(function (data) {
                core.unblockUI();
                alert(data.ErrMsg);
            });
        };
    } ]);
}