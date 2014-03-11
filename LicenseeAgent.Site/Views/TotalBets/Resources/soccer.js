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
    gridExpandHeight: 60,
    bodyRowHeight: 29,
    autoResizeColumn: true
};

var Soccer = function ($) {
    return {
        sportType: 1, //Basketball sporttype
        checkedAllLeague: true,
        checkedAllEvent: true,
        init: function () {
            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: CORE.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            Soccer.initData();

            $('form').submit(function () {
                Soccer.getLeagueData();
                Soccer.getEventDataByLeague();
                Soccer.reRenderAfterPaged();

                return false;
            });
        },

        initData: function () {
            Soccer.getLeagueData();
            Soccer.getEventDataByLeague(true);
        },

        getLeagueData: function () {
            var params = {
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                sportType: Soccer.sportType,
                checkedAll: Soccer.checkedAllLeague
            };
            $.ajax({
                url: CORE.resolveUrl("~/TotalBets/GetLeagueList"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    Soccer.initLeagueMultiSelect(data.List);
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
                checkAllText: 'All leagues.',
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
                    //Soccer.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    Soccer.getEventDataByLeague();
                },
                onCheck: function () {
                    Soccer.checkedAllLeague = false;
                },
                onCheckAll: function () {
                    Soccer.checkedAllLeague = true;
                }
            });
        },

        getEventDataByLeague: function (isLoadGrid) {
            var params = {
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                Event: $("#event-multi-select").fanexMultiSelect('getItems', true).join(','),
                sportType: Soccer.sportType,
                checkedAll: Soccer.checkedAllEvent
            };
            $.ajax({
                url: CORE.resolveUrl("~/TotalBets/GetEventsByLeagues"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    Soccer.initEventMultiSelect(data.List);

                    // Load data for the first time load page
                    if (isLoadGrid) {
                        Soccer.reRenderAfterPaged();
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        initEventMultiSelect: function (eventList) {
            $("#event-multi-select").fanexMultiSelect({
                listData: eventList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 300,
                checkAllText: 'All events.',
                noItemText: 'No event seleted.',
                noneSelectedText: 'Select event',
                selectedText: '# selected events',
                tooltipNoItem: 'There is no event selected.',
                useCustomCheckAllText: true,
                customCheckAllText: 'All events',
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    //Soccer.selectedEvent();
                },
                onFooterButtonClick: function () {
                    //Soccer.selectedEvent();
                },
                onCheck: function () {
                    Soccer.checkedAllEvent = false;
                },
                onCheckAll: function () {
                    Soccer.checkedAllEvent = true;
                }
            });
        },

        showAllBetType: function (context) {
            var matchId = $(context).attr("data-matchid");
            var matchName = $(context).attr("data-matchname");
            var league = $("#league-multi-select").fanexMultiSelect('getItems', true).join(',');
            var event = $("#event-multi-select").fanexMultiSelect('getItems', true).join(',');
            var timeInterval = $('#server-time-interval').val();

            var url = CORE.resolveUrl("~/Totalbets/SoccerDetail") + "?matchId=" + matchId + "&matchName=" + matchName + "&selectedLeague=" + league + "&selectedEvent=" + event + "&interval=" + timeInterval;
            window.location.href = url;
        },
        reRenderAfterPaged: function () {
            var params = {
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetSoccerData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    gridOption.bodyRows = Soccer.genGridRows(data.List, gridOption);
                    $('#BetListGrid').show().fanexGrid(gridOption);
                },
                complete: function () {
                    CORE.unblockUI();
                    refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
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
            var leagueId = 0;
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
                        { Name: "FTHDP", Value: Soccer.generateLinkValue(match.MatchId, 1, 'h', showLiveTicket, match.HomeFulltimeHandicap) },
                        { Name: "FTOU", Value: Soccer.generateLinkValue(match.MatchId, 3, 'h', showLiveTicket, match.HomeFulltimeOverUnder) },
                        { Name: "FT1X2", Value: Soccer.generateLinkValue(match.MatchId, 5, '1', showLiveTicket, match.HomeFulltimeOneXTwo) },
                        { Name: "FHHDP", Value: Soccer.generateLinkValue(match.MatchId, 7, 'h', showLiveTicket, match.HomeFirstHalfHandicap) },
                        { Name: "FHOU", Value: Soccer.generateLinkValue(match.MatchId, 8, 'h', showLiveTicket, match.HomeFirstHalfOverUnder) },
                        { Name: "FH1X2", Value: Soccer.generateLinkValue(match.MatchId, 15, '1', showLiveTicket, match.HomeFirstHalfOneXTwo) }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                        { Name: "FTHDP", Value: Soccer.generateLinkValue(match.MatchId, 1, 'a', showLiveTicket, match.AwayFulltimeHandicap) },
                        { Name: "FTOU", Value: Soccer.generateLinkValue(match.MatchId, 3, 'a', showLiveTicket, match.AwayFulltimeOverUnder) },
                        { Name: "FT1X2", Value: Soccer.generateLinkValue(match.MatchId, 5, '2', showLiveTicket, match.AwayFulltimeOneXTwo) },
                        { Name: "FHHDP", Value: Soccer.generateLinkValue(match.MatchId, 7, 'a', showLiveTicket, match.AwayFirstHalfHandicap) },
                        { Name: "FHOU", Value: Soccer.generateLinkValue(match.MatchId, 8, 'a', showLiveTicket, match.AwayFirstHalfOverUnder) },
                        { Name: "FH1X2", Value: Soccer.generateLinkValue(match.MatchId, 15, '2', showLiveTicket, match.AwayFirstHalfOneXTwo) }
                    );

                    var moreLink = "<span class='more-link' onclick='Soccer.showAllBetType(this)' data-matchid='" + match.MatchId + "' data-matchname='" + match.HomeName + " -vs- " + match.AwayName + "'>More</span>";
                    var drawTeam = [];
                    drawTeam.push(
                        { Name: "Event", Value: "Draw", TdClass: "draw" },
                        { Name: "FTHDP", Value: moreLink, MergeColNumber: 2, CssClass: "c-align" },
                        { Name: "FT1X2", Value: Soccer.generateLinkValue(match.MatchId, 5, 'x', showLiveTicket, match.DrawFulltimeOneXTwo) },
                        { Name: "FHHDP", Value: "", MergeColNumber: 2 },
                        { Name: "FH1X2", Value: Soccer.generateLinkValue(match.MatchId, 15, 'x', showLiveTicket, match.DrawFirstHalfOneXTwo) }
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
                url: CORE.resolveUrl("~/Totalbets/GetSoccerBetList"),
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
        generateLinkValue: function (matchId, bettype, betteam, showliveticket, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"Soccer.showBetList(";
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
    Soccer.init();
});