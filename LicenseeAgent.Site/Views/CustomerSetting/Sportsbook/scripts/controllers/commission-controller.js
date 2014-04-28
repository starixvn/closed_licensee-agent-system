/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global sportsBookSetting, numberGameSetting, settingContext */

'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.factory("CommissionModel", function ($window) {
        if ($window['numberGameSetting'] !== undefined) {
            return sportsBookSetting.CommissionSetting.CommissionItems.concat(numberGameSetting.CommissionSetting.CommissionItems);
        }
        return sportsBookSetting.CommissionSetting.CommissionItems;
    });

    fanex.customerSetting.controller('CommissionController',
    [
        '$scope',
        'CommissionModel',
        function($scope, commissionModel) {

            $scope.language = fanex.customerSetting.language[settingContext.Language];
            $scope.config = fanex.customerSetting.commissionSettingService.config;
            $scope.settingContext = settingContext;
            $scope.commissionSettingService = fanex.customerSetting.commissionSettingService;
            $scope.numberGameCommissionService = fanex.customerSetting.numberGameCommissionService;

            commissionModel = fanex.customerSetting.commissionSettingService.arrangeCommissionSettingModel(commissionModel);
            var commissionSettingViewModel = fanex.customerSetting.commissionSettingService.getCommissionSettingViewModel(
                fanex.cloneObject(commissionModel),
                $scope.language,
                settingContext.TargetRoleId,
                $scope.$parent.haveNumberGame);

            $scope.viewModel = {
                commissionSettingsViewModel: commissionSettingViewModel,
                settingContext: settingContext
            };

            $scope.saveData = function() {
                var temp =
                    $scope.commissionSettingService.getCommissionSettingModel(
                        commissionModel,
                        $scope.viewModel.commissionSettingsViewModel,
                        settingContext.TargetRoleId);
                return temp;
            };

            $scope.getNumberGameSetting = function() {
                return $scope.numberGameCommissionService.getCommissionSetting(
                    commissionModel,
                    $scope.viewModel.commissionSettingsViewModel,
                    settingContext.TargetRoleId);
            }

            $scope.commissionSettingsChangeViewModel = function() {
                $scope.viewModel.commissionSettingsViewModel = $scope.commissionSettingService.changeViewModel($scope.viewModel.commissionSettingsViewModel);
            };

            $scope.isValidData = function() {
                return $scope.commissionSettingService.isValidData($scope.viewModel.commissionSettingsViewModel);
            };
        }
    ]);
}