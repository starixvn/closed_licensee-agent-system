var Soccer = function ($) {
    return {
        init: function () {
            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: CORE.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            Soccer.initData();

            $('form').submit(function () {
                Soccer.reRenderAfterPaged();
                refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                return false;
            });
        },

        initData: function () {
            Soccer.getLeagueData();
            Soccer.getEventDataByLeague(true);
        },

        getLeagueData: function () {
            var params = {
                League: $("#League").val()
            };
            $.ajax({
                url: CORE.resolveUrl("~/TotalBets/GetLeagueList"),
                type: 'post',
                data: params,
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
                minWidth: 400,
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
                    Soccer.selectedLeague();
                },
                onFooterButtonClick: function () {
                    Soccer.selectedLeague();
                }
            });
        },

        selectedLeague: function () {
            var selectedLeagues = $("#league-multi-select").fanexMultiSelect('getItems', true).join(',');
            $("#League").val(selectedLeagues);
            Soccer.getEventDataByLeague();
        },

        getEventDataByLeague: function (isLoadGrid) {
            var params = {
                League: $("#League").val(),
                Event: $("#Event").val()
            };
            $.ajax({
                url: CORE.resolveUrl("~/TotalBets/GetEventsByLeagues"),
                type: 'post',
                data: params,
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
                minWidth: 350,
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
                onLeave: function () { Soccer.selectedEvent(); },
                onFooterButtonClick: function () {
                    Soccer.selectedEvent();
                }
            });
        },

        selectedEvent: function () {
            var selectedLeagues = $("#event-multi-select").fanexMultiSelect('getItems', true).join(',');
            $("#Event").val(selectedLeagues);
        },

        showAllBetType: function (context) {
            var matchId = $(context).attr("data-matchid");
            var matchName = $(context).attr("data-matchname");
            var url = CORE.resolveUrl("~/Totalbets/SoccerDetail") + "?matchId=" + matchId + "&matchName=" + matchName;
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
                    $('#grid').show().fanexGrid(gridOption);
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
                    rowIndex++;
                    //Home team
                    homeTeam.push(
                        { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>", MergeRowNumber: 3 },
                        { Name: "Time", Value: match.Time, MergeRowNumber: 3, CssClass: "bold" },
                        { Name: "Event", Value: match.HomeName, TdClass: "team-name" },
                        { Name: "FTHDP", Value: match.HomeFulltimeHandicap },
                        { Name: "FTOU", Value: match.HomeFulltimeOverUnder },
                        { Name: "FT1X2", Value: match.HomeFulltimeOneXTwo },
                        { Name: "FHHDP", Value: match.HomeFirstHalfHandicap },
                        { Name: "FHOU", Value: match.HomeFirstHalfOverUnder },
                        { Name: "FH1X2", Value: match.HomeFirstHalfOneXTwo }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                        { Name: "FTHDP", Value: match.AwayFulltimeHandicap },
                        { Name: "FTOU", Value: match.AwayFulltimeOverUnder },
                        { Name: "FT1X2", Value: match.AwayFulltimeOneXTwo },
                        { Name: "FHHDP", Value: match.AwayFirstHalfHandicap },
                        { Name: "FHOU", Value: match.AwayFirstHalfOverUnder },
                        { Name: "FH1X2", Value: match.AwayFirstHalfOneXTwo }
                    );

                    var moreLink = "<span class='more-link' onclick='Soccer.showAllBetType(this)' data-matchid='" + match.MatchId + "' data-matchname='" + match.HomeName + " -vs- " + match.AwayName + "'>More</span>";
                    var drawTeam = [];
                    drawTeam.push(
                        { Name: "Event", Value: "Draw", TdClass: "draw" },
                        { Name: "FTHDP", Value: moreLink, MergeColNumber: 2, CssClass: "c-align" },
                        { Name: "FT1X2", Value: match.DrawFulltimeOneXTwo },
                        { Name: "FHHDP", Value: "", MergeColNumber: 2 },
                        { Name: "FH1X2", Value: match.DrawFirstHalfOneXTwo }
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
        }
    }
} (jQuery);
$(function () {
    Soccer.init();
});