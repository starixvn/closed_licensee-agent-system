/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

if (fanex.customerSetting) {
    //// Helper
    fanex.customerSetting.baseBetLimitService = {
        showInvalidMessage: function (element, message) {
            'use strict';
            fanex.showHint(element, message);
        },
        isValidData: function (settingContext, bettingLimitsData, firstRecordData) {
            'use strict';
            if (settingContext.IsSimpleMode) {
                return firstRecordData.actions.isValid();
            }
            else {
                var datalength = bettingLimitsData.length;

                for (var i = 0; i < datalength; i++) {
                    var bettingLimit = bettingLimitsData[i];
                    if (!bettingLimit.actions.isValid()) {
                        return false;
                    }
                }
            }
            return true;
        },
        regIsDigit: function (input) {
            'use strict';
            var reg = new RegExp("^[0-9,]+$");
            return (reg.test(input));
        },
        formatInputNumber: function (input) {
            'use strict';
            return parseInt(input.toString().replace(/,/gi, ''));
        },
        reformatInputNumberForControl: function (input) {
            'use strict';
            var result = "";
            if (input === undefined) {
                return result;
            }
            input = input.toString().replace(/,/gi, '');
            if (input.length > 3) {
                for (var i = input.length - 3; i > 0; i = i - 3) {
                    var end = i + 3;
                    result = ',' + input.substring(i, end) + result;
                }

                result = input.substring(0, i + 3) + result;
            } else {
                result = input;
            }
            return result;
        },
        updateScope: function ($scope, settings) {
            'use strict';
            angular.extend($scope.model.firstRecord, settings[0]);
            $scope.betLimitService.changeSettingValidation($scope.model.firstRecord);

            angular.forEach($scope.model.bettingLimitsModel, function (model) {
                angular.forEach(settings, function (setting) {
                    if (model.SportId === setting.SportId && model.BetTypeId === setting.BetTypeId) {
                        angular.extend(model, setting);
                        $scope.betLimitService.changeSettingValidation(model);
                    }
                });
            });
        }
    };
}