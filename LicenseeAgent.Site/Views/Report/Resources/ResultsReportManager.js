var REPORT = function ($) {
    return {
        init: function () {
            $("#filter-tabs").tabs();
            $('#matchdatetimepicker').datepicker();
            $('#outrightfromdatetimepicker').datepicker();
            $('#outrighttodatetimepicker').datepicker();
            $('.selectpicker').selectpicker();

            REPORT.initData();
        },

        initData: function () {
            var currentTime = new Date();
            var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
            REPORT.matchSubmit(todayStr);

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
                        REPORT.generateLeagues(data.Leagues);

                        REPORT.generateResults(data.Results);
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
                        REPORT.generateResults(data.Results);
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
                        REPORT.generateLeagues(data.Leagues);

                        REPORT.generateResults(data.Results);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },
        
        manageOutrightLeagueSelect: function () {
            var params = {
                FromDate: $("#outrightFromDate").val(),
                ToDate: $("#outrightToDate").val(),
                SportType: $("#OutrightSport").val(),
                LeagueId: $("#OutrightLeague").val(),
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
                    if (data.Results != null) {
                        REPORT.generateResults(data.Results);
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
                    if (data.ErrCode != 0) {
                        return;
                    }
                    if (data.Sports != null) {
                        REPORT.generateSports(data.Sports, viewMatchResults);

                        REPORT.generateLeagues(data.Leagues, viewMatchResults);

                        REPORT.generateResults(data.Results);
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
                listItemsHtml += '<option value="' + sports[index].Value + '">' + sports[index].Text + '</option>';
            }

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
            var listItemsHtml = '<option value="0">Show All Leagues</option>';
            for (var index = 0; index < itemCounter; index++) {
                listItemsHtml += '<option value="' + leagues[index].Value + '">' + leagues[index].Text + '</option>';
            }

            if (viewMatchResults == 1) {
                $("#MatchLeague").html(listItemsHtml);
                $('#MatchLeague').selectpicker('refresh');
            }
            else {
                $("#OutrightLeague").html(listItemsHtml);
                $('#OutrightLeague').selectpicker('refresh');
            }
        },

        generateResults: function (results) {
            if(results.search("<!--NoInfo-->") > 0)
            {
                $( ".outright-dropdownlists" ).hide();
            }
            else
            {
                $( ".outright-dropdownlists" ).show();
            }
            $("#ResultsArea").html(results);
        },

        matchSubmit: function (eventDateString) {
            $('#matchdatetime').val(eventDateString);
            var params = {
                EventDate: eventDateString
            };

            REPORT.updateContent(params, 1);
        },

        outrightSubmit: function (fromDateString, toDateString) {
            $('#outrightFromDate').val(fromDateString);
            $('#outrightToDate').val(toDateString);
            var params = {
                FromDate: fromDateString,
                ToDate: toDateString
            };

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