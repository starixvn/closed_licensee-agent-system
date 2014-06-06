/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/fanex.core.js" />

/*global numberGameSetting , settingContext */

if (fanex.customerSetting) {
    fanex.customerSetting.PositionTakingController = {
        updateScope: function ($scope, languageService) {
            'use strict';
            fanex.customerSetting.numberGamePositionTakingService.init(numberGameSetting.PositionTakingSetting, languageService);
            $scope.config = fanex.customerSetting.numberGamePositionTakingService.config;
            $scope.settingContext = settingContext;
            $scope.positionTakingService = fanex.customerSetting.numberGamePositionTakingService;

            $scope.ptModel = {
                ptGroups: fanex.cloneObject(numberGameSetting.PositionTakingSetting),
                ptFirstData: fanex.cloneObject(numberGameSetting.PositionTakingSetting[0].PositionTakings[0]),
                settingContext: settingContext
            };

            $scope.updateSettingModel = function () {
                $scope.positionTakingService.updateScope($scope, fanex.cloneObject(numberGameSetting.PositionTakingSetting), languageService);
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
    };

    fanex.customerSetting.controller('NumberGamePositionTakingController',
    [
        '$scope',
        'Fanex.CustomerSetting.LanguageService',
        function ($scope, languageService) {
            'use strict';
            $scope.languageService = languageService;
            fanex.customerSetting.PositionTakingController.updateScope($scope, languageService);
        }
    ]);

    fanex.customerSetting.controller('NumberGamePositionTakingAdvancedModeController',
    [
        '$scope',
        'Fanex.CustomerSetting.LanguageService',
        function ($scope, languageService) {
            'use strict';
            $scope.languageService = languageService;
            fanex.customerSetting.PositionTakingController.updateScope($scope, languageService);
        }
    ]);
}