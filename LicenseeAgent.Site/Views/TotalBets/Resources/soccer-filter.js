var SoccerFilter = function ($) {
    return {        
        init: function () {
            SoccerFilter.initEvent();
        },
        initEvent: function () {
            // League Select
            $("#select-league-text").click(function () {
                EventFilter.hideFilter();
                LeagueFilter.showFilter();
            });
            $("#select-league-button").click(function () {
                EventFilter.hideFilter();
                LeagueFilter.showFilter();
            });
            $("#cancel-league-select").click(function () {
                LeagueFilter.hideFilter();
            });
            $("#ok-league-select").click(function () {
                LeagueFilter.selectedLeagues();
            });

            // Event Select
            $("#select-event-text").click(function () {
                LeagueFilter.hideFilter();
                EventFilter.showFilter();
            });
            $("#select-event-button").click(function () {
                LeagueFilter.hideFilter();
                EventFilter.showFilter();
            });
            $("#cancel-event-select").click(function () {
                EventFilter.hideFilter();
            });
            $("#ok-event-select").click(function () {
                EventFilter.selectedEvents();
            });            
        }
    }
} (jQuery);
$(function () {
    SoccerFilter.init();
});

var LeagueFilter = function ($) {
    return {
        leagues: [],
        leagueParamToGetEvent: '',
        urlGetLeague: $('#search-form').attr('action') + "/GetLeagues",

        showFilter: function () {
            $(".select-league-area").slideDown();
            $('.check-all-league').prop('checked', false);
        },
        hideFilter: function () {
            $(".select-league-area").hide();
        },
        selectedLeagues: function () {
            $(".select-league-area").slideUp();
            var leagueSelectedText = '';
            var leagueSelectedValue = '';
            var isCheckAll = $(".check-all-league").is(':checked');
            if (isCheckAll) {
                leagueSelectedText = 'All League Selected';
            } else {
                var selectedCheckLeagues = $(".select-league-area input.check-child-league:checked");
                var leagueCheckedCount = selectedCheckLeagues.length;
                if (leagueCheckedCount > 1) {
                    leagueSelectedText = leagueCheckedCount + ' Leagues Selected';
                } else {
                    leagueSelectedText = leagueCheckedCount + ' League Selected';
                }

                // set league list to form
                selectedCheckLeagues.each(function (index) {
                    leagueSelectedValue += $(this).attr("data-id") + ",";
                });
                leagueSelectedValue = leagueSelectedValue.substr(0, leagueSelectedValue.length - 1);
                LeagueFilter.leagueParamToGetEvent = leagueSelectedValue;
            };

            $("#league-selected-string").val(leagueSelectedValue);
            $("#select-league-text").val(leagueSelectedText);
        },
        allCheckChanged: function () {
            $('.check-child-league').prop('checked', $(".check-all-league").is(':checked'));
        },
        childCheckChanged: function () {
            var leagueCount = $(".select-league-area input.check-child-league").length;
            var leagueCheckedCount = $(".select-league-area input.check-child-league:checked").length;
            if (leagueCheckedCount == leagueCount) {
                $('.check-all-league').prop('checked', true);
            } else {
                $('.check-all-league').prop('checked', false);
            }
        }
    }
} (jQuery);

var EventFilter = function ($) {
    return {
        events: [],
        urlGetEvent: $('#search-form').attr('action') + "/GetEventsByLeagues",

        showFilter: function () {
            $(".select-event-area").slideDown();
            $('.check-all-event').prop('checked', false);
        },
        hideFilter: function () {
            $(".select-event-area").hide();
        },
        selectedEvents: function () {
            $(".select-event-area").slideUp();
            var eventSelectedText = '';
            var eventSelectedValue = '';
            var isCheckAll = $(".check-all-event").is(':checked');
            if (isCheckAll) {
                eventSelectedText = 'All Events Selected';
            } else {
                var selectedCheckEvent = $(".select-event-area input.check-child-event:checked");
                var eventCheckedCount = selectedCheckEvent.length;
                if (eventCheckedCount > 1) {
                    eventSelectedText = eventCheckedCount + ' Events Selected';
                } else {
                    eventSelectedText = eventCheckedCount + ' Event Selected';
                }

                // set match list to form 
                selectedCheckEvent.each(function (index) {
                    eventSelectedValue += $(this).attr("data-id") + ",";
                });
                eventSelectedValue = eventSelectedValue.substr(0, eventSelectedValue.length - 1);
            };
            $("#event-selected-string").val(eventSelectedValue);
            $("#select-event-text").val(eventSelectedText);
        },
        allCheckChanged: function () {
            $('.check-child-event').prop('checked', $(".check-all-event").is(':checked'));
        },
        childCheckChanged: function () {
            var leagueCount = $(".select-event-area input.check-child-event").length;
            var eventCheckedCount = $(".select-event-area input.check-child-event:checked").length;
            if (eventCheckedCount == leagueCount) {
                $('.check-all-event').prop('checked', true);
            } else {
                $('.check-all-event').prop('checked', false);
            }
        }
    }
} (jQuery);

var SoccerApp = angular.module('SoccerApp', []);
SoccerApp.controller('LeagueCotroller', function ($scope, $http) {
    $scope.leagues = {};
    $scope.loadLeague = function () {
        $http.post(LeagueFilter.urlGetLeague).success(function (data) {
            $scope.leagues = data.List;
        });
    };  
});

SoccerApp.controller('EventCotroller', function ($scope, $http) {
    $scope.events = {};
    $scope.loadEvent = function () {
        $http.post(EventFilter.urlGetEvent + "?leagues=" + LeagueFilter.leagueParamToGetEvent).success(function (data) {
            $scope.events = data.List;
        });
    };
});