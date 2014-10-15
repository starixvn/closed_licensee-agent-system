/*global refreshCountDownMethods, core, language, pageContext*/

var totalBets = totalBets || {};

totalBets.outright = (function ($) {
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
            { HeaderText: "#", Width: 40, Name: "RowID", CanResize: false },
            { HeaderText: language.getString("Team"), Width: 520, Name: "Team", HeaderAlign: "center", CellAlign: "left", CanResize: false },
            { HeaderText: language.getString("BetCount"), Width: 100, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0." },
            { HeaderText: language.getString("Balance"), Width: 150, Name: "Balance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString("Forecast"), Width: 150, Name: "Forecast", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00",
                FormatPositive: '$'
            }
        ],
        indexNumberColumn: 1,
        autoResizeColumn: true
    };

    var initialized = false;
    return {
        init: function () {
            var hiddenColumns = [];
            if (pageContext.LegendType === 1) {
                hiddenColumns.push(9);
            }
            if (pageContext.IsLastLevel) {
                hiddenColumns.push(10);
            }
            betListGridOptions.hiddenColumns = hiddenColumns;

            totalBets.init(betListGridOptions);

            $('form').submit(function (e) {
                totalBets.outright.getSports();
                totalBets.outright.getLeagues();
                totalBets.outright.getTeams();
                totalBets.outright.reRenderAfterPaged();
                e.preventDefault();
            });

            totalBets.outright.getSports();
            totalBets.outright.getLeagues();
            totalBets.outright.getTeams();
            totalBets.outright.reRenderAfterPaged();
            initialized = true;
        },
        getSports: function () {
            core.ajaxJSON({
                async: false,
                url: core.resolveUrl("~/TotalBets/Outright/GetSports"),
                data: {
                    selectedSports: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                    checkedAll: initialized ? $("#sport-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
                }
            }, function (data) {
                totalBets.initSportMultiSelect(data, function () {
                    totalBets.outright.getLeagues();
                    totalBets.outright.getTeams();
                });
            });
        },
        getLeagues: function () {
            core.ajaxJSON({
                async: false,
                url: core.resolveUrl("~/Totalbets/Outright/GetLeagues"),
                data: {
                    selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                    selectedLeagues: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                    checkedAll: initialized ? $("#league-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
                }
            }, function (data) {
                totalBets.initLeagueMultiSelect(data, function () {
                    totalBets.outright.getTeams();
                });
            });
        },
        getTeams: function () {
            core.ajaxJSON({
                async: false,
                url: core.resolveUrl("~/TotalBets/Outright/GetTeams"),
                data: {
                    selectedSports: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                    selectedLeagues: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                    selectedTeams: initialized ? $("#team-multi-select").fanexMultiSelect('getItems', true).join(',') : '',
                    checkedAll: initialized ? $("#team-multi-select").fanexMultiSelect('getItems', false).length === 0 : true
                }
            }, function (data) {
                totalBets.initTeamMultiSelect(data, function () {
                    totalBets.outright.reRenderAfterPaged();
                });
            });
        },
        reRenderAfterPaged: function () {
            core.ajaxJSON({
                url: $('form').attr('action'),
                data: {
                    SportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                    LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                    TeamList: $("#team-multi-select").fanexMultiSelect('getItems', true).join(',')
                }
            }, function (data) {
                gridOptions.bodyRows = totalBets.outright.genGridRows(data);
                $('#BetListGrid').show().fanexGrid(gridOptions);
                refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
            });
        },
        genGridRows: function (list) {
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
                            { Name: "Team", Value: totalBets.outright.generateBetListLink(league.LeagueId, match.TeamId, match.TeamName), CssClass: "bold" },
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
        },
        generateBetListLink: function (leagueId, teamId, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"totalBets.outright.showBetList(";
            linkHtmlString += leagueId + "," + teamId + ")\">";
            linkHtmlString += linkvalue + "</span>";
            return linkHtmlString;
        },
        showBetList: function (leagueId, teamId) {
            refreshCountDownMethods.stopCountDown();
            $('form').hide();
            $("#totalbets-back").attr("class", "history-back");

            core.ajaxJSON({
                url: core.resolveUrl("~/Totalbets/Outright/OutrightBetList"),
                data: {
                    LeagueId: leagueId,
                    TeamId: teamId
                }
            }, function (data) {
                fanex.betlist.core.reRenderAfterPagedSuccessfully(betListGridOptions, data);
            });
        }
    };
} (jQuery));