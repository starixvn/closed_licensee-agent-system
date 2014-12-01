/*global refreshCountDownMethods, CORE, LANGUAGE, BET_LIST, pageContext*/
'use strict';

var TOTAL_BETS = TOTAL_BETS || {};
TOTAL_BETS.OUTRIGHT = (function ($this, $) {
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
            { HeaderText: "#", Width: 40, Name: "RowID", CanResize: false },
            { HeaderText: LANGUAGE.getString("Team"), Width: 520, Name: "Team", HeaderAlign: "center", CellAlign: "left", CanResize: false },
            { HeaderText: LANGUAGE.getString("BetCount"), Width: 100, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0." },
            { HeaderText: LANGUAGE.getString("Balance"), Width: 150, Name: "Balance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: LANGUAGE.getString("Forecast"), Width: 150, Name: "Forecast", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00",
                FormatPositive: '$'
            }
        ],
        autoResizeColumn: true
    };

    var initialized = false;
    var getSports = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/Outright/GetSports"),
            data: {
                selectedSports: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            TOTAL_BETS.initSportMultiSelect(data, function () {
                getLeagues();
                getTeams();
            });
        });
    };

    var getLeagues = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/Totalbets/Outright/GetLeagues"),
            data: {
                selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedLeagues: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            TOTAL_BETS.initLeagueMultiSelect(data, function () {
                getTeams();
            });
        });
    };

    var getTeams = function () {
        CORE.ajaxJSON({
            async: false,
            url: CORE.resolveUrl("~/TotalBets/Outright/GetTeams"),
            data: {
                selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedLeagues: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                selectedTeams: initialized ? $("#team-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                checkedAll: initialized ? $("#team-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
            }
        }, function (data) {
            TOTAL_BETS.initTeamMultiSelect(data, function () {
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
                TeamList: $("#team-multi-select").fanexMultiSelect('getItems', true).join(',')
            }
        }, function (data) {
            gridOptions.bodyRows = genGridBodyRows(data);
            $('.freegrid').show().fanexGrid(gridOptions);
            refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
        });
    };

    var genGridBodyRows = function (list) {
        var len = list.length;
        var bodyRows = [];
        var rowIndex = 0;

        for (var k = 0; k < len; k++) { //Sport
            var sport = list[k];

            //Push Sport row
            bodyRows.push({
                Columns: ([
                    { Name: "RowID", Value: "<div align='left'>" + sport.SportName + "</div>", MergeColNumber: 5 }
                ]),
                CssClass: "sport-row"
            });

            var leagues = sport.LeagueList;
            var numLeagues = leagues.length;
            for (var i = 0; i < numLeagues; i++) {//League
                var league = leagues[i];
                //Push League row
                bodyRows.push({
                    Columns: ([
                            { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 5 }
                        ]),
                    CssClass: "league-row"
                });

                //Push Team Row
                var teams = league.TeamList;
                var numTeams = teams.length;

                for (var j = 0; j < numTeams; j++) {
                    var match = teams[j];
                    var teamRow = [];
                    rowIndex++;
                    //Home team
                    teamRow.push(
                        { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>" },
                        { Name: "Team", Value: TOTAL_BETS.OUTRIGHT.generateBetListLink(league.LeagueId, match.TeamId, match.TeamName), CssClass: "bold" },
                        { Name: "BetCount", Value: match.BetCount },
                        { Name: "Balance", Value: match.Balance },
                        { Name: "Forecast", Value: match.Forecast }
                    );

                    bodyRows.push({
                        Columns: teamRow
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

        $('form').submit(function (e) {
            getSports();
            getLeagues();
            getTeams();
            reRenderAfterPaged();
            e.preventDefault();
        });

        getSports();
        getLeagues();
        getTeams();
        reRenderAfterPaged();
        initialized = true;
    };

    $this.generateBetListLink = function (leagueId, teamId, linkvalue) {
        var linkHtmlString = "<span class=\"data-link\" onclick=\"TOTAL_BETS.OUTRIGHT.showBetList(";
        linkHtmlString += leagueId + "," + teamId + ")\">";
        linkHtmlString += linkvalue + "</span>";
        return linkHtmlString;
    };

    $this.showBetList = function (leagueId, teamId) {
        refreshCountDownMethods.stopCountDown();
        $('form').hide();
        $("#totalbets-back").attr("class", "history-back");

        CORE.ajaxJSON({
            url: CORE.resolveUrl("~/BetList/Outright"),
            data: {
                LeagueId: leagueId,
                TeamId: teamId
            }
        }, function (data) {
            BET_LIST.reRenderAfterPagedSuccessfully(betListGridOptions, data);
        });
    };

    return $this;
} (TOTAL_BETS.OUTRIGHT || {}, jQuery));