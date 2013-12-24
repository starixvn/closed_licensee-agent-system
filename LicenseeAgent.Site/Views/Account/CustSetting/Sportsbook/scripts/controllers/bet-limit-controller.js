/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../../../../libs/angular.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.sportsbookSetting) {
    var positionTakingController = fanex.sportsbookSetting.controller('fanex.custsetting.betlimitcontroller',
    [
        '$scope',
        '$filter',
        '$http',
        function ($scope, $filter, $http) {
            $scope.language = fanex.sportsbookSetting.language[settingContext.Language];

            $scope.config = fanex.sportsbookSetting.betLimitService.config;
            $scope.settingContext = settingContext;
            $scope.betLimitService = fanex.sportsbookSetting.betLimitService;

            $scope.model = {
                // For all sport types
                bettingLimitsModel: fanex.cloneObject(betLimits),
                // For only soccer setting
                firstRecord: fanex.cloneObject(betLimits[0]),
                settingContext: settingContext
            };

            $scope.saveData = function (callBackFunction) {
                return $scope.betLimitService.getbettingLimits(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
            }

            $scope.isValidData = function () {
                return $scope.betLimitService.isValidData(
                    settingContext,
                    $scope.model.bettingLimitsModel,
                    $scope.model.firstRecord);
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