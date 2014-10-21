/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/fanex.core.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global settingContext, customerSetting */

if (fanex.customerSetting) {
    fanex.customerSetting.controller('PositionTakingController',
    [
        '$scope',
        'Fanex.CustomerSetting.LanguageService',
        'ThirdProductFactory',
        function ($scope, languageService, thirdProductFactory) {
            'use strict';
            fanex.customerSetting.positionTakingService.init(customerSetting.PositionTakingSetting, languageService);
            $scope.languageService = languageService;
            $scope.config = fanex.customerSetting.positionTakingService.config;
            $scope.settingContext = settingContext;
            $scope.positionTakingService = fanex.customerSetting.positionTakingService;
            $scope.thirdProductFactory = thirdProductFactory;
            $scope.ptModel = {
                ptGroups: fanex.cloneObject(customerSetting.PositionTakingSetting),
                ptFirstData: fanex.cloneObject(customerSetting.PositionTakingSetting[0].PositionTakings[0]),
                settingContext: settingContext
            };

            $scope.updateSettingModel = function () {
                angular.forEach($scope.ptModel.ptGroups, function (ptGroup) {
                    ptGroup.ApplyAll = false;
                });

                $scope.positionTakingService.updateScope($scope, fanex.cloneObject(customerSetting.PositionTakingSetting), languageService);
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

            $scope.updatePartial = function () {
                if (!$scope.isValidData()) {
                    return;
                }

                if (!$scope.isValidData()) {
                    return;
                }

                var settings = { customerSetting: { PositionTakingSetting: $scope.saveData() } };

                $scope.$parent.updatePartial(settings, fanex.isChangedData(settings.customerSetting));
            };
        }
    ]);
}