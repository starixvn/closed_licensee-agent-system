/*global CORE, LANGUAGE*/

var Report = (function ($) {
    /*jshint strict:false */
    return {
        init: function () {
            $('.selectpicker').selectpicker();
            Report.getBalanceUpMoveBet();
            Report.initData();
        },

        initData: function () {
            var currentTime = new Date();
            var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
            Report.matchSubmit(todayStr);

            $('a[data-toggle="tab"]').on('shown', function (e) {
                if ($(e.target).attr('href') === "#filter-tab-outright") {
                    Report.manageOutrightLeagueSelect();
                }
                else {
                    Report.manageLeagueSelect();
                }
            });

            $("#MatchSport").change(function () {
                Report.manageSportSelect();
            });

            $("#MatchLeague").change(function () {
                Report.manageLeagueSelect();
            });

            $("#OutrightSport").change(function () {
                Report.manageOutrightSportSelect();
            });

            $("#OutrightLeague").change(function () {
                Report.manageOutrightLeagueSelect();
            });
        },

        getBalanceUpMoveBet: function () {
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetBalanceUpMoveBet"),
                type: 'POST',
                dataType: "json",
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        return;
                    }
                    var currentTime = new Date();
                    var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();

                    $('#matchdatetimepicker').datepicker({ startDate: data.BalanceUp, endDate: todayStr });
                    $('#outrightfromdatetimepicker').datepicker({ startDate: data.BalanceUp, endDate: todayStr });
                    $('#outrighttodatetimepicker').datepicker({ startDate: data.BalanceUp, endDate: todayStr });
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        manageSportSelect: function () {
            var params = {
                EventDate: $("#matchdatetime").val(),
                SportType: $("#MatchSport").val()
            };

            $.ajax({
                url: CORE.resolveUrl("~/Report/GetLeagues"),
                type: 'POST',
                data: params,
                dataType: "json",
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        return;
                    }
                    if (data.Leagues != null) {
                        Report.generateLeagues(data.Leagues, 1);

                        Report.generateResults(data.Results, 1);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        manageLeagueSelect: function () {
            var params = {
                EventDate: $("#matchdatetime").val(),
                SportType: $("#MatchSport").val(),
                LeagueId: $("#MatchLeague").val()
            };

            $.ajax({
                url: CORE.resolveUrl("~/Report/GetMatchResults"),
                type: 'POST',
                data: params,
                dataType: "json",
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        return;
                    }
                    if (data.Results != null) {
                        Report.generateResults(data.Results, 1);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        manageOutrightSportSelect: function () {
            var params = {
                FromDate: $("#outrightFromDate").val(),
                ToDate: $("#outrightToDate").val(),
                SportType: $("#OutrightSport").val()
            };

            $.ajax({
                url: CORE.resolveUrl("~/Report/OutrightResults"),
                type: 'POST',
                data: params,
                dataType: "json",
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.ErrCode !== 0) {
                        return;
                    }
                    if (data.Leagues != null) {
                        Report.generateLeagues(data.Leagues, 0);

                        Report.generateResults(data.Results, 0);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        manageOutrightLeagueSelect: function () {
            var sporttype, leagueid;
            sporttype = leagueid = 0;
            if ($("#OutrightSport").val() != null) {
                sporttype = $("#OutrightSport").val();
            }
            if ($("#OutrightLeague").val() != null) {
                leagueid = $("#OutrightLeague").val();
            }
            if (leagueid === "No Data" || sporttype === "No Data") {
                leagueid = 0;
                sporttype = 0;
            }

            var params = {
                FromDate: $("#outrightFromDate").val(),
                ToDate: $("#outrightToDate").val(),
                SportType: sporttype,
                LeagueId: leagueid
            };

            $.ajax({
                url: CORE.resolveUrl("~/Report/OutrightResults"),
                type: 'POST',
                data: params,
                dataType: "json",
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.Results != null) {
                        Report.generateResults(data.Results, 0);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        updateContent: function (params, viewMatchResults) {
            var actionUrl = "~/Report/OutrightResults";
            if (viewMatchResults === 1) {
                actionUrl = "~/Report/Results";
            }

            $.ajax({
                url: CORE.resolveUrl(actionUrl),
                type: 'POST',
                data: params,
                dataType: "json",
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    if (data.Sports != null) {
                        Report.generateSports(data.Sports, viewMatchResults);

                        Report.generateLeagues(data.Leagues, viewMatchResults);

                        Report.generateResults(data.Results, viewMatchResults);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        generateSports: function (sports, viewMatchResults) {
            var itemCounter = sports.length;
            var listItemsHtml = '';
            for (var index = 0; index < itemCounter; index++) {
                var selectedText = sports[index].Selected === true ? 'selected="selected"' : '';
                listItemsHtml += '<option value="' + sports[index].Value + '" ' + selectedText + '>' + sports[index].Text + '</option>';
            }

            if (itemCounter <= 0) {
                listItemsHtml = '<option>No Data</option>';
            }

            if (viewMatchResults === 1) {
                $("#MatchSport").html(listItemsHtml);
                $('#MatchSport').selectpicker('refresh');
            }
            else {
                $("#OutrightSport").html(listItemsHtml);
                $('#OutrightSport').selectpicker('refresh');
            }
        },

        generateLeagues: function (leagues, viewMatchResults) {
            var itemCounter = leagues.length;
            var listItemsHtml = '<option value="0">' + LANGUAGE.getString('AllLeagues') + '</option>';
            for (var index = 0; index < itemCounter; index++) {
                listItemsHtml += '<option value="' + leagues[index].Value + '">' + leagues[index].Text + '</option>';
            }

            if (itemCounter <= 0) {
                listItemsHtml = '<option>No Data</option>';
            }

            if (viewMatchResults === 1) {
                $("#MatchLeague").html(listItemsHtml);
                $('#MatchLeague').selectpicker('refresh');
            }
            else {
                $("#OutrightLeague").html(listItemsHtml);
                $('#OutrightLeague').selectpicker('refresh');
            }
        },

        generateResults: function (results, viewMatchResults) {
            if (viewMatchResults === 0) {
                if (results.search("<!--NoInfo-->") > 0) {
                    $(".hide").hide();
                }
                else {
                    $(".hide").show();
                }
            }
            $("#ResultsArea").html(results);
        },

        matchSubmit: function (eventDateString) {
            $('#matchdatetimepicker').datepicker("update", eventDateString);
            var params = {
                EventDate: eventDateString
            };

            Report.updateContent(params, 1);
        },

        outrightSubmit: function (fromDateString, toDateString) {
            $('#outrightfromdatetimepicker').datepicker("update", fromDateString);
            $('#outrighttodatetimepicker').datepicker("update", toDateString);
            var sportType = $("#OutrightSport").val();
            var params = {
                FromDate: fromDateString,
                ToDate: toDateString
            };

            if (sportType > 0) {
                params.SportType = sportType;
            }

            Report.updateContent(params, 0);
        },

        matchYesterday: function () {
            var currentTime = new Date();
            var yesterdayStr = (currentTime.getMonth() + 1) + "/" + (currentTime.getDate() - 1) + "/" + currentTime.getFullYear();
            Report.matchSubmit(yesterdayStr);
            return false;
        },

        matchToday: function () {
            var currentTime = new Date();
            var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
            Report.matchSubmit(todayStr);
            return false;
        },

        outrightYesterday: function () {
            var currentTime = new Date();
            var yesterdayStr = (currentTime.getMonth() + 1) + "/" + (currentTime.getDate() - 1) + "/" + currentTime.getFullYear();
            Report.outrightSubmit(yesterdayStr, yesterdayStr);
            return false;
        },

        outrightToday: function outrightToday() {
            var currentTime = new Date();
            var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
            Report.outrightSubmit(todayStr, todayStr);
            return false;
        }
    };
} (jQuery));
/*jshint unused:false*/
function matchSubmit() {
    /*jshint strict:false */
    resultReport.matchSubmit($('#matchdatetime').val());
    return false;
}

/*jshint unused:false*/
function outrightSubmit() {
    /*jshint strict:false */
    Report.outrightSubmit($('#outrightFromDate').val(), $('#outrightToDate').val());
    return false;
}

$(function () {
    /*jshint strict:false */
    Report.init();
});