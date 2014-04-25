/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global sportsBookSetting, numberGameSetting, settingContext */

'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.factory("CommissionModel", function () {
        return sportsBookSetting.CommissionSetting.CommissionItems.concat(numberGameSetting.CommissionSetting.CommissionItems);
    });

    fanex.customerSetting.controller('CommissionController',
    [
        '$scope',
        'CommissionModel',
        function($scope, CommissionModel) {

            $scope.language = fanex.customerSetting.language[settingContext.Language];
            $scope.config = fanex.customerSetting.commissionSettingService.config;
            $scope.settingContext = settingContext;
            $scope.commissionSettingService = fanex.customerSetting.commissionSettingService;

            CommissionModel = fanex.customerSetting.commissionSettingService.arrangeCommissionSettingModel(CommissionModel);
            var commissionSettingViewModel = fanex.customerSetting.commissionSettingService.getCommissionSettingViewModel(
                fanex.cloneObject(CommissionModel),
                $scope.language,
                settingContext.TargetRoleId);

            $scope.viewModel = {
                commissionSettingsViewModel: commissionSettingViewModel,
                settingContext: settingContext
            };

            $scope.saveData = function() {
                var temp =
                    $scope.commissionSettingService.getCommissionSettingModel(
                        CommissionModel,
                        $scope.viewModel.commissionSettingsViewModel,
                        settingContext.TargetRoleId);
                return temp;
            };

            $scope.commissionSettingsChangeViewModel = function() {
                $scope.viewModel.commissionSettingsViewModel = $scope.commissionSettingService.changeViewModel($scope.viewModel.commissionSettingsViewModel);
            };

            $scope.isValidData = function() {
                return $scope.commissionSettingService.isValidData($scope.viewModel.commissionSettingsViewModel);
            };
        }
    ]);
}