var Basketball = function ($) {
    return {
        sportType: 1, //Basketball sporttype
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
            Basketball.getEventDataByLeague();
            //Waiting 1 second for loading league and events
            setTimeout("Basketball.reRenderAfterPaged();", 1000);
            CORE.unblockUI();
        },

        getLeagueData: function () {
            var params = {
                League: $("#League").val(),
                sportType: Basketball.sportType
            };
            $.ajax({
                url: CORE.resolveUrl("/Totalbets/GetLeagueList"),
                type: 'post',
                data: params,
                beforeSend: function () {
                },
                success: function (data) {
                    Basketball.initLeagueMultiSelect(data.List);
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
                checkAllText: 'All league.',
                noItemText: 'No league seleted.',
                noneSelectedText: 'Select league',
                selectedText: '# selected leagues',
                tooltipNoItem: 'There is no league selected.',
                showParentSelectedText: false,
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

        getEventDataByLeague: function () {
            var params = {
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                Event: $("#event-multi-select").fanexMultiSelect('getItems', true).join(','),
                sportType: Basketball.sportType
            };

            $.ajax({
                url: CORE.resolveUrl("/Totalbets/GetEventsByLeagues"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    Basketball.initEventMultiSelect(data.List);
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
                minWidth: 250,
                checkAllText: 'All event.',
                noItemText: 'No event seleted.',
                noneSelectedText: 'Select event',
                selectedText: '# selected events',
                tooltipNoItem: 'There is no event selected.',
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
                url: CORE.resolveUrl("/Totalbets/GetBasketballData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    gridOption.bodyRows = Basketball.genGridRows(data.List, gridOption);
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
                        { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>", MergeRowNumber: 2 },
                        { Name: "Time", Value: match.Time, MergeRowNumber: 2, CssClass: "bold" },
                        { Name: "Event", Value: match.HomeName, TdClass: "bold noborder" },
                        { Name: "FTHDP", Value: match.HomeFulltimeHandicap },
                        { Name: "FTOU", Value: match.HomeFulltimeOverUnder },
                        { Name: "FTOE", Value: match.HomeFulltimeOddEven },
                        { Name: "FHHDP", Value: match.HomeFirstHalfHandicap },
                        { Name: "FHOU", Value: match.HomeFirstHalfOverUnder },
                        { Name: "FHOE", Value: match.HomeFirstHalfOddEven }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName, CssClass: "bold" },
                        { Name: "FTHDP", Value: match.AwayFulltimeHandicap },
                        { Name: "FTOU", Value: match.AwayFulltimeOverUnder },
                        { Name: "FTOE", Value: match.AwayFulltimeOddEven },
                        { Name: "FHHDP", Value: match.AwayFirstHalfHandicap },
                        { Name: "FHOU", Value: match.AwayFirstHalfOverUnder },
                        { Name: "FHOE", Value: match.AwayFirstHalfOddEven }
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
        }
    }
} (jQuery);
$(function () {
    Basketball.init();
});