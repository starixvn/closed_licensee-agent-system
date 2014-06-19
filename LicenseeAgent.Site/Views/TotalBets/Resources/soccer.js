/*global refreshCountDownMethods, reportGridOptions, core, language*/

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
    gridExpandHeight: 60,
    bodyRowHeight: 29,
    autoResizeColumn: true
};

var Soccer = (function ($) {
    'use strict';
    return {
        sportType: 1, //Basketball sporttype
        checkedAllLeague: true,
        checkedAllEvent: true,
        init: function () {
            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: core.TimeInterval,
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
            if ($('#SelectedLeague').val() !== '' || $('#SelectedEvent').val() !== '') {
                Soccer.checkedAllLeague = $('#CheckAllLeague').val();
                Soccer.checkedAllEvent = $('#CheckAllEvent').val();
            }
            Soccer.getLeagueData();
            Soccer.getEventDataByLeague(true);
        },

        getLeagueData: function () {
            var params = {
                League: $("#SelectedLeague").val(),
                sportType: Soccer.sportType,
                checkedAll: Soccer.checkedAllLeague
            };
            $.ajax({
                url: core.resolveUrl("~/TotalBets/GetLeagueList"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    core.blockUI();
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
                    $("#SelectedLeague").val($("#league-multi-select").fanexMultiSelect('getItems', true).join(','));
                    Soccer.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    $("#SelectedLeague").val($("#league-multi-select").fanexMultiSelect('getItems', true).join(','));
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
                Event: $("#SelectedEvent").val(),
                sportType: Soccer.sportType,
                checkedAll: Soccer.checkedAllEvent
            };
            $.ajax({
                url: core.resolveUrl("~/TotalBets/GetEventsByLeagues"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    Soccer.initEventMultiSelect(data.List);

                    // Load data for the first time load page
                    if (isLoadGrid) {
                        Soccer.reRenderAfterPaged();
                    }
                },
                complete: function () {
                    core.unblockUI();
                }
            });
        },

        initEventMultiSelect: function (eventList) {
            $("#event-multi-select").fanexMultiSelect({
                listData: eventList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 300,
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
                    $("#SelectedEvent").val($("#event-multi-select").fanexMultiSelect('getItems', true).join(','));
                    //Soccer.selectedEvent();
                },
                onFooterButtonClick: function () {
                    $("#SelectedEvent").val($("#event-multi-select").fanexMultiSelect('getItems', true).join(','));
                    Soccer.reRenderAfterPaged();
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

            var url = core.resolveUrl("~/Totalbets/SoccerDetail") + "?matchId=" + matchId + "&matchName=" + matchName
                    + "&selectedLeague=" + league + "&selectedEvent=" + event + "&timeInterval=" + timeInterval + "&checkAllLeague=" + Soccer.checkedAllLeague
                    + "&checkAllEvent=" + Soccer.checkedAllEvent;
            window.location.href = url;
        },
        reRenderAfterPaged: function () {
            var params = {
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                MatchList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: core.resolveUrl("~/Totalbets/GetSoccerData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    core.blockUI();
                },
                success: function (data) {
                    reportGridOptions.bodyRows = Soccer.genGridRows(data.List);
                    $('#BetListGrid').show().fanexGrid(reportGridOptions);
                },
                complete: function () {
                    core.unblockUI();
                    refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                }
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

                    var moreLink = "<span class='more-link' onclick='Soccer.showAllBetType(this)' data-matchid='" + match.MatchId + "' data-matchname='"
                                + match.HomeName + " -vs- " + match.AwayName + "'>" + language.getString("More") + "</span>";
                    var drawTeam = [];
                    drawTeam.push(
                        { Name: "Event", Value: language.getString("Draw"), TdClass: "draw" },
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
                url: core.resolveUrl("~/Totalbets/GetSoccerBetList"),
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

                    fanex.betlist.core.reRenderAfterPagedSuccessfully(gridOptions, data, [10]);
                },
                complete: function () {
                    core.unblockUI();
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
    };
} (jQuery));

$(function () {
    'use strict';
    Soccer.init();
});