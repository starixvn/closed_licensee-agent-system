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

var Basketball = function ($) {
    return {
        sportType: 2, //Basketball sporttype
        init: function () {
            Basketball.initData();

            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: CORE.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            // search
            $('form').submit(function () {
                Basketball.reRenderAfterPaged();
                refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                return false;
            });
        },

        initData: function () {
            CORE.blockUI();
            Basketball.getLeagueData();
            Basketball.getEventDataByLeague(true);
        },

        getLeagueData: function () {
            var params = {
                League: $("#League").val(),
                sportType: Basketball.sportType
            };
            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetLeagueList"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        Basketball.initLeagueMultiSelect(data.List);
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
                minWidth: 300,
                checkAllText: 'All leagues',
                noItemText: 'No league seleted.',
                noneSelectedText: 'Select league',
                selectedText: '# selected leagues',
                tooltipNoItem: 'There is no league selected.',
                showParentSelectedText: false,
                useCustomCheckAllText: true,
                customCheckAllText: 'All leagues',
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    Basketball.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    Basketball.getEventDataByLeague();
                }
            });
        },

        selectedLeague: function () {
            var selectedLeagues = $("#league-multi-select").fanexMultiSelect('getItems', true).join(',');
            Basketball.getEventDataByLeague();
        },

        getEventDataByLeague: function (isLoadGrid) {
            var params = {
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                Event: $("#event-multi-select").fanexMultiSelect('getItems', true).join(','),
                sportType: Basketball.sportType
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetEventsByLeagues"),
                type: 'post',
                data: params,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        Basketball.initEventMultiSelect(data.List);
                    }
                    else {
                        alert(data.ErrMsg);
                    }
                },
                complete: function () {
                    //Load data for the first time load page
                    if (isLoadGrid)
                        Basketball.reRenderAfterPaged();
                }
            });
        },

        initEventMultiSelect: function (eventList) {
            $("#event-multi-select").fanexMultiSelect({
                listData: eventList,
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
                    //Basketball.reRenderAfterPaged();
                }

            });
        },

        reRenderAfterPaged: function () {
            var params = {
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetBasketballData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode == 0) {
                        gridOption.bodyRows = Basketball.genGridRows(data.List, gridOption);
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
            var len = list.length;
            var bodyRows = [];
            var oldLeagueId = 0;
            var rowIndex = 0;

            for (var i = 0; i < len; i++) {//League
                var league = list[i];
                var newLeagueId = league.LeagueId;

                if (oldLeagueId != newLeagueId) {
                    rowIndex = 0;
                    oldLeagueId = newLeagueId;
                    //Push League row
                    bodyRows.push({
                        Columns: ([
                            { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 13
                            }
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
                    rowIndex++;
                    //Home team
                    homeTeam.push(
                        { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>", MergeRowNumber: 2 },
                        { Name: "Time", Value: match.Time, MergeRowNumber: 2, CssClass: "bold" },
                        { Name: "Event", Value: match.HomeName, TdClass: "team-name" },
                        { Name: "FTHDP", Value: Basketball.generateLinkValue(match.MatchId, 1, 'h', match.HomeFulltimeHandicap) },
                        { Name: "StatisticsHDP", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                        { Name: "FTOU", Value: Basketball.generateLinkValue(match.MatchId, 3, 'h', match.HomeFulltimeOverUnder) },
                        { Name: "StatisticsOU", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                        { Name: "FTOE", Value: Basketball.generateLinkValue(match.MatchId, 2, 'h', match.HomeFulltimeOddEven) },
                        { Name: "FHHDP", Value: Basketball.generateLinkValue(match.MatchId, 7, 'h', match.HomeFirstHalfHandicap) },
                        { Name: "StatisticsFHHDP", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                        { Name: "FHOU", Value: Basketball.generateLinkValue(match.MatchId, 8, 'h', match.HomeFirstHalfOverUnder) },
                        { Name: "StatisticsFHOU", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                        { Name: "FHOE", Value: Basketball.generateLinkValue(match.MatchId, 12, 'h', match.HomeFirstHalfOddEven) }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                        { Name: "FTHDP", Value: Basketball.generateLinkValue(match.MatchId, 1, 'a', match.AwayFulltimeHandicap) },
                        { Name: "FTOU", Value: Basketball.generateLinkValue(match.MatchId, 3, 'h', match.AwayFulltimeOverUnder) },
                        { Name: "FTOE", Value: Basketball.generateLinkValue(match.MatchId, 2, 'h', match.AwayFulltimeOddEven) },
                        { Name: "FHHDP", Value: Basketball.generateLinkValue(match.MatchId, 7, 'h', match.AwayFirstHalfHandicap) },
                        { Name: "FHOU", Value: Basketball.generateLinkValue(match.MatchId, 8, 'h', match.AwayFirstHalfOverUnder) },
                        { Name: "FHOE", Value: Basketball.generateLinkValue(match.MatchId, 12, 'h', match.AwayFirstHalfOddEven) }
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
                url: CORE.resolveUrl("~/Totalbets/GetBasketballBetList"),
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
            var linkHtmlString = "<a href=\"javascript:void(0);\" onclick=\"Basketball.showBetList(";
            linkHtmlString += matchId + ",'" + bettype + "','" + betteam + "')\">";
            linkHtmlString += linkvalue + "</a>";

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
    Basketball.init();
});