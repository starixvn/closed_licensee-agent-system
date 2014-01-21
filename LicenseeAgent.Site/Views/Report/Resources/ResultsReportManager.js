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
            REPORT.matchToday();

            $("#MatchSport").change(function () {
                REPORT.manageSportSelect();
            });

            $("#MatchLeague").change(function () {
                REPORT.manageLeagueSelect();
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
                url: CORE.resolveUrl("~/Report/GetResults"),
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

                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        updateContent: function (params) {
            $.ajax({
                url: CORE.resolveUrl("~/Report/Results"),
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
                        REPORT.generateSports(data.Sports);

                        REPORT.generateLeagues(data.Leagues);

                        REPORT.generateResults(data.Results);
                    }
                },
                complete: function () {
                    CORE.unblockUI();
                }
            });
        },

        generateSports: function (sports) {
            var itemCounter = sports.length;
            var listItemsHtml = '';
            for (var index = 0; index < itemCounter; index++) {
                listItemsHtml += '<option value="' + sports[index].Value + '">' + sports[index].Text + '</option>';
            }
            $("#MatchSport").html(listItemsHtml);
            $('#MatchSport').selectpicker('refresh');
        },

        generateLeagues: function (leagues) {
            var itemCounter = leagues.length;
            var listItemsHtml = '';
            for (var index = 0; index < itemCounter; index++) {
                listItemsHtml += '<option value="' + leagues[index].Value + '">' + leagues[index].Text + '</option>';
            }
            $("#MatchLeague").html(listItemsHtml);
            $('#MatchLeague').selectpicker('refresh');
        },

        generateResults: function (results) {
            $("#ResultsArea").html(results);
        },

        matchSubmit: function () {
            var params = {
                EventDate: $("#matchdatetime").val()
            };

            REPORT.updateContent(params);
        },

        matchYesterday: function () {
            var currentTime = new Date();
            var yesterdayStr = (currentTime.getMonth() + 1) + "/" + (currentTime.getDate() - 1) + "/" + currentTime.getFullYear();
            $('#matchdatetime').val(yesterdayStr);
            var params = {
                EventDate: yesterdayStr
            };

            REPORT.updateContent(params);
        },

        matchToday: function () {
            var currentTime = new Date();
            var todayStr = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
            $('#matchdatetime').val(todayStr);
            var params = {
                EventDate: todayStr
            };

            REPORT.updateContent(params);
        }
    }
} (jQuery);

/* Match Resuls */
function matchSubmit()
{
    REPORT.matchSubmit();
    return false;
}

function matchYesterday() {
    REPORT.matchYesterday();
    return false;
}

function matchToday() {
    REPORT.matchToday();
    return false;
}

/* Outright Results*/

$(function () {
    REPORT.init();
});