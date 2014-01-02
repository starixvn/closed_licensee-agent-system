var OtherSports = function ($) {
    return {
        //sportType: 2, //Basketball sporttype
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
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetSportListOtherSports"),
                type: 'post',
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.MsgId == 0) {
                        OtherSports.initSportMultiSelect(data.List);
                    }
                    else {
                        alert(data.MsgContent);
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
                checkAllText: 'All sport.',
                noItemText: 'No sport seleted.',
                noneSelectedText: 'Select sport',
                selectedText: '# selected sport',
                tooltipNoItem: 'There is no sport selected.',
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
                }
            });
        },

        getLeagueData: function () {
            var params = {
                sportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                leagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(',')
            };
            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetLeagueListOtherSports"),
                type: 'post',
                data: params,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.MsgId == 0) {
                        OtherSports.initLeagueMultiSelect(data.List);
                    }
                    else {
                        alert(data.MsgContent);
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
                checkAllText: 'All league.',
                noItemText: 'No league seleted.',
                noneSelectedText: 'Select league',
                selectedText: '# selected leagues',
                tooltipNoItem: 'There is no league selected.',
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    OtherSports.getEventDataByLeague();
                },
                onFooterButtonClick: function () {
                    OtherSports.getEventDataByLeague();
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
                eventList: $("#event-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetEventsByLeaguesOtherSports"),
                type: 'post',
                data: params,
                beforeSend: function () {
                },
                success: function (data) {
                    if (data.MsgId == 0) {
                        $("#event-multi-select").fanexMultiSelect({
                            listData: data.List,
                            showTooltip: true,
                            maxHeight: 500,
                            minWidth: 250,
                            checkAllText: 'All event.',
                            noItemText: 'No event seleted.',
                            noneSelectedText: 'Select event',
                            selectedText: '# selected events',
                            tooltipNoItem: 'There is no event selected.',
                            showParentSelectedText: false,
                            onLeave: function () {
                                OtherSports.reRenderAfterPaged();
                            }
                        });
                        //Load data for the first time load page
                        if (isLoadGrid) {
                            setTimeout("OtherSports.reRenderAfterPaged();", 500);
                        }
                    }
                    else {
                        alert(data.MsgContent);
                    }
                },
                complete: function () {
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
                    if (data.MsgId == 0) {
                        gridOption.bodyRows = OtherSports.genGridRows(data.List, gridOption);
                        $('#grid').show().fanexGrid(gridOption);
                    }
                    else {
                        alert(data.MsgContent);
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
            var leagueId = 0;

            for (var i = 0; i < len; i++) {//League
                var rowIndex = 0;
                var league = list[i];

                //Push League row
                bodyRows.push({
                    Columns: ([
                            { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 9
                            }
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
                        { Name: "Event", Value: match.HomeName, TdClass: "bold noborder" },
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
                        { Name: "Event", Value: match.AwayName, CssClass: "bold" },
                        { Name: "FTHDP", Value: match.AwayFulltimeHandicap },
                        { Name: "FTOU", Value: match.AwayFulltimeOverUnder },
                        { Name: "FT1X2", Value: match.AwayFulltimeOneXTwo },
                        { Name: "FHHDP", Value: match.AwayFirstHalfHandicap },
                        { Name: "FHOU", Value: match.AwayFirstHalfOverUnder },
                        { Name: "FH1X2", Value: match.AwayFirstHalfOneXTwo }
                    );

                    var drawRow = [];
                    drawRow.push(
                        { Name: "Event", Value: "Draw", CssClass: "draw" },
                        { Name: "FTHDP", Value: "", MergeColNumber: 2 },
                        { Name: "FT1X2", Value: match.DrawFulltimeOneXTwo },
                        { Name: "FHHDP", Value: "", MergeColNumber: 2 },
                        { Name: "FT1X2", Value: match.DrawFirstHalfOneXTwo }
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
            return bodyRows;
        }
    }
} (jQuery);
$(function () {
    OtherSports.init();
});