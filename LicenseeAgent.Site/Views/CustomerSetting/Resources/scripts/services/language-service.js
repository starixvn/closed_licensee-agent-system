/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global language */

'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.languageService = {
        getSportName: function (sportTypeId) {
            return language.getSportName(sportTypeId);
        },
        getBetTypeName: function (betTypeId) {
            return language.getBetTypeName(betTypeId);
        },
        getString: function (key) {
            var value = language.getResourceString(fanex.customerSetting.language, key);
            if (!value) {
                value = language.getString(key);
            }
            return value;
        }
    };
}