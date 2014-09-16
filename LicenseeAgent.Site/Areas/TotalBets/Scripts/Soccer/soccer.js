/*global core, language, pageContext*/

var totalBets = totalBets || {};
totalBets.soccer = (function ($) {
    'use strict';

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
            { HeaderText: "#", Width: 30, Name: "RowID", CanResize: false },
            { HeaderText: language.getString("Time"), Width: 80, Name: "Time", HeaderAlign: "center", CellAlign: "center", CanResize: false },
            { HeaderText: language.getString("Event"), Width: 200, Name: "Event", HeaderAlign: "center" },
            { HeaderText: language.getBetTypeName(1), Width: 120, Name: "FTHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(3), Width: 120, Name: "FTOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(5), Width: 120, Name: "FT1X2", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(7), Width: 120, Name: "FHHDP", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(8), Width: 120, Name: "FHOU", HeaderAlign: "center", CellAlign: "right", CanResize: false },
            { HeaderText: language.getBetTypeName(15), Width: 120, Name: "FH1X2", HeaderAlign: "center", CellAlign: "right", CanResize: false }
        ],
        indexNumberColumn: 1,
        autoResizeColumn: true
    };

    var sportType = 1;
    var initialized = false;
    return {
        init: function () {
            totalBets.soccer.initBetListGridOptions();
            totalBets.init(betListGridOptions);

            $('#BetListGrid').on('data', function (e, data) {
                gridOptions.bodyRows = totalBets.soccer.genGridRows(data);
                $(this).show().fanexGrid(gridOptions);
            });

            $('form').submit(function (e) {
                totalBets.soccer.getLeagues();
                totalBets.soccer.getEvents();
                totalBets.reRenderAfterPaged();
                e.preventDefault();
            });

            totalBets.soccer.getLeagues();
            totalBets.soccer.getEvents();
            totalBets.reRenderAfterPaged();
            initialized = true;
        },
        initBetListGridOptions: function () {
            var hiddenColumns = [];
            if (pageContext.LegendType === 1) {
                hiddenColumns.push(9);
            }
            if (pageContext.IsLastLevel) {
                hiddenColumns.push(10);
            }
            betListGridOptions.hiddenColumns = hiddenColumns;
            return betListGridOptions;
        },
        getLeagues: function () {
            core.ajaxJSON({
                async: false,
                url: core.resolveUrl("~/TotalBets/Soccer/GetLeagues"),
                data: {
                    sportType: sportType,
                    selectedLeagues: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', true).join(',') : $('#SelectedLeagues').val(),
                    checkedAll: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', false).length === 0 : $('#CheckAllLeague').val()
                }
            }, function (data) {
                totalBets.initLeagueMultiSelect(data, function () {
                    totalBets.soccer.getEvents();
                });
            });
        },
        getEvents: function () {
            core.ajaxJSON({
                async: false,
                url: core.resolveUrl("~/TotalBets/Soccer/GetEvents"),
                data: {
                    sportType: sportType,
                    selectedLeagues: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                    selectedEvents: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', true).join(',') : $('#SelectedEvents').val(),
                    checkedAll: initialized ? $("#event-multi-select").fanexMultiSelect('getItems', false).length === 0 : $('#CheckAllEvent').val()
                }
            }, function (data) {
                totalBets.initEventMultiSelect(data, function () {
                    totalBets.reRenderAfterPaged();
                });
            });
        },
        genGridRows: function (list) {
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
                        { Name: "FTHDP", Value: totalBets.generateBetListLink(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                        { Name: "FTOU", Value: totalBets.generateBetListLink(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                        { Name: "FT1X2", Value: totalBets.generateBetListLink(match.MatchId, 5, '1', showLiveTicket, match.HomeFulltimeOneXTwo) },
                        { Name: "FHHDP", Value: totalBets.generateBetListLink(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                        { Name: "FHOU", Value: totalBets.generateBetListLink(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) },
                        { Name: "FH1X2", Value: totalBets.generateBetListLink(match.MatchId, 15, '1', showLiveTicket, match.HomeFirstHalfOneXTwo) }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                        { Name: "FTHDP", Value: totalBets.generateBetListLink(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                        { Name: "FTOU", Value: totalBets.generateBetListLink(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                        { Name: "FT1X2", Value: totalBets.generateBetListLink(match.MatchId, 5, '2', showLiveTicket, match.AwayFulltimeOneXTwo) },
                        { Name: "FHHDP", Value: totalBets.generateBetListLink(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                        { Name: "FHOU", Value: totalBets.generateBetListLink(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) },
                        { Name: "FH1X2", Value: totalBets.generateBetListLink(match.MatchId, 15, '2', showLiveTicket, match.AwayFirstHalfOneXTwo) }
                    );

                    var moreLink = "<span class='more-link' onclick='totalBets.soccer.showDetail(this)' data-matchid='" + match.MatchId
                                    + "' data-matchname='" + match.HomeName + " -vs- " + match.AwayName + "'>" + language.getString("More") + "</span>";
                    var drawTeam = [];
                    drawTeam.push(
                        { Name: "Event", Value: language.getString("Draw"), TdClass: "draw" },
                        { Name: "FTHDP", Value: moreLink, MergeColNumber: 2, CssClass: "c-align" },
                        { Name: "FT1X2", Value: totalBets.generateBetListLink(match.MatchId, 5, 'x', showLiveTicket, match.DrawFulltimeOneXTwo) },
                        { Name: "FHHDP", Value: "", MergeColNumber: 2 },
                        { Name: "FH1X2", Value: totalBets.generateBetListLink(match.MatchId, 15, 'x', showLiveTicket, match.DrawFirstHalfOneXTwo) }
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
        },
        showDetail: function (context) {
            var url = core.resolveUrl("~/Totalbets/Soccer/Detail", {
                matchId: $(context).attr("data-matchid"),
                matchName: $(context).attr("data-matchname"),
                timeInterval: $('#server-time-interval').val(),
                selectedLeagues: $('#league-multi-select').fanexMultiSelect('getItems', true).join(','),
                checkAllLeague: $('#league-multi-select').fanexMultiSelect('getItems', false).length === 0,
                selectedEvents: $('#event-multi-select').fanexMultiSelect('getItems', true).join(','),
                checkAllEvent: $('#event-multi-select').fanexMultiSelect('getItems', false).length === 0
            });
            window.location.href = url;
        }
    };
} (jQuery));