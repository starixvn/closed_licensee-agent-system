/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global language */

if (fanex.customerSetting) {
    //// Helper

    fanex.customerSetting.factory('Fanex.CustomerSetting.LanguageService', function () {
        'use strict';
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

                if (isLive !== undefined) {
                    var status;
                    if (isLive === true) {
                        status = language.getString('Live');
                    } else if (isLive === false) {
                        status = language.getString('DeadBall');
                    }
                    if (sportTypeId === 1) {
                        result = result + ' - ' + status;
                    }
                    if (sportTypeId === 161) {
                        result = status;
                    }
                }

                return result;
            },
            getBetTypeName: function (betTypeId) {
                var result = '';
                switch (betTypeId) {
                    case 5:
                        result = language.getString('OneXTwo');
                        break;
                    case 81:
                        result = language.getString('FirstLastBallOverUnder');
                        break;
                    case 83:
                        result = language.getString('FirstLastBallOddEven');
                        break;
                    default:
                        result = language.getBetTypeName(betTypeId);
                }
                return result;
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