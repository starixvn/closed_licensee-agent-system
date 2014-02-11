/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/fanex.core.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.customerSetting) {
    var positionTakingController = fanex.customerSetting.controller('sportsbook.positionTakingController',
    [
        '$scope',
        '$filter',
        function ($scope, $filter) {
            $scope.language = fanex.customerSetting.language[settingContext.Language];
            fanex.customerSetting.positionTakingService.init(positionTakingSetting, $scope.language);
            $scope.config = fanex.customerSetting.positionTakingService.config;
            $scope.settingContext = settingContext;
            $scope.positionTakingService = fanex.customerSetting.positionTakingService;
            $scope.ptModel = {
                ptGroups: fanex.cloneObject(positionTakingSetting),
                ptFirstData: fanex.cloneObject(positionTakingSetting[0].PositionTakings[0]),
                settingContext: settingContext
            };

            $scope.saveData = function (callBackFunction) {
                return $scope.positionTakingService.getPositionTakingUpdate(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            }

            $scope.isValidData = function () {
                return $scope.positionTakingService.isValidData(
                    settingContext,
                    $scope.ptModel.ptFirstData,
                    $scope.ptModel.ptGroups);
            };

            return {
                formatNumber: function (number) {
                    return $filter('number')(number, 2);
                },
                getScope: function () {
                    return $scope;
                }
            }
        } ]);
}