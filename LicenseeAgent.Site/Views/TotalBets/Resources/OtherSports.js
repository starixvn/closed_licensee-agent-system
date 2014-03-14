var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "left", CanResize: true },
                { HeaderText: LANGUAGE.getString("TransTime"), Width: 100, Name: "TransTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Choice"), Width: 345, Name: "Choice", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Odds"), Width: 60, Name: "Odds", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Stake"), Width: 70, Name: "Stake", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("WinLoss"), Width: 60, Name: "WinLoss", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Status"), Width: 90, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("AgentPT"), Width: 60, Name: "AgentPT", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("AgentComm"), Width: 75, Name: "AgentComm", HeaderAlign: "center", CellAlign: "right" },
                { HeaderText: LANGUAGE.getString("Feature"), Width: 55, Name: "Feature", HeaderAlign: "center", CellAlign: "center" }
            ],
    showPager: true,
    gridExpandHeight: 60,
    bodyRowHeight: 29,
    autoResizeColumn: true
};

var OtherSports = function ($) {
    return {
        //sportType: 2, //Basketball sporttype
        checkedAllSport: true,
        checkedAllLeague: true,
        checkedAllEvent: true,
        init: function () {
            OtherSports.initData();

            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: CORE.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            // search
            $('form').submit(function () {
                OtherSports.getSportData();
                OtherSports.getLeagueData();
                OtherSports.getEventDataByLeague();
                OtherSports.reRenderAfterPaged();
                refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                return false;
            });
        },

        initData: function () {
            CORE.blockUI(); //Block ui and unblock when all data is loaded
            OtherSports.getSportData();
            OtherSports.getLeagueData();
            OtherSports.getEventDataByLeague(true);
        },

        getSportData: function () {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: OtherSports.checkedAllSport
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetSportListOtherSports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        OtherSports.initSportMultiSelect(data.List);
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
                checkAllText: LANGUAGE.getString('AllSports'),
                noItemText: LANGUAGE.getString('NoSelectedSport'),
                noneSelectedText: LANGUAGE.getString('PleaseSelectSport'),
                selectedText: LANGUAGE.getString('NumberOfSelectedSports'),
                tooltipNoItem: LANGUAGE.getString('NoSelectedSport'),
                useCustomCheckAllText: true,
                customCheckAllText: LANGUAGE.getString('AllSports'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    OtherSports.getLeagueData();
                    OtherSports.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    OtherSports.getLeagueData();
                    OtherSports.getEventDataByLeague();
                },
                onCheck: function () {
                    OtherSports.checkedAllSport = false;
                },
                onCheckAll: function () {
                    OtherSports.checkedAllSport = true;
                }
            });
        },

        getLeagueData: function () {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                leagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: OtherSports.checkedAllLeague
            };
            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetLeagueListOtherSports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        OtherSports.initLeagueMultiSelect(data.List);
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
                checkAllText: LANGUAGE.getString('AllLeagues'),
                noItemText: LANGUAGE.getString('NoSelectedLeague'),
                noneSelectedText: LANGUAGE.getString('PleaseSelectLeague'),
                selectedText: LANGUAGE.getString('NumberOfSelectedLeagues'),
                tooltipNoItem: LANGUAGE.getString('NoSelectedLeague'),
                useCustomCheckAllText: true,
                customCheckAllText: LANGUAGE.getString('AllLeagues'),
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    OtherSports.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    OtherSports.getEventDataByLeague();
                },
                onCheck: function () {
                    OtherSports.checkedAllLeague = false;
                },
                onCheckAll: function () {
                    OtherSports.checkedAllLeague = true;
                }
            });
        },

        getEventDataByLeague: function (isLoadGrid) {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                leagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                eventList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: OtherSports.checkedAllEvent
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetEventsByLeaguesOtherSports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        $("#event-multi-select").fanexMultiSelect({
                            listData: data.List,
                            showTooltip: true,
                            maxHeight: 500,
                            minWidth: 250,
                            checkAllText: LANGUAGE.getString('AllEvents'),
                            noItemText: LANGUAGE.getString('NoSelectedEvent'),
                            noneSelectedText: LANGUAGE.getString('PleaseSelectEvent'),
                            selectedText: LANGUAGE.getString('NumberOfSelectedEvents'),
                            tooltipNoItem: LANGUAGE.getString('NoSelectedEvent'),
                            useCustomCheckAllText: true,
                            customCheckAllText: LANGUAGE.getString('AllEvents'),
                            showParentSelectedText: false,
                            onLeave: function () {
                                //OtherSports.reRenderAfterPaged();
                            },
                            onFooterButtonClick: function () {
                                OtherSports.reRenderAfterPaged();
                            },
                            onCheck: function () {
                                OtherSports.checkedAllEvent = false;
                            },
                            onCheckAll: function () {
                                OtherSports.checkedAllEvent = true;
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
                        OtherSports.reRenderAfterPaged();
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
                url: CORE.resolveUrl("~/Totalbets/GetOtherSportsData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        reportGridOptions.bodyRows = OtherSports.genGridRows(data.List, reportGridOptions);
                        $('#BetListGrid').show().fanexGrid(reportGridOptions);
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        genGridRows: function (list, options) {
            var columns = options.columns;
            var rowCssClass = options.rowCssClass; // custom option
            var rowHeight = options.RowHeight;
            var indexNumberColumn = options.indexNumberColumn; // custom option
            var lenSports = list.length;
            var bodyRows = [];
            var oldSportId = 0;
            var oldLeagueId = 0;
            var rowIndex = 0;

            for (var k = 0; k < lenSports; k++) {//Sport
                var sport = list[k];
                //Push Sport row
                if (oldSportId != sport.SportType) {
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
                    if (oldLeagueId != league.LeagueId) {
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
                            { Name: "MoneyLine", Value: OtherSports.generateLinkValue(match.MatchId, 20, 'h', showLiveTicket, match.HomeFulltimeMoneyLine) },
                            { Name: "FTHDP", Value: OtherSports.generateLinkValue(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                            { Name: "FTOU", Value: OtherSports.generateLinkValue(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                            { Name: "FT1X2", Value: OtherSports.generateLinkValue(match.MatchId, 5, '1', showLiveTicket, match.HomeFulltimeOneXTwo) },
                            { Name: "FTOE", Value: OtherSports.generateLinkValue(match.MatchId, 2, 'h', showLiveTicket, match.HomeFulltimeOddEven) },
                            { Name: "FHHDP", Value: OtherSports.generateLinkValue(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                            { Name: "FHOU", Value: OtherSports.generateLinkValue(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) }
                        );

                        var awayTeam = [];
                        //Away team
                        awayTeam.push(
                            { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                            { Name: "MoneyLine", Value: OtherSports.generateLinkValue(match.MatchId, 20, 'a', showLiveTicket, match.AwayFulltimeMoneyLine) },
                            { Name: "FTHDP", Value: OtherSports.generateLinkValue(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                            { Name: "FTOU", Value: OtherSports.generateLinkValue(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                            { Name: "FT1X2", Value: OtherSports.generateLinkValue(match.MatchId, 5, '2', showLiveTicket, match.AwayFulltimeOneXTwo) },
                            { Name: "FTOE", Value: OtherSports.generateLinkValue(match.MatchId, 2, 'a', showLiveTicket, match.AwayFulltimeOddEven) },
                            { Name: "FHHDP", Value: OtherSports.generateLinkValue(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                            { Name: "FHOU", Value: OtherSports.generateLinkValue(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) }
                        );

                        var drawRow = [];
                        drawRow.push(
                            { Name: "Event", Value: LANGUAGE.getString("Draw"), TdClass: "draw" },
                            { Name: "MoneyLine", Value: "", MergeColNumber: 3 },
                            { Name: "FT1X2", Value: OtherSports.generateLinkValue(match.MatchId, 5, 'x', showLiveTicket, match.DrawFulltimeOneXTwo) },
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
                url: CORE.resolveUrl("~/Totalbets/GetOtherSportsBetList"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode != 0) {
                        alert(data.ErrMsg);
                        return;
                    }

                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data, [10]);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        generateLinkValue: function (matchId, bettype, betteam, showliveticket, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"OtherSports.showBetList(";
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
    }
} (jQuery);
$(function () {
    OtherSports.init();
});