/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global settingContext, numberGameSetting */
'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.controller('NumberGameBetLimitController',
    [
        '$scope',
        function($scope) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];

            $scope.config = fanex.customerSetting.betLimitService.config;
            $scope.settingContext = settingContext;
            $scope.betLimitService = fanex.customerSetting.betLimitService;

            $scope.model = {
                // For all sport types
                bettingLimitsModel: fanex.cloneObject(numberGameSetting.BettingLimits),
                // For only soccer setting
                firstRecord: fanex.cloneObject(numberGameSetting.BettingLimits[0]),
                settingContext: settingContext
            };

            $scope.saveData = function() {
                return $scope.betLimitService.getbettingLimits(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
            };

            $scope.isValidData = function() {
                return $scope.betLimitService.isValidData(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
            };
        }
    ]);

    fanex.customerSetting.controller('NumberGameBetLimitAdvancedModeController',
    [
        '$scope',
        function($scope) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];

            $scope.config = fanex.customerSetting.betLimitService.config;
            $scope.settingContext = settingContext;
            $scope.betLimitService = fanex.customerSetting.betLimitService;
            $scope.model = {
                // For all sport types
                bettingLimitsModel: fanex.cloneObject(numberGameSetting.BettingLimits),
                // For only soccer setting
                firstRecord: fanex.cloneObject(numberGameSetting.BettingLimits[0]),
                settingContext: settingContext
            };

            $scope.saveData = function() {
                return $scope.betLimitService.getbettingLimits(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
            };

            $scope.isValidData = function() {
                return $scope.betLimitService.isValidData(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
            };
        }
    ]);
}