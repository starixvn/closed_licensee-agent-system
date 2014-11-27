/*global core, language, pageContext, refreshCountDownMethods*/
'use strict';

var totalBets = totalBets || {};
totalBets.otherSports = (function ($this, $) {
    var betListGridOptions = {
        columns: [
            { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: language.getString("TransTime"), Width: 100, Name: "TransTime", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getString("Stake"), Width: 118, Name: "Stake", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getString("WinLoss"), Width: 125, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center", CanResize: false }
        ],
        showPager: true,
        enableHiddenColumns: false,
        autoResizeColumn: true
    };
    var gridOptions = {
        columns: [
            { HeaderText: "#", Width: 20, Name: "RowID", CanResize: false },
            { HeaderText: language.getString("Time"), Width: 80, Name: "Time", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString("Event"), Width: 200, Name: "Event", HeaderAlign: "center" },
            { HeaderText: language.getBetTypeName(20), Width: 120, Name: "MoneyLine", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(1), Width: 120, Name: "FTHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(3), Width: 120, Name: "FTOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(5), Width: 120, Name: "FT1X2", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(2), Width: 120, Name: "FTOE", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(7), Width: 120, Name: "FHHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(8), Width: 120, Name: "FHOU", HeaderAlign: "center", CellAlign: "right", CanResize: false }
        ],
        indexNumberColumn: 1,
        autoResizeColumn: true
    };

    var initialized = false;

    $this.init = function () {
        var hiddenColumns = [];
        if (pageContext.LegendType === 1) {
            hiddenColumns.push(9);
        }
        if (pageContext.IsLastLevel) {
            hiddenColumns.push(10);
        }
        betListGridOptions.hiddenColumns = hiddenColumns;

        totalBets.init(betListGridOptions);

        // search
        $('form').submit(function (e) {
            totalBets.otherSports.getSports();
            totalBets.otherSports.getLeagues();
            totalBets.otherSports.getEvents();
            totalBets.otherSports.reRenderAfterPaged();
            e.preventDefault();
        });

        totalBets.otherSports.getSports();
        totalBets.otherSports.getLeagues();
        totalBets.otherSports.getEvents();
        totalBets.otherSports.reRenderAfterPaged();
        initialized = true;
    };

    $this.getSports = function () {
        core.ajaxJSON({
            async: false,
            url: core.resolveUrl("~/TotalBets/OtherSports/GetSports"),
            data: {
                selectedSports: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            totalBets.initSportMultiSelect(data, function () {
                totalBets.otherSports.getLeagues();
                totalBets.otherSports.getEvents();
            });
        });
    };

    $this.getLeagues = function () {
        core.ajaxJSON({
            async: false,
            url: core.resolveUrl("~/Totalbets/OtherSports/GetLeagues"),
            data: {
                selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedLeagues: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            totalBets.initLeagueMultiSelect(data, function () {
                totalBets.otherSports.getEvents();
            });
        });
    };

    $this.getEvents = function () {
        core.ajaxJSON({
            async: false,
            url: core.resolveUrl("~/Totalbets/OtherSports/GetEvents"),
            data: {
                selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedLeagues: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedEvents: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            totalBets.initEventMultiSelect(data, function () {
                totalBets.otherSports.reRenderAfterPaged();
            });
        });
    };

    $this.reRenderAfterPaged = function () {
        core.ajaxJSON({
            url: $('form').attr('action'),
            data: {
                SportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            }
        }, function (data) {
            gridOptions.bodyRows = totalBets.otherSports.genGridRows(data);
            $('.freegrid').show().fanexGrid(gridOptions);
            refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
        });
    };

    $this.genGridRows = function (list) {
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
                        { Name: "MoneyLine", Value: totalBets.generateBetListLink(match.MatchId, 20, 'h', showLiveTicket, match.HomeFulltimeMoneyLine) },
                        { Name: "FTHDP", Value: totalBets.generateBetListLink(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                        { Name: "FTOU", Value: totalBets.generateBetListLink(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                        { Name: "FT1X2", Value: totalBets.generateBetListLink(match.MatchId, 5, '1', showLiveTicket, match.HomeFulltimeOneXTwo) },
                        { Name: "FTOE", Value: totalBets.generateBetListLink(match.MatchId, 2, 'h', showLiveTicket, match.HomeFulltimeOddEven) },
                        { Name: "FHHDP", Value: totalBets.generateBetListLink(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                        { Name: "FHOU", Value: totalBets.generateBetListLink(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                        { Name: "MoneyLine", Value: totalBets.generateBetListLink(match.MatchId, 20, 'a', showLiveTicket, match.AwayFulltimeMoneyLine) },
                        { Name: "FTHDP", Value: totalBets.generateBetListLink(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                        { Name: "FTOU", Value: totalBets.generateBetListLink(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                        { Name: "FT1X2", Value: totalBets.generateBetListLink(match.MatchId, 5, '2', showLiveTicket, match.AwayFulltimeOneXTwo) },
                        { Name: "FTOE", Value: totalBets.generateBetListLink(match.MatchId, 2, 'a', showLiveTicket, match.AwayFulltimeOddEven) },
                        { Name: "FHHDP", Value: totalBets.generateBetListLink(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                        { Name: "FHOU", Value: totalBets.generateBetListLink(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) }
                    );

                    var drawRow = [];
                    drawRow.push(
                        { Name: "Event", Value: language.getString("Draw"), TdClass: "draw" },
                        { Name: "MoneyLine", Value: "", MergeColNumber: 3 },
                        { Name: "FT1X2", Value: totalBets.generateBetListLink(match.MatchId, 5, 'x', showLiveTicket, match.DrawFulltimeOneXTwo) },
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

    return $this;
} (totalBets.otherSports || {}, jQuery));