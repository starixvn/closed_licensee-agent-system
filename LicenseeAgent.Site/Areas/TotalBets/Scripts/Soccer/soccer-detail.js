/*global CORE*/
'use strict';

var TOTAL_BETS = TOTAL_BETS || {};
TOTAL_BETS.SOCCER = TOTAL_BETS.SOCCER || {};
TOTAL_BETS.SOCCER.DETAIL = (function ($this, $) {
    $this.init = function () {
        var betListGridOptions = TOTAL_BETS.SOCCER.initBetListGridOptions();
        TOTAL_BETS.init(betListGridOptions);

        var matchId = $('#soccer-more-betlist').attr("data-matchid");
        var bettype = $('#soccer-more-betlist').attr("data-bettype");
        var betteam = $('#soccer-more-betlist').attr("data-betteam");
        TOTAL_BETS.showBetList(matchId, bettype, betteam, false);
    };

    $this.showBetList = function (matchid, bettype, betteam) {
        var url = CORE.resolveUrl('~/TotalBets/Soccer/DetailBetList', {
            matchId: matchid,
            bettype: bettype,
            betteam: betteam
        });
        window.location.href = url;
    };

    return $this;
} (TOTAL_BETS.SOCCER.DETAIL || {}, jQuery));