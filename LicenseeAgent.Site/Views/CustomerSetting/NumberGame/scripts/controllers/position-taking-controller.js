/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/fanex.core.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global numberGameSetting , settingContext */

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.PositionTakingController = {
        updateScope: function ($scope) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];
            fanex.customerSetting.numberGamePositionTakingService.init(numberGameSetting.PositionTakingSetting, $scope.language);
            $scope.config = fanex.customerSetting.numberGamePositionTakingService.config;
            $scope.settingContext = settingContext;
            $scope.numberGamePositionTakingService = fanex.customerSetting.numberGamePositionTakingService;
            $scope.ptModel = {
                ptGroups: fanex.cloneObject(numberGameSetting.PositionTakingSetting),
                ptFirstData: fanex.cloneObject(numberGameSetting.PositionTakingSetting[0].PositionTakings[0]),
                settingContext: settingContext
            };

            $scope.saveData = function () {
                return $scope.numberGamePositionTakingService.getPositionTakingUpdate(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            };

            $scope.isValidData = function () {
                return $scope.numberGamePositionTakingService.isValidData(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            };
        }
    };

    fanex.customerSetting.controller('NumberGamePositionTakingController',
    [
        '$scope',
        function ($scope) {
            fanex.customerSetting.PositionTakingController.updateScope($scope);
        }
    ]);

    fanex.customerSetting.controller('NumberGamePositionTakingAdvancedModeController',
    [
        '$scope',
        function ($scope) {
            fanex.customerSetting.PositionTakingController.updateScope($scope);
        }
    ]);
}