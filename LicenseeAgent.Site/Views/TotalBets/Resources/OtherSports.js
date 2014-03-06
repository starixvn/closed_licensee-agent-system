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
                        $('#grid').show().fanexGrid(gridOption);
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
                            { Name: "MoneyLine", Value: match.HomeFulltimeMoneyLine },
                            { Name: "FTHDP", Value: match.HomeFulltimeHandicap },
                            { Name: "StatisticsHDP", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FTOU", Value: match.HomeFulltimeOverUnder },
                            { Name: "StatisticsOU", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FT1X2", Value: match.HomeFulltimeOneXTwo },
                            { Name: "Statistics1X2", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FTOE", Value: match.HomeFulltimeOddEven },
                            { Name: "FHHDP", Value: match.HomeFirstHalfHandicap },
                            { Name: "StatisticsFHHDP", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" },
                            { Name: "FHOU", Value: match.HomeFirstHalfOverUnder },
                            { Name: "StatisticsFHOU", Value: "", MergeRowNumber: 2, CssClass: "totalbets-item" }
                        );

                        var awayTeam = [];
                        //Away team
                        awayTeam.push(
                            { Name: "Event", Value: match.AwayName, TdClass: "team-name" },
                            { Name: "MoneyLine", Value: match.AwayFulltimeMoneyLine },
                            { Name: "FTHDP", Value: match.AwayFulltimeHandicap },
                            { Name: "FTOU", Value: match.AwayFulltimeOverUnder },
                            { Name: "FT1X2", Value: match.AwayFulltimeOneXTwo },
                            { Name: "FTOE", Value: match.AwayFulltimeOddEven },
                            { Name: "FHHDP", Value: match.AwayFirstHalfHandicap },
                            { Name: "FHOU", Value: match.AwayFirstHalfOverUnder }
                        );

                        var drawRow = [];
                        drawRow.push(
                            { Name: "Event", Value: "Draw", TdClass: "draw" },
                            { Name: "MoneyLine", Value: "", MergeColNumber: 5 },
                            { Name: "FT1X2", Value: match.DrawFulltimeOneXTwo },
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
        }
    }
} (jQuery);
$(function () {
    OtherSports.init();
});