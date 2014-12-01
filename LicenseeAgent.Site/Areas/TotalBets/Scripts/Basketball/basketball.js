/*global CORE, LANGUAGE, pageContext*/
'use strict';

var TOTAL_BETS = TOTAL_BETS || {};
TOTAL_BETS.BASKETBALL = (function ($this, $) {
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
            { HeaderText: "#", Width: 20, Name: "RowID", CanResize: false },
            { HeaderText: LANGUAGE.getString("Time"), Width: 80, Name: "Time", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: LANGUAGE.getString("Event"), Width: 200, Name: "Event", HeaderAlign: "center" },
            { HeaderText: LANGUAGE.getBetTypeName(1), Width: 120, Name: "FTHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(3), Width: 120, Name: "FTOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(2), Width: 120, Name: "FTOE", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(7), Width: 120, Name: "FHHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(8), Width: 120, Name: "FHOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(12), Width: 120, Name: "FHOE", HeaderAlign: "center", CellAlign: "right", CanResize: false }
        ],
        autoResizeColumn: true
    };

    var sportType = 2;
    var initialized = false;
    var genGridBodyRows = function (list) {
        var len = list.length;
        var bodyRows = [];
        var oldLeagueId = 0;
        var rowIndex = 0;

        for (var i = 0; i < len; i++) {//League
            var league = list[i];
            var newLeagueId = league.LeagueId;

            if (oldLeagueId !== newLeagueId) {
                rowIndex = 0;
                oldLeagueId = newLeagueId;
                //Push League row
                bodyRows.push({
                    Columns: ([
                            { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 13 }
                        ]),
                    CssClass: "league-row"
                });
            }

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
                    { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>", MergeRowNumber: 2 },
                    { Name: "Time", Value: match.Time, MergeRowNumber: 2, CssClass: "bold" },
                    { Name: "Event", Value: match.HomeName, TdClass: "team-name" },
                    { Name: "FTHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                    { Name: "FTOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                    { Name: "FTOE", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 2, 'h', showLiveTicket, match.HomeFulltimeOddEven) },
                    { Name: "FHHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                    { Name: "FHOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) },
                    { Name: "FHOE", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 12, 'h', showLiveTicket, match.HomeFirstHalfOddEven) }
                );

                var awayTeam = [];
                //Away team
                awayTeam.push(
                    { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                    { Name: "FTHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                    { Name: "FTOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                    { Name: "FTOE", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 2, 'a', showLiveTicket, match.AwayFulltimeOddEven) },
                    { Name: "FHHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                    { Name: "FHOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) },
                    { Name: "FHOE", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 12, 'a', showLiveTicket, match.AwayFirstHalfOddEven) }
                );

                bodyRows.push({
                    Columns: homeTeam,
                    CssClass: match.LiveIndicator ? "live-row" : "normal-row"
                });
                bodyRows.push({
                    Columns: awayTeam,
                    CssClass: match.LiveIndicator ? "live-row" : "normal-row"
                });
            }
        }
        return bodyRows;
    };

    var getLeagues = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/Basketball/GetLeagues"),
            data: {
                sportType: sportType,
                selectedLeagues: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            TOTAL_BETS.initLeagueMultiSelect(data, function () {
                TOTAL_BETS.BASKETBALL.getEvents();
            });
        });
    };

    var getEvents = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/Basketball/GetEvents"),
            data: {
                sportType: sportType,
                selectedLeagues: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedEvents: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            TOTAL_BETS.initEventMultiSelect(data, function () {
                TOTAL_BETS.reRenderAfterPaged();
            });
        });
    };

    $this.init = function () {
        var hiddenColumns = [];
        if (pageContext.LegendType === 1) {
            hiddenColumns.push(9);
        }
        if (pageContext.IsLastLevel) {
            hiddenColumns.push(10);
        }
        gridOptions.hiddenColumns = hiddenColumns;

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

        $('.freegrid').on('data', function (e, data) {
            gridOptions.bodyRows = genGridBodyRows(data);
            $(this).show().fanexGrid(gridOptions);
        });

        getLeagues();
        getEvents();
        TOTAL_BETS.reRenderAfterPaged();
        initialized = true;
    };

    return $this;
} (TOTAL_BETS.BASKETBALL || {}, jQuery));