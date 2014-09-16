/*global core*/

var totalBets = totalBets || {};
totalBets.soccer = totalBets.soccer || {};
totalBets.soccer.detail = (function ($) {
    'use strict';
    return {
        init: function () {
            var betListGridOptions = totalBets.soccer.initBetListGridOptions();
            totalBets.init(betListGridOptions);

            var matchId = $('#soccer-more-betlist').attr("data-matchid");
            var bettype = $('#soccer-more-betlist').attr("data-bettype");
            var betteam = $('#soccer-more-betlist').attr("data-betteam");
            totalBets.showBetList(matchId, bettype, betteam, false);
        },
        showBetList: function (matchid, bettype, betteam) {
            var url = core.resolveUrl('~/Totalbets/Soccer/DetailBetList', {
                matchId: matchid,
                bettype: bettype,
                betteam: betteam
            });
            window.location.href = url;
        }
    };
} (jQuery));