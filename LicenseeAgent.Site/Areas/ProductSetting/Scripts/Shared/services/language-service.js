/// <reference path="../../../../libs/angular.js" />
/// <reference path="../../../../libs/jquery-1.10.2.js" />
/// <reference path="../fanex.sports-book-setting.js" />

/*global LANGUAGE*/

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
                            result = LANGUAGE.getString('OtherSports');
                        } else {
                            result = LANGUAGE.getString('MixSportsParlay');
                        }
                        break;

                    case 154:
                        result = LANGUAGE.getString('HrFixedOdds');
                        break;

                    default:
                        result = LANGUAGE.getSportName(sportTypeId);
                }

                if (isLive !== undefined) {
                    var status;
                    if (isLive === true) {
                        status = LANGUAGE.getString('Live');
                    } else if (isLive === false) {
                        status = LANGUAGE.getString('DeadBall');
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
                        result = LANGUAGE.getString('OneXTwo');
                        break;
                    case 81:
                        result = LANGUAGE.getString('FirstLastBallOverUnder');
                        break;
                    case 83:
                        result = LANGUAGE.getString('FirstLastBallOddEven');
                        break;
                    default:
                        result = LANGUAGE.getBetTypeName(betTypeId);
                }
                return result;
            },
            getString: function (key) {
                return LANGUAGE.getString(key);
            },
            getFormatString: function () {
                return LANGUAGE.getFormatString.apply(LANGUAGE, arguments);
            }
        };
    });
}