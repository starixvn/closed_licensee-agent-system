/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global language */

'use strict';

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.factory('Fanex.CustomerSetting.LanguageService', function () {
        return {
            getSportName: function (sportTypeId, betTypeId, isLive) {
                var result = '';
                switch (sportTypeId) {
                    case 99:
                        if (betTypeId === 1) {
                            result = language.getString('OtherSports');
                        } else {
                            result = language.getString('MixSportsParlay');
                        }
                        break;

                    case 154:
                        result = language.getString('HrFixedOdds');
                        break;

                    default:
                        result = language.getSportName(sportTypeId);
                }

                if (isLive !== undefined && sportTypeId === 1 || sportTypeId === 161) {
                    if (isLive === true) {
                        result = result + ' - ' + language.getString('Live');
                    } else if (isLive === false) {
                        result = result + ' - ' + language.getString('DeadBall');
                    }
                }

                return result;
            },
            getBetTypeName: function (betTypeId) {
                return language.getBetTypeName(betTypeId);
            },
            getString: function (key) {
                return language.getString(key);
            }, 
            getFormatString: function () {
                return language.getFormatString.apply(language, arguments);
            }
        };
    });
}