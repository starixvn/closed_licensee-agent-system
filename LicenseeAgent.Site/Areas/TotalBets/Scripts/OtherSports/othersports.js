/*global CORE, LANGUAGE, pageContext, refreshCountDownMethods*/
'use strict';

var TOTAL_BETS = TOTAL_BETS || {};
TOTAL_BETS.OTHER_SPORTS = (function ($this, $) {
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
            { HeaderText: LANGUAGE.getBetTypeName(20), Width: 120, Name: "MoneyLine", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(1), Width: 120, Name: "FTHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(3), Width: 120, Name: "FTOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(5), Width: 120, Name: "FT1X2", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(2), Width: 120, Name: "FTOE", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(7), Width: 120, Name: "FHHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: LANGUAGE.getBetTypeName(8), Width: 120, Name: "FHOU", HeaderAlign: "center", CellAlign: "right", CanResize: false }
        ],
        autoResizeColumn: true
    };

    var initialized = false;
    var getSports = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/OtherSports/GetSports"),
            data: {
                selectedSports: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            TOTAL_BETS.initSportMultiSelect(data, function () {
                getLeagues();
                getEvents();
            });
        });
    };

    var getLeagues = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/OtherSports/GetLeagues"),
            data: {
                selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedLeagues: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
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
            url: CORE.resolveUrl("~/TotalBets/OtherSports/GetEvents"),
            data: {
                selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedLeagues: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedEvents: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            TOTAL_BETS.initEventMultiSelect(data, function () {
                reRenderAfterPaged();
            });
        });
    };

    var reRenderAfterPaged = function () {
        CORE.ajaxJSON({
            url: $('form').attr('action'),
            data: {
                SportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            }
        }, function (data) {
            gridOptions.bodyRows = genGridBodyRows(data);
            $('.freegrid').show().fanexGrid(gridOptions);
            refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
        });
    };

    var genGridBodyRows = function (list) {
        var lenSports = list.length;
        var bodyRows = [];
        var oldSportId = 0;
        var oldLeagueId = 0;
        var rowIndex = 0;

        for (var k = 0; k < lenSports; k++) {//Sport
            var sport = list[k];
            //Push Sport row
            if (oldSportId !== sport.SportType) {
                oldSportId = sport.SportType;
                bodyRows.push({
                    Columns: ([
                        { Name: "RowID", Value: "<div align='left'>" + sport.SportName + "</div>", MergeColNumber: 15 }
                    ]),
                    CssClass: "sport-row"
                });
            }

            //League in sport
            var leagues = sport.LeagueList;
            var len = leagues.length;
            for (var i = 0; i < len; i++) {
                var league = leagues[i];
                //Push League row
                if (oldLeagueId !== league.LeagueId) {
                    rowIndex = 0;
                    oldLeagueId = league.LeagueId;
                    bodyRows.push({
                        Columns: ([
                                { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 15 }
                            ]),
                        CssClass: "league-row"
                    });
                }

                //Push Match Row in League
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
                        { Name: "MoneyLine", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 20, 'h', showLiveTicket, match.HomeFulltimeMoneyLine) },
                        { Name: "FTHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                        { Name: "FTOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                        { Name: "FT1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 5, '1', showLiveTicket, match.HomeFulltimeOneXTwo) },
                        { Name: "FTOE", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 2, 'h', showLiveTicket, match.HomeFulltimeOddEven) },
                        { Name: "FHHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                        { Name: "FHOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                        { Name: "MoneyLine", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 20, 'a', showLiveTicket, match.AwayFulltimeMoneyLine) },
                        { Name: "FTHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                        { Name: "FTOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                        { Name: "FT1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 5, '2', showLiveTicket, match.AwayFulltimeOneXTwo) },
                        { Name: "FTOE", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 2, 'a', showLiveTicket, match.AwayFulltimeOddEven) },
                        { Name: "FHHDP", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                        { Name: "FHOU", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) }
                    );

                    var drawRow = [];
                    drawRow.push(
                        { Name: "Event", Value: LANGUAGE.getString("Draw"), TdClass: "draw" },
                        { Name: "MoneyLine", Value: "", MergeColNumber: 3 },
                        { Name: "FT1X2", Value: TOTAL_BETS.generateBetListLink(match.MatchId, 5, 'x', showLiveTicket, match.DrawFulltimeOneXTwo) },
                        { Name: "FTOE", Value: "", MergeColNumber: 3 }
                    );

                    var cssRow = match.LiveIndicator ? "live-row" : "normal-row";

                    bodyRows.push({
                        Columns: homeTeam,
                        CssClass: cssRow
                    });
                    bodyRows.push({
                        Columns: awayTeam,
                        CssClass: cssRow
                    });
                    bodyRows.push({
                        Columns: drawRow,
                        CssClass: cssRow
                    });
                }
            }
        }
        return bodyRows;
    };

    $this.init = function () {
        var hiddenColumns = [];
        if (pageContext.LegendType === 1) {
            hiddenColumns.push(9);
        }
        if (pageContext.IsLastLevel) {
            hiddenColumns.push(10);
        }
        betListGridOptions.hiddenColumns = hiddenColumns;

        TOTAL_BETS.init(betListGridOptions);

        // search
        $('form').submit(function (e) {
            getSports();
            getLeagues();
            getEvents();
            reRenderAfterPaged();
            e.preventDefault();
        });

        getSports();
        getLeagues();
        getEvents();
        reRenderAfterPaged();
        initialized = true;
    };

    return $this;
} (TOTAL_BETS.OTHER_SPORTS || {}, jQuery));