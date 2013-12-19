if (fanex.sportsbookSetting) {
    var custSettingController = fanex.sportsbookSetting.controller('sportsbook.custSettingController', ['$scope', '$http', function CustSettingController($scope, $http) {
        CORE.blockUI();
        if (typeof result != 'undefined') {
            $('#custSetting').html(result.MsgContent);
            $('#custSetting').show();
            CORE.unblockUI();
            return;
        }

        CORE.initSectionInfo();
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
                    if (data.MsgId == 0) {
                        if (angular.isDefined(data.Messages))
                            if (data.Messages.length == 1 && data.Messages[0].MsgId == 0) {
                                alert(data.Messages[0].MsgContent);
                                return;
                            }

                        $scope.Messages = data.Messages;
                        CORE.scrollTop();
                    }
                    else {
                        alert(data.MsgContent);
                    }
                }).
                error(function (data, status, headers, config) {
                    CORE.unblockUI();
                    alert(data.MsgContent);
                });
            }
        }

        $scope.updateCustomer = function () {
            $scope.MsgId = 0;
            $scope.MsgContent = '';

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
                if (data.MsgId == 0) {
                    $scope.Messages = data.Messages;
                    CORE.scrollTop();
                    POPUP.close();
                }
                else {
                    alert(data.MsgContent);
                }
            }).
            error(function (data, status, headers, config) {
                CORE.unblockUI();
                alert(data.MsgContent);
            });
        }

        if (typeof settingContext == 'undefined') {
            $('#custSetting').html("It has a problem about data.");
        }

        $('#custSetting').show();
        CORE.unblockUI();
    } ]);
}