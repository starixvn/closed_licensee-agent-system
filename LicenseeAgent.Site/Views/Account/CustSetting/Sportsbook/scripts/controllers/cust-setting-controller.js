if (fanex.sportsbookSetting) {
    var custSettingController = fanex.sportsbookSetting.controller('sportsbook.custSettingController', ['$scope', '$http', function CustSettingController($scope, $http) {
        if (typeof result != 'undefined') {
            $('#custSetting').html(result.MsgContent);
            $('#custSetting').show();
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
            $scope.MsgId = 0;
            $scope.MsgContent = '';

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
                    if (data.CustIdReturn > 0) {
                        alert('Insert Success!');
                    }
                    else {
                        $scope.MsgId = data.MsgId;
                        $scope.MsgContent = data.MsgContent;
                    }
                }).
                error(function (data, status, headers, config) {
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
                    if (data.MsgId == 0) {
                        alert('Updated Successfully!');
                    }
                    else {
                        alert(data.MsgContent);
                    }
                }).
                error(function (data, status, headers, config) {
                });
        }

        if (typeof settingContext == 'undefined') {
            $('#custSetting').html("It has a problem about data.");
        }

        $('#custSetting').show();
    } ]);
}