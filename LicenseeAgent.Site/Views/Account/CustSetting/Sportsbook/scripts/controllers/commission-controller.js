/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.sportsbookSetting) {
    var commissionController = fanex.sportsbookSetting.controller('fanex.custsetting.commissioncontroller',
    [
        '$scope',
        '$filter',
        function ($scope, $filter) {
            $scope.language = fanex.sportsbookSetting.language[settingContext.Language];

            $scope.config = fanex.sportsbookSetting.commissionSettingService.config;
            $scope.settingContext = settingContext;
            $scope.commissionSettingService = fanex.sportsbookSetting.commissionSettingService;
            commissionSetting.CommissionItems = fanex.sportsbookSetting.commissionSettingService.arrangeCommissionSettingModel(commissionSetting.CommissionItems);
            var commissionSettingViewModel = fanex.sportsbookSetting.commissionSettingService.getCommissionSettingViewModel(fanex.cloneObject(commissionSetting.CommissionItems), $scope.language, settingContext.TargetRoleId);

            $scope.viewModel = {
                commissionSettingsViewModel: commissionSettingViewModel,
                settingContext: settingContext
            };

            $scope.saveData = function (callBackFunction) {
                var temp =
                 $scope.commissionSettingService.getCommissionSettingModel(
                                commissionSetting.CommissionItems,
                                $scope.viewModel.commissionSettingsViewModel,
                                settingContext.TargetRoleId);
                return temp;
            }

            $scope.commissionSettingsChangeViewModel = function () {
                $scope.viewModel.commissionSettingsViewModel = $scope.commissionSettingService.changeViewModel($scope.viewModel.commissionSettingsViewModel);
            }

            $scope.isValidData = function () {
                return $scope.commissionSettingService.isValidData($scope.viewModel.commissionSettingsViewModel);
            };

            return {
                formatNumber: function (number) {
                    return $filter('number')(number, 2);
                },
                getScope: function () {
                    return $scope;
                }
            }
        } ]);
}