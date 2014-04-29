/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/fanex.core.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global sportsBookSetting, settingContext */

'use strict';

if (fanex.customerSetting) {
    fanex.customerSetting.controller('PositionTakingController',
    [
        '$scope',
        function ($scope) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];
            fanex.customerSetting.positionTakingService.init(sportsBookSetting.PositionTakingSetting, $scope.language);
            $scope.config = fanex.customerSetting.positionTakingService.config;
            $scope.settingContext = settingContext;
            $scope.positionTakingService = fanex.customerSetting.positionTakingService;
            $scope.ptModel = {
                ptGroups: fanex.cloneObject(sportsBookSetting.PositionTakingSetting),
                ptFirstData: fanex.cloneObject(sportsBookSetting.PositionTakingSetting[0].PositionTakings[0]),
                settingContext: settingContext
            };

            $scope.saveData = function() {
                return $scope.positionTakingService.getPositionTakingUpdate(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            };

            $scope.isValidData = function() {
                return $scope.positionTakingService.isValidData(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            };
        }
    ]);
}