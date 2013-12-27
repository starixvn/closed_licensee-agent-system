var Basketball = function ($) {
    return {
        sportType: 2, //Basketball sporttype
        init: function () {
            //alert(CORE.resolveUrl("/Totalbets/GetLeagueList"))
            //            $("#refresh-count-down").refreshCountDown({
            //                listTimeInterval: CORE.TimeInterval,
            //                timeIntervalName: "TimeInterval",
            //                timeIntervalValue: $("#server-time-interval").val(),
            //                formName: "main-form"
            //            });

            // search
            $('form').submit(function () {
                Basketball.reRenderAfterPaged();
                return false;
            });

            Basketball.initData();
            //Basketball.reRenderAfterPaged();
        },

        initData: function () {
            //CORE.blockUI();
            Basketball.getLeagueData();
            Basketball.getEventDataByLeague();
            //CORE.unblockUI();
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
                onLeave: function () {
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
                    //CORE.blockUI();
                },
                success: function (data) {
                    Basketball.initEventMultiSelect(data.List);
                    Basketball.reRenderAfterPaged();
                },
                complete: function () {
                    //CORE.unblockUI();
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
                    Basketball.reRenderAfterPaged();
                }

            });
        },

        selectedEvent: function () {
            var selectedLeagues = $("#event-multi-select").fanexMultiSelect('getItems', true).join(',');
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
                    //CORE.blockUI();
                },
                success: function (data) {
                    gridOption.bodyRows = Basketball.genGridRows(data.List, gridOption);
                    $('#grid').show().fanexGrid(gridOption);
                    //customerList.gridAfterBinding();
                    //customerList.resizeBox();
                },
                complete: function () {
                    //CORE.unblockUI();
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
                            { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 9 }
                        ]),
                    CssClass: rowCssClass ? rowCssClass(rowIndex, league) : undefined,
                    CustomCellRender: function (cell, cellFormated) {
                        if (col.Formatter)
                            return col.Formatter(rIndex, rowData, cell, cellFormated);
                        else
                            return cellFormated;
                    }
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
                        { Name: "Time", Value: match.Time, MergeRowNumber: 2 },
                        { Name: "Event", Value: match.HomeName },
                        { Name: "FTHDP", Value: match.HomeFullTimeHandicap },
                        { Name: "FTOU", Value: match.HomeFullTimeOverUnder },
                        { Name: "FTOE", Value: match.HomeFullTimeOddEven },
                        { Name: "FHHDP", Value: match.HomeFirstHaflHandicap },
                        { Name: "FHOU", Value: match.HomeFirstHaflOverUnder },
                        { Name: "FHOE", Value: match.HomeFirstHaflOddEven }
                    );

                    var awayTeam = [];
                    //Away team
                    awayTeam.push(
                        { Name: "Event", Value: match.AwayName },
                        { Name: "FTHDP", Value: match.AwayFullTimeHandicap },
                        { Name: "FTOU", Value: match.AwayFullTimeOverUnder },
                        { Name: "FTOE", Value: match.AwayFullTimeOddEven },
                        { Name: "FHHDP", Value: match.AwayFirstHaflHandicap },
                        { Name: "FHOU", Value: match.AwayFirstHaflOverUnder },
                        { Name: "FHOE", Value: match.AwayFirstHaflOddEven }
                    );

                    bodyRows.push({
                        Columns: homeTeam,
                        CssClass: rowCssClass ? rowCssClass(rowIndex, record) : undefined,
                        RowHeight: rowHeight ? rowHeight(rowIndex, record) : undefined
                    });
                    bodyRows.push({
                        Columns: awayTeam,
                        CssClass: rowCssClass ? rowCssClass(rowIndex, record) : undefined,
                        RowHeight: rowHeight ? rowHeight(rowIndex, record) : undefined
                    });

                    /*for (var k = 0; k < columns.length; k++) {

                        (function () {
                            var rIndex = rowIndex;
                            var rowData = record;
                            var col = columns[k];
                            var colName = col.Name;

                            dataRow.push({
                                Name: colName,
                                Value: (indexNumberColumn == k) ? rIndex + 1 : record[colName],
                                CustomCellRender: function (cell, cellFormated) {
                                    if (col.Formatter)
                                        return col.Formatter(rIndex, rowData, cell, cellFormated);
                                    else
                                        return cellFormated;
                                }
                            });
                        } ());
                    }*/

                }
            }
            return bodyRows;
        }
    }
} (jQuery);
$(function () {
    Basketball.init();
});