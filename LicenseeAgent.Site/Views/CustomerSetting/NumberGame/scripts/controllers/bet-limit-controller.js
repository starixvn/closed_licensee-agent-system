/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />

/*global settingContext, numberGameSetting */
'use strict';

if (fanex.customerSetting) {

    fanex.customerSetting.BetLimitController = {
        updateScope: function ($scope) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];

            $scope.config = fanex.customerSetting.numberGameBetLimitService.config;
            $scope.settingContext = settingContext;
            $scope.betLimitService = fanex.customerSetting.numberGameBetLimitService;

            $scope.model = {
                // For all sport types
                bettingLimitsModel: fanex.cloneObject(numberGameSetting.BettingLimitSetting),
                // For only soccer setting
                firstRecord: fanex.cloneObject(numberGameSetting.BettingLimitSetting[0]),
                settingContext: settingContext
            };

            $scope.saveData = function () {
                return $scope.betLimitService.getbettingLimits(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
            };

            $scope.isValidData = function () {
                return $scope.betLimitService.isValidData(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
            };
        }
    };

    fanex.customerSetting.controller('NumberGameBetLimitController',
    [
        '$scope',
        function ($scope) {
            fanex.customerSetting.BetLimitController.updateScope($scope);
        }
    ]);

    fanex.customerSetting.controller('NumberGameBetLimitAdvancedModeController',
    [
        '$scope',
        function ($scope) {
            fanex.customerSetting.BetLimitController.updateScope($scope);
        }
    ]);
}