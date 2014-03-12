var Outright = function ($) {
    return {
        checkedAllSport: true,
        checkedAllLeague: true,
        checkedAllEvent: true,
        init: function () {
            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: CORE.TimeInterval,
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            Outright.initData();

            $('form').submit(function () {
                Outright.getSportData();
                Outright.getLeagueDataBySport();
                Outright.getTeamDataByLeague();

                Outright.reRenderAfterPaged();
                //refreshCountDownMethods.resetCountDown($("#server-time-interval").val(), "main-form");
                return false;
            });
        },

        initData: function () {
            Outright.getSportData();
            Outright.getLeagueDataBySport();
            Outright.getTeamDataByLeague(true);
        },

        // ------------------------------------------------ Sport filter -----------------------------------------
        getSportData: function () {
            var params = {
                Sport: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: Outright.checkedAllSport
            };
            $.ajax({
                url: CORE.resolveUrl("~/TotalBets/GetSportList"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    Outright.initSportMultiSelect(data.List);
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
                checkAllText: 'All sports.',
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
                    Outright.getLeagueDataBySport();
                    Outright.getTeamDataByLeague();
                },
                onFooterButtonClick: function () {
                    Outright.getLeagueDataBySport();
                    Outright.getTeamDataByLeague();
                },
                onCheck: function () {
                    Outright.checkedAllSport = false;
                },
                onCheckAll: function () {
                    Outright.checkedAllSport = true;
                }
            });
        },

        // ------------------------------------------------ League filter -----------------------------------------
        getLeagueDataBySport: function () {
            var params = {
                Sport: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: Outright.checkedAllLeague
            };
            $.ajax({
                url: CORE.resolveUrl("~/TotalBets/GetLeaguesBySports"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    //CORE.blockUI();
                },
                success: function (data) {
                    Outright.initLeagueMultiSelect(data.List);
                },
                complete: function () {
                    //CORE.unblockUI();
                }
            });
        },

        initLeagueMultiSelect: function (leagueList) {
            $("#league-multi-select").fanexMultiSelect({
                listData: leagueList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 270,
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
                    Outright.getTeamDataByLeague();
                },
                onFooterButtonClick: function () {
                    Outright.getTeamDataByLeague();
                },
                onCheck: function () {
                    Outright.checkedAllLeague = false;
                },
                onCheckAll: function () {
                    Outright.checkedAllLeague = true;
                }
            });
        },

        // ------------------------------------------------ Team filter -----------------------------------------
        getTeamDataByLeague: function (isLoadGrid) {
            var params = {
                Sport: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                League: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                Team: $("#team-multi-select").fanexMultiSelect('getItems', true).join(','),
                checkedAll: Outright.checkedAllEvent
            };
            $.ajax({
                url: CORE.resolveUrl("~/TotalBets/GetTeamsByLeagues"),
                type: 'post',
                data: params,
                async: false,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    Outright.initEventMultiSelect(data.List);

                    // Load data for the first time load page
                    if (isLoadGrid) {
                        Outright.reRenderAfterPaged();
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        initEventMultiSelect: function (eventList) {
            $("#team-multi-select").fanexMultiSelect({
                listData: eventList,
                showTooltip: true,
                maxHeight: 500,
                minWidth: 250,
                checkAllText: 'All teams.',
                noItemText: 'No team seleted.',
                noneSelectedText: 'Select team',
                selectedText: '# selected teams',
                tooltipNoItem: 'There is no team selected.',
                useCustomCheckAllText: true,
                customCheckAllText: 'All teams',
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    //Outright.selectedEvent(); 
                },
                onFooterButtonClick: function () {
                    //Outright.selectedEvent();
                },
                onCheck: function () {
                    Outright.checkedAllEvent = false;
                },
                onCheckAll: function () {
                    Outright.checkedAllEvent = true;
                }
            });
        },

        // ------------------------------------------------ Refresh data -----------------------------------------
        reRenderAfterPaged: function () {
            var params = {
                SportList: $("#sport-multi-select").fanexMultiSelect('getItems', true).join(','),
                LeagueList: $("#league-multi-select").fanexMultiSelect('getItems', true).join(','),
                TeamList: $("#team-multi-select").fanexMultiSelect('getItems', true).join(',')
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetOutrightData"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    gridOption.bodyRows = Outright.genGridRows(data.List, gridOption);
                    $('#BetListGrid').show().fanexGrid(gridOption);
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

            for (var k = 0; k < len; k++) { //Sport
                var sport = list[k];

                //Push Sport row
                bodyRows.push({
                    Columns: ([
                            { Name: "RowID", Value: "<div align='left'>" + sport.SportName + "</div>", MergeColNumber: 5 }
                        ]),
                    CssClass: "sport-row"
                });

                var leagues = sport.LeagueList;
                var numLeagues = leagues.length;
                for (var i = 0; i < numLeagues; i++) {//League

                    var league = leagues[i];
                    //Push League row
                    bodyRows.push({
                        Columns: ([
                            { Name: "RowID", Value: "<div align='left'>" + league.LeagueName + "</div>", MergeColNumber: 5 }
                        ]),
                        CssClass: "league-row"
                    });

                    //Push Team Row
                    var teams = league.TeamList;
                    var numTeams = teams.length;

                    for (var j = 0; j < numTeams; j++) {
                        var match = teams[j];
                        var teamRow = [];
                        rowIndex++;
                        //Home team
                        teamRow.push(
                            { Name: "RowID", Value: "<div align='center'>" + rowIndex + "</div>" },
                            { Name: "Team", Value: Outright.generateLinkValue(league.LeagueId, match.TeamId, match.TeamName), CssClass: "bold" },
                            { Name: "BetCount", Value: match.BetCount },
                            { Name: "Balance", Value: match.Balance },
                            { Name: "Forecast", Value: match.Forecast }
                        );

                        bodyRows.push({
                            Columns: teamRow
                        });

                    }
                }
            }
            return bodyRows;
        },
        showBetList: function (leagueId, teamId) {
            refreshCountDownMethods.stopCountDown();
            $("#totalbets-filter").attr("class", "hide");
            $("#totalbets-back").attr("class", "history-back");

            var params = {
                LeagueId: leagueId,
                TeamId: teamId
            };

            $.ajax({
                url: CORE.resolveUrl("~/Totalbets/GetOutrightBetList"),
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
        generateLinkValue: function (leagueId, teamId, linkvalue) {
            var linkHtmlString = "<span class=\"data-link\" onclick=\"Outright.showBetList(";
            linkHtmlString += leagueId + "," + teamId + ")\">";
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
    Outright.init();
});