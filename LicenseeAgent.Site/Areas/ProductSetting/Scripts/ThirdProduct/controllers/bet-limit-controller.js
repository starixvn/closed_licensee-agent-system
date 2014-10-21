/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global settingContext */

if (fanex.customerSetting) {
    fanex.customerSetting.controller('BetLimitController',
    [
        '$scope',
        'Fanex.CustomerSetting.LanguageService',
        'ThirdProductFactory',
        function ($scope, languageService, thirdProductFactory) {
            'use strict';
            $scope.languageService = languageService;
            $scope.config = fanex.customerSetting.betLimitService.config;
            $scope.settingContext = settingContext;
            $scope.betLimitService = fanex.customerSetting.betLimitService;

            $scope.model = {
                // For all sport types
                bettingLimitsModel: fanex.cloneObject(thirdProductFactory.productSetting.BettingLimitSetting),
                // For only soccer setting
                firstRecord: fanex.cloneObject(thirdProductFactory.productSetting.BettingLimitSetting[0]),
                settingContext: settingContext
            };

            $scope.updateSettingModel = function () {
                $scope.betLimitService.updateScope($scope, fanex.cloneObject(thirdProductFactory.productSetting.BettingLimitSetting));
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

            $scope.updatePartial = function () {
                var settings = {};

                if (!$scope.isValidData()) {
                    return;
                }

                settings[thirdProductFactory.productSettingName] = { BettingLimitSetting: $scope.saveData() };

                $scope.$parent.updatePartial(settings, fanex.isChangedData(settings[thirdProductFactory.productSettingName]));
            };
        }
    ]);
}