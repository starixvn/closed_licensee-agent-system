var Soccer = function ($) {
    var urlAjax = $('#search-form').attr('action');
    return {
        init: function () {            
            $(".more-link").click(function () {
                Soccer.showAllBetType(this);
            });

            $("#refresh-count-down").refreshCountDown({
                listTimeInterval: [15, 30, 45, 60, 75],
                timeIntervalName: "TimeInterval",
                timeIntervalValue: $("#server-time-interval").val(),
                formName: "main-form"
            });

            Soccer.initData();
            
        },

        initData: function () {
            CORE.blockUI();
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
                },
                success: function (data) {
                    Soccer.initLeagueMultiSelect(data.List);
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
                minWidth: 400,
                checkAllText: 'All league.',
                noItemText: 'No league seleted.',
                noneSelectedText: 'Select league',
                selectedText: '# selected leagues',
                tooltipNoItem: 'There is no league selected.',
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () {
                    Soccer.selectedLeague();
                },
                onFooterButtonClick: function () {
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
                minWidth: 350,
                checkAllText: 'All event.',
                noItemText: 'No event seleted.',
                noneSelectedText: 'Select event',
                selectedText: '# selected events',
                tooltipNoItem: 'There is no event selected.',
                showParentSelectedText: false,
                showFooter: true,
                footerButtonText: 'OK',
                onLeave: function () { Soccer.selectedEvent(); },
                onFooterButtonClick: function () {
                    Soccer.selectedEvent(); 
                }
            });
        },

        selectedEvent: function () {
            var selectedLeagues = $("#event-multi-select").fanexMultiSelect('getItems', true).join(',');
            $("#Event").val(selectedLeagues);
        },

        showAllBetType: function (context) {
            var matchId = $(context).attr("data-matchid");
            var matchName = $(context).attr("data-matchname");
            var url = urlAjax + "/SoccerDetail?matchId=" + matchId + "&matchName=" + matchName;
            window.location.href = url;
        }
    }
} (jQuery);
$(function () {
    Soccer.init();
});