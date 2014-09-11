/*global refreshCountDownMethods, core, language, pageContext*/


var totalBets = totalBets || {};

totalBets.outright = (function ($) {
    'use strict'; 
    var gridOptions = {
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
        gridExpandHeight: 60,
        bodyRowHeight: 29,
        autoResizeColumn: true
    };

    var reportGridOptions = {
        columns: [
            { HeaderText: "#", Width: 40, Name: "RowID", CanResize: false },
            { HeaderText: language.getString("Team"), Width: 520, Name: "Team", HeaderAlign: "center", CellAlign: "left", CanResize: false },
            { HeaderText: language.getString("BetCount"), Width: 100, Name: "BetCount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0." },
            { HeaderText: language.getString("Balance"), Width: 150, Name: "Balance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00" },
            { HeaderText: language.getString("Forecast"), Width: 150, Name: "Forecast", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", 
                FormatPositive: '$' }
        ],
        gridExpandHeight: 125,
        indexNumberColumn: 1,
        bodyRowHeight: 26,
        autoResizeColumn: true
    };

    return {
        checkedAllSport: true,
        checkedAllLeague: true,
        checkedAllEvent: true,
        init: function () {
            var hiddenColumns = [];
            if (pageContext.LegendType === 1) {
                hiddenColumns.push(9);
            }
            if (pageContext.IsLastLevel) {
                hiddenColumns.push(10);
            }
            gridOptions.hiddenColumns = hiddenColumns;

            totalBets.outright.initData();

            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: core.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            $('form').submit(function () {
                totalBets.outright.getSportData();
                totalBets.outright.getLeagueDataBySport();
                totalBets.outright.getTeamDataByLeague();

                totalBets.outright.reRenderAfterPaged();
                refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                return false;
            });
        },

        initData: function () {
            totalBets.outright.getSportData();
            totalBets.outright.getLeagueDataBySport();
            totalBets.outright.getTeamDataByLeague(true);
        },

        // ------------------------------------------------ Sport filter -----------------------------------------
        getSportData: function () {
            var params = {
                Sport: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: totalBets.outright.checkedAllSport
            };
            $.ajax({
                url: core.resolveUrl("~/TotalBets/Outright/GetSportList"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    totalBets.outright.initSportMultiSelect(data.List);
                },
                complete: function () {
                }
            });
        },

        initSportMultiSelect: function (sportList) {
            $("#sport-multi-select").fanexMultiSelect({
                listData: sportList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 150,
                checkAllText: language.getString('AllSports'),
                noItemText: language.getString('NoSelectedSport'),
                noneSelectedText: language.getString('PleaseSelectSport'),
                selectedText: language.getString('NumberOfSelectedSports'),
                tooltipNoItem: language.getString('NoSelectedSport'),
                useCustomCheckAllText: true,
                customCheckAllText: language.getString('AllSports'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    totalBets.outright.getLeagueDataBySport();
                    totalBets.outright.getTeamDataByLeague();
                },
                onFooterButtonClick: function () {
                    totalBets.outright.getLeagueDataBySport();
                    totalBets.outright.getTeamDataByLeague();
                },
                onCheck: function () {
                    totalBets.outright.checkedAllSport = false;
                },
                onCheckAll: function () {
                    totalBets.outright.checkedAllSport = true;
                }
            });
        },

        // ------------------------------------------------ League filter -----------------------------------------
        getLeagueDataBySport: function () {
            var params = {
                Sport: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: totalBets.outright.checkedAllLeague
            };
            $.ajax({
                url: core.resolveUrl("~/TotalBets/Outright/GetLeaguesBySports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    //core.blockUI();
                },
                success: function (data) {
                    totalBets.outright.initLeagueMultiSelect(data.List);
                },
                complete: function () {
                    //core.unblockUI();
                }
            });
        },

        initLeagueMultiSelect: function (leagueList) {
            $("#league-multi-select").fanexMultiSelect({
                listData: leagueList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 270,
                checkAllText: language.getString('AllLeagues'),
                noItemText: language.getString('NoSelectedLeague'),
                noneSelectedText: language.getString('PleaseSelectLeague'),
                selectedText: language.getString('NumberOfSelectedLeagues'),
                tooltipNoItem: language.getString('NoSelectedLeague'),
                useCustomCheckAllText: true,
                customCheckAllText: language.getString('AllLeagues'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    totalBets.outright.getTeamDataByLeague();
                },
                onFooterButtonClick: function () {
                    totalBets.outright.getTeamDataByLeague();
                },
                onCheck: function () {
                    totalBets.outright.checkedAllLeague = false;
                },
                onCheckAll: function () {
                    totalBets.outright.checkedAllLeague = true;
                }
            });
        },

        // ------------------------------------------------ Team filter -----------------------------------------
        getTeamDataByLeague: function (isLoadGrid) {
            var params = {
                Sport: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                Team: $("#team-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: totalBets.outright.checkedAllEvent
            };
            $.ajax({
                url: core.resolveUrl("~/TotalBets/Outright/GetTeamsByLeagues"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    totalBets.outright.initEventMultiSelect(data.List);

                    // Load data for the first time load page
                    if (isLoadGrid) {
                        totalBets.outright.reRenderAfterPaged();
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },

        initEventMultiSelect: function (eventList) {
            $("#team-multi-select").fanexMultiSelect({
                listData: eventList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 250,
                checkAllText: language.getString('AllTeams'),
                noItemText: language.getString('NoSelectedTeam'),
                noneSelectedText: language.getString('PleaseSelectTeam'),
                selectedText: language.getString('NumberOfSelectedTeams'),
                tooltipNoItem: language.getString('NoSelectedTeam'),
                useCustomCheckAllText: true,
                customCheckAllText: language.getString('AllTeams'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    //totalBets.outright.selectedEvent();
                },
                onFooterButtonClick: function () {
                    //totalBets.outright.selectedEvent();
                },
                onCheck: function () {
                    totalBets.outright.checkedAllEvent = false;
                },
                onCheckAll: function () {
                    totalBets.outright.checkedAllEvent = true;
                }
            });
        },

        // ------------------------------------------------ Refresh data -----------------------------------------
        reRenderAfterPaged: function () {
            var params = {
                SportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                TeamList: $("#team-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/Outright/GetOutrightData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    reportGridOptions.bodyRows = totalBets.outright.genGridRows(data.List);
                    $('#BetListGrid').show().fanexGrid(reportGridOptions);
                },
                complete: function () {
                    core.unblockUI();
                }
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
                            { Name: "Team", Value: totalBets.outright.generateLinkValue(league.LeagueId, match.TeamId, match.TeamName), CssClass: "bold" },
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
        showBetList: function (leagueId, teamId) {
            refreshCountDownMethods.stopCountDown();
            $("#totalbets-filter").attr("class", "hide");
            $("#totalbets-back").attr("class", "history-back");

            var params = {
                LeagueId: leagueId,
                TeamId: teamId
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/Outright/GetOutrightBetList"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        alert(data.ErrMsg);
                        return;
                    }

                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data);
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },
        generateLinkValue: function (leagueId, teamId, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"totalBets.outright.showBetList(";
            linkHtmlString += leagueId + "," + teamId + ")\">";
            linkHtmlString += linkvalue + "</span>";

            return linkHtmlString;
        },
        submitForm: function () {
            refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
            $("#totalbets-filter").attr("class", "");
            $("#totalbets-back").attr("class", "history-back hide");
            $('form').submit();
        }
    };
} (jQuery));