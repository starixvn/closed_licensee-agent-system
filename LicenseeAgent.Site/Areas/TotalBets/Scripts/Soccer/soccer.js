/*global CORE, LANGUAGE, pageContext*/
'use strict';

var TOTAL_BETS = TOTAL_BETS || {};
TOTAL_BETS.SOCCER = (function ($this, $) {
    var betListGridOptions = {
        columns: [
            { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: LANGUAGE.getString("TransTime"), Width: 100, Name: "TransTime", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center", CanResize: false }
        ],
        showPager: true,
        enableHiddenColumns: false,
        autoResizeColumn: true
    };

    var gridOptions = {
        columns: [
            { HeaderText: "#", Width: 30, Name: "RowID", CanResize: false },
            { HeaderText: LANGUAGE.getString("Time"), Width: 80, Name: "Time", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("Event"), Width: 200, Name: "Event", HeaderAlign: "center" },
            { HeaderText: LANGUAGE.getBetTypeName(1), Width: 120, Name: "FTHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(3), Width: 120, Name: "FTOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(5), Width: 120, Name: "FT1X2", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(7), Width: 120, Name: "FHHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(8), Width: 120, Name: "FHOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(15), Width: 120, Name: "FH1X2", HeaderAlign: "center", CellAlign: "right", CanResize: false }
        ],
        autoResizeColumn: true
    };

    var sportType = 1;
    var initialized = false;
    var initBetListGridOptions = function () {
        var hiddenColumns = [];
        if (pageContext.LegendType === 1) {
            hiddenColumns.push(9);
        }
        if (pageContext.IsLastLevel) {
            hiddenColumns.push(10);
        }
        betListGridOptions.hiddenColumns = hiddenColumns;
        return betListGridOptions;
    };

    var getLeagues = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/Soccer/GetLeagues"),
            data: {
                sportType: sportType,
                selectedLeagues: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', true).join(',') : $('#SelectedLeagues').val(),
                checkedAll: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', false).length === 0 : $('#CheckAllLeague').val()
            }
        }, function (data) {
            TOTAL_BETS.initLeagueMultiSelect(data, function () {
                getEvents();
            });
        });
    };

    var getEvents = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/Soccer/GetEvents"),
            data: {
                sportType: sportType,
                selectedLeagues: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedEvents: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', true).join(',') : $('#SelectedEvents').val(),
                checkedAll: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', false).length === 0 : $('#CheckAllEvent').val()
            }
        }, function (data) {
            TOTAL_BETS.initEventMultiSelect(data, function () {
                TOTAL_BETS.reRenderAfterPaged();
            });
        });
    };

    var genGridBodyRows = function (list) {
        var len = list.length;
        var bodyRows = [];
        var rowIndex = 0;

        for (var i = 0; i < len; i++) {//League
            var league = list[i];
            //Push League row
            bodyRows.push({
                Columns: ([
                    { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 9 }
                ]),
                CssClass: "league-row"
            });

            //Push Match Row
            var matches = league.MatchList;
            var numMatches = matches.length;

            for (var j = 0; j < numMatches; j++) {
                var match = matches[j];
                var homeTeam = [];
                var showLiveTicket = match.LiveIndicator ? true : false;
                rowIndex++;
                //Home team
                homeTeam.push(
                    { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>", MergeRowNumber: 3 },
                    { Name: "Time", Value: match.Time, MergeRowNumber: 3, CssClass: "bold" },
                    { Name: "Event", Value: match.HomeName, TdClass: "team-name" },
                    { Name: "FTHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                    { Name: "FTOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                    { Name: "FT1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 5, '1', showLiveTicket, match.HomeFulltimeOneXTwo) },
                    { Name: "FHHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                    { Name: "FHOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) },
                    { Name: "FH1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 15, '1', showLiveTicket, match.HomeFirstHalfOneXTwo) }
                );

                var awayTeam = [];
                //Away team
                awayTeam.push(
                    { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                    { Name: "FTHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                    { Name: "FTOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                    { Name: "FT1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 5, '2', showLiveTicket, match.AwayFulltimeOneXTwo) },
                    { Name: "FHHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                    { Name: "FHOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) },
                    { Name: "FH1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 15, '2', showLiveTicket, match.AwayFirstHalfOneXTwo) }
                );

                var moreLink = "<span class='more-link' onclick='TOTAL_BETS.SOCCER.showDetail(this)' data-matchid='" + match.MatchId
                    + "' data-matchname='" + match.HomeName + " -vs- " + match.AwayName + "'>" + LANGUAGE.getString("More") + "</span>";
                var drawTeam = [];
                drawTeam.push(
                    { Name: "Event", Value: LANGUAGE.getString("Draw"), TdClass: "draw" },
                    { Name: "FTHDP", Value: moreLink, MergeColNumber: 2, CssClass: "c-align" },
                    { Name: "FT1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 5, 'x', showLiveTicket, match.DrawFulltimeOneXTwo) },
                    { Name: "FHHDP", Value: "", MergeColNumber: 2 },
                    { Name: "FH1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 15, 'x', showLiveTicket, match.DrawFirstHalfOneXTwo) }
                );

                bodyRows.push({
                    Columns: homeTeam,
                    CssClass: match.LiveIndicator ? "live-row" : "normal-row"
                });
                bodyRows.push({
                    Columns: awayTeam,
                    CssClass: match.LiveIndicator ? "live-row" : "normal-row"
                });
                bodyRows.push({
                    Columns: drawTeam,
                    CssClass: match.LiveIndicator ? "live-row" : "normal-row"
                });
            }
        }
        return bodyRows;
    };

    $this.init = function () {
        initBetListGridOptions();
        TOTAL_BETS.init(betListGridOptions);

        $('.freegrid').on('data', function (e, data) {
            gridOptions.bodyRows = genGridBodyRows(data);
            $(this).show().fanexGrid(gridOptions);
        });

        $('form').submit(function (e) {
            getLeagues();
            getEvents();
            TOTAL_BETS.reRenderAfterPaged();
            e.preventDefault();
        });

        getLeagues();
        getEvents();
        TOTAL_BETS.reRenderAfterPaged();
        initialized = true;
    };

    $this.showDetail = function (context) {
        var url = CORE.resolveUrl("~/TotalBets/Soccer/Detail", {
            matchId: $(context).attr("data-matchid"),
            matchName: $(context).attr("data-matchname"),
            timeInterval: $('#server-time-interval').val(),
            selectedLeagues: $('#league-multi-select').fanexMultiSelect('getItems', true).join(','),
            checkAllLeague: $('#league-multi-select').fanexMultiSelect('getItems', false).length === 0,
            selectedEvents: $('#event-multi-select').fanexMultiSelect('getItems', true).join(','),
            checkAllEvent: $('#event-multi-select').fanexMultiSelect('getItems', false).length === 0
        });
        window.location.href = url;
    };

    return $this;
} (TOTAL_BETS.SOCCER || {}, jQuery));