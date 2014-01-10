if (fanex.sportsbookSetting) {
    var custSettingController = fanex.sportsbookSetting.controller('sportsbook.custSettingController', ['$scope', '$http', function CustSettingController($scope, $http) {
        CORE.blockUI();
        if (typeof result != 'undefined') {
            $('#custSetting').html(result.ErrMsg);
            $('#custSetting').show();
            CORE.unblockUI();
            return;
        }

        CORE.initInstruction();
        // init tooltip
        $(document).tooltip();

        $('#modeSwitcher').on('switch-change', function (e, data) {
            var isSimpleMode = !data.value;
            fanex.hideHint();
            $scope.settingMode = isSimpleMode;
            var positionTakingScope = angular.element('[ng-controller="sportsbook.positionTakingController"]').scope();
            positionTakingScope.settingContext.IsSimpleMode = isSimpleMode;
            $scope.$apply();
        });

        $scope.settingMode = settingContext.IsSimpleMode;
        $scope.formId = settingContext.FormId;
        $scope.insertCustomer = function () {
            var custInfoScope = angular.element('#custInfoController').scope();
            if (custInfoScope.formCustInfo.$invalid) {
                var keepGoing = true;
                angular.forEach(custInfoScope.formCustInfo.$error, function (field) {
                    if (angular.isArray(field)) {
                        if (keepGoing) {
                            var element = $('#' + field[0].$name);
                            element.focus();
                            keepGoing = false;
                        }
                    }
                });
            }
            else {
                // Commission Setting
                var commissionSettingScope = angular.element('[ng-controller="fanex.custsetting.commissioncontroller"]').scope();
                if (!commissionSettingScope.isValidData()) {
                    return;
                }
                var commissionSettingsModel = commissionSettingScope.saveData();

                // Betting Limit
                var bettingLimitScope = angular.element('[ng-controller="fanex.custsetting.betlimitcontroller"]').scope();
                if (!bettingLimitScope.isValidData()) {
                    return;
                }
                var bettingLimits = bettingLimitScope.saveData();

                // position taking
                var positionTakingScope = angular.element('[ng-controller="sportsbook.positionTakingController"]').scope();
                if (!positionTakingScope.isValidData()) {
                    return;
                }
                var positionTakingUpdated = positionTakingScope.saveData();

                $scope.Messages = null;
                CORE.blockUI();
                $http({
                    method: 'POST',
                    url: CORE.resolveUrl($('#custSetting').data('href')),
                    headers: { 'Content-Type': 'application/json' },
                    data: {
                        settingContext: settingContext,
                        updateDataSportsbookCustomer:
                        {
                            PositionTakingSetting: positionTakingUpdated,
                            CustInfo: custInfoScope.custInfo,
                            CommissionSetting: {
                                CommissionItems: commissionSettingsModel,
                                UserGroup: bettingLimitScope.settingContext.TargetRoleId == 1 ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup : ''
                            },
                            BettingLimits: bettingLimits
                        }
                    }
                }).
                success(function (data, status, headers, config) {
                    CORE.unblockUI();
                    if (data.ErrCode == 0) {
                        $scope.Messages = data.Messages;
                        CORE.scrollTop();
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                }).
                error(function (data, status, headers, config) {
                    CORE.unblockUI();
                    alert(data.ErrMsg);
                });
            }
        }

        $scope.updateCustomer = function () {
            $scope.ErrCode = 0;
            $scope.ErrMsg = '';

            var custInfoScope = angular.element('[ng-controller="sportsbook.custInfoController"]').scope();

            // Commission Setting
            var commissionSettingScope = angular.element('[ng-controller="fanex.custsetting.commissioncontroller"]').scope();
            if (!commissionSettingScope.isValidData()) {
                return;
            }
            var commissionSettingsModel = commissionSettingScope.saveData();

            // Betting Limit
            var bettingLimitScope = angular.element('[ng-controller="fanex.custsetting.betlimitcontroller"]').scope();
            if (!bettingLimitScope.isValidData()) {
                return;
            }
            var bettingLimits = bettingLimitScope.saveData();

            // position taking
            var positionTakingScope = angular.element('[ng-controller="sportsbook.positionTakingController"]').scope();
            if (!positionTakingScope.isValidData()) {
                return;
            }
            var positionTakingUpdated = positionTakingScope.saveData();

            $scope.Messages = null;
            CORE.blockUI();
            $http({
                method: 'POST',
                url: CORE.resolveUrl($('#custSetting').data('href')),
                headers: { 'Content-Type': 'application/json' },
                data: {
                    settingContext: settingContext,
                    updateDataSportsbookCustomer:
                        {
                            PositionTakingSetting: positionTakingUpdated,
                            CustInfo: custInfoScope.custInfo,
                            CommissionSetting: {
                                CommissionItems: commissionSettingsModel,
                                UserGroup: bettingLimitScope.settingContext.TargetRoleId == 1 ? commissionSettingScope.viewModel.commissionSettingsViewModel.selectedGroup : ''
                            },
                            BettingLimits: bettingLimits
                        }
                }
            }).
            success(function (data, status, headers, config) {
                CORE.unblockUI();
                if (data.ErrCode == 0) {
                    $scope.Messages = data.Messages;
                    CORE.scrollTop();
                    var isSuccess = true;
                    for (var i = 0; i < data.Messages.length; i++) {
                        var message = data.Messages[i];
                        if (message.ErrCode != 0) {
                            isSuccess = false;
                            break;
                        }
                    }
                    if (isSuccess) {
                        POPUP.close();
                    }
                }
                else {
                    alert(data.ErrMsg);
                }
            }).
            error(function (data, status, headers, config) {
                CORE.unblockUI();
                alert(data.ErrMsg);
            });
        }

        $scope.updateIndirectCustomer = function () {
            $scope.ErrCode = 0;
            $scope.ErrMsg = '';

            var custInfoScope = angular.element('[ng-controller="sportsbook.custInfoController"]').scope();

            // position taking
            var positionTakingScope = angular.element('[ng-controller="sportsbook.positionTakingController"]').scope();
            if (!positionTakingScope.isValidData()) {
                return;
            }
            var positionTakingUpdated = positionTakingScope.saveData();

            $scope.Messages = null;
            CORE.blockUI();
            $http({
                method: 'POST',
                url: CORE.resolveUrl($('#custSetting').data('href')),
                headers: { 'Content-Type': 'application/json' },
                data: {
                    settingContext: settingContext,
                    updateDataSportsbookCustomer:
                        {
                            PositionTakingSetting: positionTakingUpdated,
                            CustInfo: custInfoScope.custInfo
                        }
                }
            }).
            success(function (data, status, headers, config) {
                CORE.unblockUI();
                if (data.ErrCode == 0) {
                    $scope.Messages = data.Messages;
                    CORE.scrollTop();
                    var isSuccess = true;
                    for (var i = 0; i < data.Messages.length; i++) {
                        var message = data.Messages[i];
                        if (message.ErrCode != 0) {
                            isSuccess = false;
                            break;
                        }
                    }
                    if (isSuccess) {
                        POPUP.close();
                    }
                }
                else {
                    alert(data.ErrMsg);
                }
            }).
            error(function (data, status, headers, config) {
                CORE.unblockUI();
                alert(data.ErrMsg);
            });
        }

        if (typeof settingContext == 'undefined') {
            $('#custSetting').html("It has a problem about data.");
        }

        $('#custSetting').show();
        CORE.unblockUI();
    } ]);
}