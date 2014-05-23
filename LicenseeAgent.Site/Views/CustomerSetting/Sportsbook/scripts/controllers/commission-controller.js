/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global settingContext */

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.controller('CommissionController',
    [
        '$scope',
        function ($scope) {

            $scope.language = fanex.customerSetting.language[settingContext.Language];
            $scope.config = fanex.customerSetting.commissionSettingService.config;
            $scope.settingContext = settingContext;
            $scope.commissionSettingService = fanex.customerSetting.commissionSettingService;
            $scope.numberGameCommissionService = fanex.customerSetting.numberGameCommissionService;

            var commissionModel = $scope.commissionSettingService.arrangeCommissionSettingModel(
                                                                            $scope.commissionSettingService.getCommissionModel($scope.hasOfferedNumberGame));
            var commissionSettingViewModel = fanex.customerSetting.commissionSettingService.getCommissionSettingViewModel(
                fanex.cloneObject(commissionModel),
                $scope.language,
                $scope.settingContext.TargetRoleId,
                $scope.hasOfferedNumberGame);

            $scope.viewModel = {
                commissionSettingsViewModel: commissionSettingViewModel,
                settingContext: settingContext
            };

            $scope.updateSettingModel = function () {
                $scope.commissionSettingService.updateScope($scope);
            };

            $scope.saveData = function () {
                var temp =
                    $scope.commissionSettingService.getCommissionSettingModel(
                        commissionModel,
                        $scope.viewModel.commissionSettingsViewModel,
                        settingContext.TargetRoleId);
                return temp;
            };

            $scope.getNumberGameSetting = function () {
                return $scope.numberGameCommissionService.getCommissionSetting(
                    commissionModel,
                    $scope.viewModel.commissionSettingsViewModel,
                    settingContext.TargetRoleId);
            };

            $scope.commissionSettingsChangeViewModel = function () {
                $scope.viewModel.commissionSettingsViewModel = $scope.commissionSettingService.changeViewModel($scope.viewModel.commissionSettingsViewModel);
            };

            $scope.isValidData = function () {
                return $scope.commissionSettingService.isValidData($scope.viewModel.commissionSettingsViewModel);
            };
        }
    ]);
}