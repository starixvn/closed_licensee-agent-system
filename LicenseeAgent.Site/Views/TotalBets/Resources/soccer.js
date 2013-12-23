var Soccer = function ($) {

    var urlAjax = $('#search-form').attr('action');
    return {
        init: function () {
            // search
            $('#search-form').submit(function (e) {

            });

            Soccer.getLeagueData();
            Soccer.getEventDataByLeague();
        },

        getLeagueData: function () {
            var params = {
                League: $("#League").val()
            };
            $.ajax({
                url: urlAjax + "/GetLeagueList",
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    Soccer.initLeagueMultiSelect(data.List);
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
                minWidth: 300,
                checkAllText: 'All league.',
                noItemText: 'No league seleted.',
                noneSelectedText: 'Select league',
                selectedText: '# selected',
                tooltipNoItem: 'There is no league selected.',
                showParentSelectedText: false,
                onLeave: function () {
                    Soccer.selectedLeague();
                }
            });
        },

        selectedLeague: function () {
            var selectedLeagues = $("#league-multi-select").fanexMultiSelect('getItems', true).join(',');
            $("#League").val(selectedLeagues);
            Soccer.getEventDataByLeague();
        },
                
        getEventDataByLeague: function () {
            var params = {
                League: $("#League").val(),
                Event: $("#Event").val()
            };
            $.ajax({
                url: urlAjax + "/GetEventsByLeagues",
                type: 'post',
                data: params,
                beforeSend: function () {
                    CORE.blockUI();
                },
                success: function (data) {
                    Soccer.initEventMultiSelect(data.List);
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
                selectedText: '# selected',
                tooltipNoItem: 'There is no event selected.',
                showParentSelectedText: false,
                onLeave: function () {
                    Soccer.selectedEvent();
                }
            });
        },

        selectedEvent: function () {
            var selectedLeagues = $("#event-multi-select").fanexMultiSelect('getItems', true).join(',');
            $("#Event").val(selectedLeagues);
        }
    }
} (jQuery);
$(function () {
    Soccer.init();
});