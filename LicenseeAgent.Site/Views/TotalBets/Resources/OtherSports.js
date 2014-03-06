var gridOptions = {
    columns: [
                { HeaderText: "#", Width: 25, Name: "Index", CellAlign: "center" },
                { HeaderText: LANGUAGE.getString("Member"), Width: 95, Name: "Member", HeaderAlign: "center", CellAlign: "center", CanResize: true },
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
    gridExpandHeight: 100,
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
                checkAllText: 'All sports',
                noItemText: 'No sport seleted.',
                noneSelectedText: 'Select sport',
                selectedText: '# selected sports',
                tooltipNoItem: 'There is no sport selected.',
                useCustomCheckAllText: true,
                customCheckAllText: 'All sports',
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
                checkAllText: 'All leagues',
                noItemText: 'No league seleted.',
                noneSelectedText: 'Select league',
                selectedText: '# selected leagues',
                tooltipNoItem: 'There is no league selected.',
                useCustomCheckAllText: true,
                customCheckAllText: 'All leagues',
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

        selectedLeague: function () {
            var selectedLeagues = $("#league-multi-select").fanexMultiSelect('getItems', true).join(',');
            OtherSports.getEventDataByLeague();
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
                            checkAllText: 'All events',
                            noItemText: 'No event seleted.',
                            noneSelectedText: 'Select event',
                            selectedText: '# selected events',
                            tooltipNoItem: 'There is no event selected.',
                            useCustomCheckAllText: true,
                            customCheckAllText: 'All events',
                            showParentSelectedText: false,
                            onLeave: function () {
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
                        gridOption.bodyRows = OtherSports.genGridRows(data.List, gridOption);
                        $('#BetListGrid').show().fanexGrid(gridOption);
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
                        rowIndex++;
                        //Home team
                        homeTeam.push(
                            { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>", MergeRowNumber: 3 },
                            { Name: "Time", Value: match.Time, MergeRowNumber: 3, CssClass: "bold" },
                            { Name: "Event", Value: match.HomeName, TdClass: "team-name" },
                            { Name: "MoneyLine", Value: OtherSports.generateLinkValue(match.MatchId, 20, 'h', match.HomeFulltimeMoneyLine) },
                            { Name: "FTHDP", Value: OtherSports.generateLinkValue(match.MatchId, 1, 'h', match.HomeFulltimeHandicap) },
                            { Name: "StatisticsHDP", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FTOU", Value: OtherSports.generateLinkValue(match.MatchId, 3, 'h', match.HomeFulltimeOverUnder) },
                            { Name: "StatisticsOU", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FT1X2", Value: OtherSports.generateLinkValue(match.MatchId, 5, '1', match.HomeFulltimeOneXTwo) },
                            { Name: "Statistics1X2", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FTOE", Value: OtherSports.generateLinkValue(match.MatchId, 2, 'h', match.HomeFulltimeOddEven) },
                            { Name: "FHHDP", Value: OtherSports.generateLinkValue(match.MatchId, 7, 'h', match.HomeFirstHalfHandicap) },
                            { Name: "StatisticsFHHDP", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FHOU", Value: OtherSports.generateLinkValue(match.MatchId, 8, 'h', match.HomeFirstHalfOverUnder) },
                            { Name: "StatisticsFHOU", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" }
                        );

                        var awayTeam = [];
                        //Away team
                        awayTeam.push(
                            { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                            { Name: "MoneyLine", Value: OtherSports.generateLinkValue(match.MatchId, 20, 'a', match.AwayFulltimeMoneyLine) },
                            { Name: "FTHDP", Value: OtherSports.generateLinkValue(match.MatchId, 1, 'a', match.AwayFulltimeHandicap) },
                            { Name: "FTOU", Value: OtherSports.generateLinkValue(match.MatchId, 3, 'a', match.AwayFulltimeOverUnder) },
                            { Name: "FT1X2", Value: OtherSports.generateLinkValue(match.MatchId, 5, '2', match.AwayFulltimeOneXTwo) },
                            { Name: "FTOE", Value: OtherSports.generateLinkValue(match.MatchId, 2, 'a', match.AwayFulltimeOddEven) },
                            { Name: "FHHDP", Value: OtherSports.generateLinkValue(match.MatchId, 7, 'a', match.AwayFirstHalfHandicap) },
                            { Name: "FHOU", Value: OtherSports.generateLinkValue(match.MatchId, 8, 'a', match.AwayFirstHalfOverUnder) }
                        );

                        var drawRow = [];
                        drawRow.push(
                            { Name: "Event", Value: "Draw", TdClass: "draw" },
                            { Name: "MoneyLine", Value: "", MergeColNumber: 5 },
                            { Name: "FT1X2", Value: OtherSports.generateLinkValue(match.MatchId, 5, 'x', match.DrawFulltimeOneXTwo) },
                            { Name: "Statistics1X2", Value: "", MergeColNumber: 6 }
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
        showBetList: function (matchId, bettype, betteam) {
            refreshCountDownMethods.stopCountDown();
            $("#totalbets-filter").attr("class", "totalbets-hide");
            $("#totalbets-back").attr("class", "history-link");

            var params = {
                MatchId: matchId,
                Bettype: bettype,
                Betteam: betteam
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

                    fanex.betlist.core.reRenderAfterPagedSuccessfully(data, [10]);
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        generateLinkValue: function (matchId, bettype, betteam, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"OtherSports.showBetList(";
            linkHtmlString += matchId + ",'" + bettype + "','" + betteam + "')\">";
            linkHtmlString += linkvalue + "</span>";

            return linkHtmlString;
        },
        submitForm: function () {
            refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
            $("#totalbets-filter").attr("class", "");
            $("#totalbets-back").attr("class", "totalbets-hide");
            $('form').submit();
        }
    }
} (jQuery);
$(function () {
    OtherSports.init();
});