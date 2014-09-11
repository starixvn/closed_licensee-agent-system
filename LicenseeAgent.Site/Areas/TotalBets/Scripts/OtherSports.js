/*global refreshCountDownMethods, core, language, pageContext*/

var totalBets = totalBets || {};

totalBets.otherSports = (function ($) {
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
        gridExpandHeight: 125,
        indexNumberColumn: 1,
        bodyRowHeight: 26,
        autoResizeColumn: true
    };

    return {
        //sportType: 2, //Basketball sporttype
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

            totalBets.otherSports.initData();

            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: core.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            // search
            $('form').submit(function () {
                totalBets.otherSports.getSportData();
                totalBets.otherSports.getLeagueData();
                totalBets.otherSports.getEventDataByLeague();
                totalBets.otherSports.reRenderAfterPaged();
                refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                return false;
            });
        },

        initData: function () {
            core.blockUI(); //Block ui and unblock when all data is loaded
            totalBets.otherSports.getSportData();
            totalBets.otherSports.getLeagueData();
            totalBets.otherSports.getEventDataByLeague(true);
        },

        getSportData: function () {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: totalBets.otherSports.checkedAllSport
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/OtherSports/GetSportListOtherSports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        totalBets.otherSports.initSportMultiSelect(data.List);
                    }
                    else {
                        alert(data.ErrMsg);
                    }
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
                    totalBets.otherSports.getLeagueData();
                    totalBets.otherSports.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    totalBets.otherSports.getLeagueData();
                    totalBets.otherSports.getEventDataByLeague();
                },
                onCheck: function () {
                    totalBets.otherSports.checkedAllSport = false;
                },
                onCheckAll: function () {
                    totalBets.otherSports.checkedAllSport = true;
                }
            });
        },

        getLeagueData: function () {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                leagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: totalBets.otherSports.checkedAllLeague
            };
            $.ajax({
                url: core.resolveUrl("~/Totalbets/OtherSports/GetLeagueListOtherSports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        totalBets.otherSports.initLeagueMultiSelect(data.List);
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                }
            });
        },

        initLeagueMultiSelect: function (leagueList) {
            $("#league-multi-select").fanexMultiSelect({
                listData: leagueList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 250,
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
                    totalBets.otherSports.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    totalBets.otherSports.getEventDataByLeague();
                },
                onCheck: function () {
                    totalBets.otherSports.checkedAllLeague = false;
                },
                onCheckAll: function () {
                    totalBets.otherSports.checkedAllLeague = true;
                }
            });
        },

        getEventDataByLeague: function (isLoadGrid) {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                leagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                eventList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: totalBets.otherSports.checkedAllEvent
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/OtherSports/GetEventsByLeaguesOtherSports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        $("#event-multi-select").fanexMultiSelect({
                            listData: data.List,
                            showTooltip: true,
                            maxHeight: 500,
                            minWidth: 250,
                            checkAllText: language.getString('AllEvents'),
                            noItemText: language.getString('NoSelectedEvent'),
                            noneSelectedText: language.getString('PleaseSelectEvent'),
                            selectedText: language.getString('NumberOfSelectedEvents'),
                            tooltipNoItem: language.getString('NoSelectedEvent'),
                            useCustomCheckAllText: true,
                            customCheckAllText: language.getString('AllEvents'),
                            showParentSelectedText: false,
                            showFooter: true,
                            footerButtonText: 'OK',
                            onLeave: function () {
                                //totalBets.otherSports.reRenderAfterPaged();
                            },
                            onFooterButtonClick: function () {
                                //totalBets.otherSports.reRenderAfterPaged();
                            },
                            onCheck: function () {
                                totalBets.otherSports.checkedAllEvent = false;
                            },
                            onCheckAll: function () {
                                totalBets.otherSports.checkedAllEvent = true;
                            }
                        });
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    //Load data for the first time load page
                    if (isLoadGrid) {
                        totalBets.otherSports.reRenderAfterPaged();
                    }
                }
            });
        },

        reRenderAfterPaged: function () {
            var params = {
                SportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/OtherSports/GetOtherSportsData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode === 0) {
                        reportGridOptions.bodyRows = totalBets.otherSports.genGridRows(data.List);
                        $('#BetListGrid').show().fanexGrid(reportGridOptions);
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },

        genGridRows: function (list) {
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
                            { Name: "RowID", Value: "<div align='left'>" + sport.SportName + "</div>", MergeColNumber: 15
                            }
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
                                { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 15
                                }
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
                        { Name: "MoneyLine", Value: totalBets.otherSports.generateLink(match.MatchId, 20, 'h', showLiveTicket, match.HomeFulltimeMoneyLine) },
                        { Name: "FTHDP", Value: totalBets.otherSports.generateLink(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                        { Name: "FTOU", Value: totalBets.otherSports.generateLink(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                        { Name: "FT1X2", Value: totalBets.otherSports.generateLink(match.MatchId, 5, '1', showLiveTicket, match.HomeFulltimeOneXTwo) },
                        { Name: "FTOE", Value: totalBets.otherSports.generateLink(match.MatchId, 2, 'h', showLiveTicket, match.HomeFulltimeOddEven) },
                        { Name: "FHHDP", Value: totalBets.otherSports.generateLink(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                        { Name: "FHOU", Value: totalBets.otherSports.generateLink(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) }
                        );

                        var awayTeam = [];
                        //Away team
                        awayTeam.push(
                        { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                        { Name: "MoneyLine", Value: totalBets.otherSports.generateLink(match.MatchId, 20, 'a', showLiveTicket, match.AwayFulltimeMoneyLine) },
                        { Name: "FTHDP", Value: totalBets.otherSports.generateLink(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                        { Name: "FTOU", Value: totalBets.otherSports.generateLink(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                        { Name: "FT1X2", Value: totalBets.otherSports.generateLink(match.MatchId, 5, '2', showLiveTicket, match.AwayFulltimeOneXTwo) },
                        { Name: "FTOE", Value: totalBets.otherSports.generateLink(match.MatchId, 2, 'a', showLiveTicket, match.AwayFulltimeOddEven) },
                        { Name: "FHHDP", Value: totalBets.otherSports.generateLink(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                        { Name: "FHOU", Value: totalBets.otherSports.generateLink(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) }
                        );

                        var drawRow = [];
                        drawRow.push(
                        { Name: "Event", Value: language.getString("Draw"), TdClass: "draw" },
                        { Name: "MoneyLine", Value: "", MergeColNumber: 3 },
                        { Name: "FT1X2", Value: totalBets.otherSports.generateLink(match.MatchId, 5, 'x', showLiveTicket, match.DrawFulltimeOneXTwo) },
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
        },
        showBetList: function (matchId, bettype, betteam, showliveticket) {
            refreshCountDownMethods.stopCountDown();
            $("#totalbets-filter").attr("class", "hide");
            $("#totalbets-back").attr("class", "history-back");

            var params = {
                MatchId: matchId,
                Bettype: bettype,
                Betteam: betteam,
                ShowLiveTicket: showliveticket
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/OtherSports/GetOtherSportsBetList"),
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
        generateLink: function (matchId, bettype, betteam, showliveticket, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"totalBets.otherSports.showBetList(";
            linkHtmlString += matchId + ",'" + bettype + "','" + betteam + "'," + showliveticket + ")\">";
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