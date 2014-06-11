/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/fanex.core.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global virtualSportsSetting, settingContext */

if (fanex.customerSetting) {
    fanex.customerSetting.controller('PositionTakingController',
    [
        '$scope',
        'Fanex.CustomerSetting.LanguageService',
        function ($scope, languageService) {
            'use strict';
            fanex.customerSetting.positionTakingService.init(virtualSportsSetting.PositionTakingSetting, languageService);
            $scope.languageService = languageService;
            $scope.config = fanex.customerSetting.positionTakingService.config;
            $scope.settingContext = settingContext;
            $scope.positionTakingService = fanex.customerSetting.positionTakingService;
            $scope.ptModel = {
                ptGroups: fanex.cloneObject(virtualSportsSetting.PositionTakingSetting),
                ptFirstData: fanex.cloneObject(virtualSportsSetting.PositionTakingSetting[0].PositionTakings[0]),
                settingContext: settingContext
            };

            $scope.updateSettingModel = function () {
                angular.forEach($scope.ptModel.ptGroups, function (ptGroup) {
                    ptGroup.ApplyAll = false;
                });

                $scope.positionTakingService.updateScope($scope, fanex.cloneObject(virtualSportsSetting.PositionTakingSetting), languageService);
            };

            $scope.saveData = function () {
                return $scope.positionTakingService.getPositionTakingUpdate(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            };

            $scope.isValidData = function () {
                return $scope.positionTakingService.isValidData(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            };
        }
    ]);
}