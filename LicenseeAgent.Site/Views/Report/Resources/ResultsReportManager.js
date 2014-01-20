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
            //REPORT.getLeagueDataBySport();
            //REPORT.getEventDataByLeague(true);
        },

        getLeagueDataBySport: function () {
            var params = {
                EventDate: $("#matchdatetime").val()
            };
            $.ajax({
                url: CORE.resolveUrl("~/Report/GetSports"),
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function () {
                },
                complete: function () {
                    CORE.unblockUI();
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
                    REPORT.selectedLeague();
                },
                onFooterButtonClick: function () {
                    REPORT.selectedLeague();
                }
            });
        },

        selectedLeague: function () {
            var selectedLeagues = $("#league-multi-select").fanexMultiSelect('getItems', true).join(',');
            $("#League").val(selectedLeagues);
            Soccer.getEventDataByLeague();
        }

    }
} (jQuery);

$(function () {
    REPORT.init();
});