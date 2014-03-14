var REPORT = function ($) {
    return {
        init: function () {
            $('.selectpicker').selectpicker();
            REPORT.getBalanceUpMoveBet();
            REPORT.initData();
        },

        initData: function () {
            var currentTime = new Date();
            var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
            REPORT.matchSubmit(todayStr);

            $('a[data-toggle="tab"]').on('shown', function (e) {
                if ($(e.target).attr('href') == "#filter-tab-outright") {
                    REPORT.manageOutrightLeagueSelect();
                }
                else {
                    REPORT.manageLeagueSelect();
                }
            });

            $("#MatchSport").change(function () {
                REPORT.manageSportSelect();
            });

            $("#MatchLeague").change(function () {
                REPORT.manageLeagueSelect();
            });

            $("#OutrightSport").change(function () {
                REPORT.manageOutrightSportSelect();
            });

            $("#OutrightLeague").change(function () {
                REPORT.manageOutrightLeagueSelect();
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
                    if (data.ErrCode != 0) {
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
                    if (data.ErrCode != 0) {
                        return;
                    }
                    if (data.Leagues != null) {
                        REPORT.generateLeagues(data.Leagues, 1);

                        REPORT.generateResults(data.Results, 1);
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
                    if (data.ErrCode != 0) {
                        return;
                    }
                    if (data.Results != null) {
                        REPORT.generateResults(data.Results, 1);
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
                    if (data.ErrCode != 0) {
                        return;
                    }
                    if (data.Leagues != null) {
                        REPORT.generateLeagues(data.Leagues, 0);

                        REPORT.generateResults(data.Results, 0);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        manageOutrightLeagueSelect: function () {
            var sporttype = leagueid = 0;
            if ($("#OutrightSport").val() != null)
                sporttype = $("#OutrightSport").val();
            if ($("#OutrightLeague").val() != null)
                leagueid = $("#OutrightLeague").val();
            if (leagueid == "No Data" || sporttype == "No Data") {
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
                        REPORT.generateResults(data.Results, 0);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        updateContent: function (params, viewMatchResults) {
            var actionUrl = "~/Report/OutrightResults";
            if (viewMatchResults == 1)
                actionUrl = "~/Report/Results";

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
                        REPORT.generateSports(data.Sports, viewMatchResults);

                        REPORT.generateLeagues(data.Leagues, viewMatchResults);

                        REPORT.generateResults(data.Results, viewMatchResults);
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
                var selectedText = sports[index].Selected == true ? 'selected="selected"' : '';
                listItemsHtml += '<option value="' + sports[index].Value + '" ' + selectedText + '>' + sports[index].Text + '</option>';
            }

            if (itemCounter <= 0)
                listItemsHtml = '<option>No Data</option>';

            if (viewMatchResults == 1) {
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

            if (itemCounter <= 0)
                listItemsHtml = '<option>No Data</option>';

            if (viewMatchResults == 1) {
                $("#MatchLeague").html(listItemsHtml);
                $('#MatchLeague').selectpicker('refresh');
            }
            else {
                $("#OutrightLeague").html(listItemsHtml);
                $('#OutrightLeague').selectpicker('refresh');
            }
        },

        generateResults: function (results, viewMatchResults) {
            if (viewMatchResults == 0) {
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

            REPORT.updateContent(params, 1);
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

            REPORT.updateContent(params, 0);
        }
    }
} (jQuery);

/* Match Resuls */
function matchSubmit() {
    REPORT.matchSubmit($('#matchdatetime').val());
    return false;
}

function matchYesterday() {
    var currentTime = new Date();
    var yesterdayStr = (currentTime.getMonth() + 1) + "/" + (currentTime.getDate() - 1) + "/" + currentTime.getFullYear();
    REPORT.matchSubmit(yesterdayStr);
    return false;
}

function matchToday() {
    var currentTime = new Date();
    var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
    REPORT.matchSubmit(todayStr);
    return false;
}

/* Outright Resuls */
function outrightSubmit() {
    REPORT.outrightSubmit($('#outrightFromDate').val(), $('#outrightToDate').val());
    return false;
}

function outrightYesterday() {
    var currentTime = new Date();
    var yesterdayStr = (currentTime.getMonth() + 1) + "/" + (currentTime.getDate() - 1) + "/" + currentTime.getFullYear();
    REPORT.outrightSubmit(yesterdayStr, yesterdayStr);
    return false;
}

function outrightToday() {
    var currentTime = new Date();
    var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
    REPORT.outrightSubmit(todayStr, todayStr);
    return false;
}

/* Outright Results*/

$(function () {
    REPORT.init();
});