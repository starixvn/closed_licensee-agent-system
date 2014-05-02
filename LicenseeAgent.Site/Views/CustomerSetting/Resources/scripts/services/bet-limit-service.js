/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

'use strict';

if (fanex.customerSetting) {
    //// Helper
    fanex.customerSetting.baseBetLimitService = {
        showInvalidMessage: function (element, message) {
            fanex.showHint(element, message);
        },
        isValidData: function (settingContext, bettingLimitsData, firstRecordData) {
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
            var reg = new RegExp("^[0-9,]+$");
            return (reg.test(input));
        },
        formatInputNumber: function (input) {
            return parseInt(input.toString().replace(/,/gi, ''));
        },
        reformatInputNumberForControl: function (input) {
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
        }
    };
}